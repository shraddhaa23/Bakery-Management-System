using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace BakeryShop
{
    public partial class Checkout : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetNoStore();
            if (!IsPostBack)
            {
                if (Session["CustomerId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    LoadCustomerDetails();
                    LoadCartSummary();
                    btnConfirmOrder.Enabled = false;
                }
            }
        }

        private void LoadCustomerDetails()
        {
            string customerId = Session["CustomerId"].ToString();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT CONCAT(FirstName, ' ', LastName) AS FullName, Email, PhoneNumber FROM customer WHERE CustomerId = @CustomerId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblCustomerName.Text = "Name: " + reader["FullName"].ToString();
                        lblCustomerEmail.Text = "Email: " + reader["Email"].ToString();
                        lblCustomerPhone.Text = "Phone: " + reader["PhoneNumber"].ToString();
                    }
                }
            }
        }

        private void LoadCartSummary()
        {
            string customerId = Session["CustomerId"].ToString();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = @"SELECT COUNT(*) AS TotalItems, SUM(p.Price * c.Quantity) AS Subtotal
                                 FROM cart c
                                 JOIN bakeryproducts p ON c.ProductId = p.ProductId
                                 WHERE c.CustomerId = @CustomerId AND c.status = 'active'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        object totalItemsObj = reader["TotalItems"];
                        object subtotalObj = reader["Subtotal"];

                        int totalItems = (totalItemsObj == DBNull.Value) ? 0 : Convert.ToInt32(totalItemsObj);
                        decimal subtotal = (subtotalObj == DBNull.Value) ? 0.00m : Convert.ToDecimal(subtotalObj);

                        lblTotalItems.Text = "Total Items: " + totalItems.ToString();
                        lblSubtotal.Text = "Subtotal: ₹" + subtotal.ToString("N2");
                    }
                }
            }
        }

        protected void chkCashOnDelivery_CheckedChanged(object sender, EventArgs e)
        {
            btnConfirmOrder.Enabled = chkCashOnDelivery.Checked;
        }

        protected void btnConfirmOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["CustomerId"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }
                if (!ValidateInput())
                {
                    return; // Stop execution if validation fails
                }

                string customerId = Session["CustomerId"].ToString();
                string address = $"{txtFlat.Text}, {txtLocality.Text}, {txtLandmark.Text}, {txtPincode.Text}, {txtCity.Text}, {txtState.Text}";
                string orderStatus = "Pending";
                decimal totalAmount;
                string orderId = string.Empty;
                string invoiceId = string.Empty;

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Check stock availability before confirming order
                    string checkStockQuery = @"SELECT c.ProductId, c.Quantity, p.StockQuantity 
                                       FROM cart c 
                                       JOIN bakeryproducts p ON c.ProductId = p.ProductId 
                                       WHERE c.CustomerId = @CustomerId AND c.status = 'active'";
                    MySqlCommand checkStockCmd = new MySqlCommand(checkStockQuery, conn);
                    checkStockCmd.Parameters.AddWithValue("@CustomerId", customerId);
                    MySqlDataReader stockReader = checkStockCmd.ExecuteReader();

                    List<string> outOfStockProducts = new List<string>();
                    Dictionary<string, int> productStockUpdates = new Dictionary<string, int>();

                    while (stockReader.Read())
                    {
                        string productId = stockReader["ProductId"].ToString();
                        int requestedQty = Convert.ToInt32(stockReader["Quantity"]);
                        int availableQty = Convert.ToInt32(stockReader["StockQuantity"]);

                        if (requestedQty > availableQty)
                        {
                            outOfStockProducts.Add(productId);
                        }
                        else
                        {
                            productStockUpdates[productId] = availableQty - requestedQty;
                        }
                    }
                    stockReader.Close();

                    if (outOfStockProducts.Count > 0)
                    {
                        lblDebug.Text = "Some items are out of stock. Please update your cart.";
                        lblDebug.Visible = true;
                        return;
                    }

                    // Begin transaction
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Calculate Total Amount
                            string totalQuery = @"SELECT SUM(p.Price * c.Quantity) AS TotalAmount
                              FROM cart c
                              JOIN bakeryproducts p ON c.ProductId = p.ProductId
                              WHERE c.CustomerId = @CustomerId AND c.status = 'active'";
                            MySqlCommand totalCmd = new MySqlCommand(totalQuery, conn, transaction);
                            totalCmd.Parameters.AddWithValue("@CustomerId", customerId);
                            object result = totalCmd.ExecuteScalar();
                            totalAmount = result != DBNull.Value ? Convert.ToDecimal(result) : 0;

                            // Insert Order
                            string orderQuery = @"INSERT INTO orders (CustomerId, OrderDate, OrderStatus, TotalAmount, DeliveryAddress)
                              VALUES (@CustomerId, NOW(), @OrderStatus, @TotalAmount, @DeliveryAddress)";
                            MySqlCommand orderCmd = new MySqlCommand(orderQuery, conn, transaction);
                            orderCmd.Parameters.AddWithValue("@CustomerId", customerId);
                            orderCmd.Parameters.AddWithValue("@OrderStatus", orderStatus);
                            orderCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                            orderCmd.Parameters.AddWithValue("@DeliveryAddress", address);
                            int rowsAffected = orderCmd.ExecuteNonQuery();

                            if (rowsAffected == 0)
                            {
                                throw new Exception("Failed to insert order.");
                            }

                            // Retrieve OrderId
                            string getOrderIdQuery = @"SELECT OrderId FROM orders 
                           WHERE CustomerId = @CustomerId 
                           ORDER BY OrderDate DESC LIMIT 1";

                            MySqlCommand getOrderIdCmd = new MySqlCommand(getOrderIdQuery, conn);
                            getOrderIdCmd.Parameters.AddWithValue("@CustomerId", customerId);

                            object orderIdResult = getOrderIdCmd.ExecuteScalar();  // Execute the query

                            // Check if OrderId is valid
                            if (orderIdResult == null || orderIdResult == DBNull.Value)  // Check for null or database NULL
                            {
                                lblDebug.Text = "Error: OrderId not generated. Order insertion failed.";
                                lblDebug.Visible = true;
                                return;
                            }

                            orderId = orderIdResult.ToString().Trim();  // Convert to string only if not null

                            // Insert Order Items
                            string insertOrderDetailsQuery = @"INSERT INTO orderitems (OrderId, ProductId, Quantity, Price, TotalPrice)
                                           SELECT @OrderId, c.ProductId, c.Quantity, p.Price, (c.Quantity * p.Price)
                                           FROM cart c
                                           JOIN bakeryproducts p ON c.ProductId = p.ProductId
                                           WHERE c.CustomerId = @CustomerId AND c.status = 'active'";
                            MySqlCommand insertOrderDetailsCmd = new MySqlCommand(insertOrderDetailsQuery, conn, transaction);
                            insertOrderDetailsCmd.Parameters.AddWithValue("@OrderId", orderId);
                            insertOrderDetailsCmd.Parameters.AddWithValue("@CustomerId", customerId);
                            insertOrderDetailsCmd.ExecuteNonQuery();

                            // Deduct stock
                            foreach (var entry in productStockUpdates)
                            {
                                string updateStockQuery = @"UPDATE bakeryproducts 
                                        SET StockQuantity = @NewStock 
                                        WHERE ProductId = @ProductId";
                                MySqlCommand updateStockCmd = new MySqlCommand(updateStockQuery, conn, transaction);
                                updateStockCmd.Parameters.AddWithValue("@NewStock", entry.Value);
                                updateStockCmd.Parameters.AddWithValue("@ProductId", entry.Key);
                                updateStockCmd.ExecuteNonQuery();
                            }

                            // Insert into Invoice table
                            string invoiceQuery = @"INSERT INTO Invoice (OrderId, CustomerId, InvoiceDate)
                                VALUES (@OrderId, @CustomerId, NOW())";
                            MySqlCommand invoiceCmd = new MySqlCommand(invoiceQuery, conn, transaction);
                            invoiceCmd.Parameters.AddWithValue("@OrderId", orderId);
                            invoiceCmd.Parameters.AddWithValue("@CustomerId", customerId);
                            invoiceCmd.ExecuteNonQuery();

                            // Retrieve InvoiceId
                            string getInvoiceIdQuery = @"SELECT InvoiceId FROM Invoice 
                             WHERE OrderId = @OrderId 
                             ORDER BY InvoiceDate DESC LIMIT 1";

                            MySqlCommand getInvoiceIdCmd = new MySqlCommand(getInvoiceIdQuery, conn, transaction);
                            getInvoiceIdCmd.Parameters.AddWithValue("@OrderId", orderId);  // Use OrderId, NOT CustomerId

                            object invoiceIdResult = getInvoiceIdCmd.ExecuteScalar();  // Execute the query

                            // Check if InvoiceId is valid
                            if (invoiceIdResult == null || invoiceIdResult == DBNull.Value)  // Check for null or database NULL
                            {
                                lblDebug.Text = "Error: InvoiceId not generated. Order insertion failed.";
                                lblDebug.Visible = true;
                                return;
                            }

                            invoiceId = invoiceIdResult.ToString().Trim();  // Convert to string only if not null

                            // Update Cart
                            string updateCartQuery = "UPDATE cart SET status = 'checked_out' WHERE CustomerId = @CustomerId AND status = 'active'";
                            MySqlCommand updateCartCmd = new MySqlCommand(updateCartQuery, conn, transaction);
                            updateCartCmd.Parameters.AddWithValue("@CustomerId", customerId);
                            updateCartCmd.ExecuteNonQuery();

                            // Commit transaction
                            transaction.Commit();

                            lblDebug.Text = "Order placed successfully!";
                            lblDebug.Visible = true;

                            // Redirect to Bill page with InvoiceId
                            Response.Redirect($"Bill.aspx?InvoiceId={invoiceId}");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            lblDebug.Text = "Error processing order: " + ex.Message + "<br/>" + ex.StackTrace;
                            lblDebug.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblDebug.Text = "Error: " + ex.Message;
                lblDebug.Visible = true;
                lblError.Text = "Error: " + ex.Message;
                lblError.Visible = true;
            }
        }

        private bool ValidateInput()
        {
            lblError.Text = ""; // Clear previous errors
            lblError.Visible = false;

            // Check if any field is empty
            if (string.IsNullOrWhiteSpace(txtFlat.Text) ||
                string.IsNullOrWhiteSpace(txtLocality.Text) ||
                string.IsNullOrWhiteSpace(txtLandmark.Text) ||
                string.IsNullOrWhiteSpace(txtPincode.Text) ||
                string.IsNullOrWhiteSpace(txtCity.Text) ||
                string.IsNullOrWhiteSpace(txtState.Text))
            {
                lblError.Text = "All address fields are required.";
                lblError.Visible = true;
                return false;
            }

            // Validate the total length of all fields combined
            int totalLength = txtFlat.Text.Length + txtLocality.Text.Length + txtLandmark.Text.Length +
                              txtPincode.Text.Length + txtCity.Text.Length + txtState.Text.Length;
            if (totalLength > 255)
            {
                lblError.Text = "The total length of all fields must not exceed 255 characters.";
                lblError.Visible = true;
                return false;
            }

            // Validate Pincode (6-digit number)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPincode.Text, @"^\d{6}$"))
            {
                lblError.Text = "Pincode must be a 6-digit number.";
                lblError.Visible = true;
                return false;
            }

            // Validate Flat No/Building Name (Allow letters, numbers, spaces, hyphens, and slashes)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtFlat.Text, @"^[a-zA-Z0-9\s\-/]+$"))
            {
                lblError.Text = "Flat No / Building Name contains invalid characters.";
                lblError.Visible = true;
                return false;
            }

            // Validate Locality/Area/Street (Allow letters, numbers, and spaces)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtLocality.Text, @"^[a-zA-Z0-9\s]+$"))
            {
                lblError.Text = "Locality / Area / Street contains invalid characters.";
                lblError.Visible = true;
                return false;
            }

            // Validate Landmark (Allow letters, numbers, spaces, and special characters like '-' and ',')
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtLandmark.Text, @"^[a-zA-Z0-9\s\-/,]+$"))
            {
                lblError.Text = "Landmark contains invalid characters.";
                lblError.Visible = true;
                return false;
            }

            // Validate City (Allow only alphabetic characters and spaces)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtCity.Text, @"^[a-zA-Z\s]+$"))
            {
                lblError.Text = "City must contain only alphabetic characters.";
                lblError.Visible = true;
                return false;
            }

            // Validate State (Allow only alphabetic characters and spaces)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtState.Text, @"^[a-zA-Z\s]+$"))
            {
                lblError.Text = "State must contain only alphabetic characters.";
                lblError.Visible = true;
                return false;
            }

            return true; // If all validations pass
        }
    }
}

            