using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BakeryShop
{
    public partial class Cart : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CustomerId"] == null)
                {
                    pnlNotLoggedIn.Visible = true;
                    pnlEmptyCart.Visible = false;
                    gvCart.Visible = false;
                }
                else
                {
                    LoadCart();
                    CheckCartStock();
                }
            }
        }

        protected void gvCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string productId = e.CommandArgument.ToString();
            string customerId = Session["CustomerId"]?.ToString();

            if (string.IsNullOrEmpty(customerId))
                return;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                try
                {
                    if (e.CommandName == "IncreaseQuantity")
                    {
                        string stockQuery = "SELECT StockQuantity FROM bakeryproducts WHERE ProductId = @ProductId";
                        using (MySqlCommand stockCmd = new MySqlCommand(stockQuery, conn))
                        {
                            stockCmd.Parameters.AddWithValue("@ProductId", productId);
                            int stockQuantity = Convert.ToInt32(stockCmd.ExecuteScalar());

                            // Get current quantity
                            string checkQuery = @"SELECT Quantity FROM cart WHERE CustomerId = @CustomerId AND ProductId = @ProductId  AND status = 'active'";
                            using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                            {
                                checkCmd.Parameters.AddWithValue("@CustomerId", customerId);
                                checkCmd.Parameters.AddWithValue("@ProductId", productId);
                                object result = checkCmd.ExecuteScalar();

                                if (result != null && result != DBNull.Value)
                                {
                                    int currentQuantity = Convert.ToInt32(result);

                                    // Only increase if there's stock available
                                    if (currentQuantity < stockQuantity)
                                    {
                                        string updateQuery = @"UPDATE cart 
                                           SET Quantity = Quantity + 1 
                                           WHERE CustomerId = @CustomerId AND ProductId = @ProductId AND status = 'active'";
                                        ExecuteQuery(conn, updateQuery, customerId, productId);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Cannot increase quantity. No more stock available.');", true);
                                    }
                                }
                            }
                        }
                    }
                    if (e.CommandName == "DecreaseQuantity")
                    {
                        string checkQuery = @"SELECT Quantity FROM cart 
                          WHERE CustomerId = @CustomerId AND ProductId = @ProductId AND status = 'active'";

                        using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@CustomerId", customerId);
                            checkCmd.Parameters.AddWithValue("@ProductId", productId);
                            object result = checkCmd.ExecuteScalar();

                            if (result != null && result != DBNull.Value)
                            {
                                int currentQuantity = Convert.ToInt32(result);

                                if (currentQuantity > 1)
                                {
                                    // ✅ Corrected: Reduce quantity only if greater than 1
                                    string updateQuery = @"UPDATE cart SET Quantity = Quantity - 1 
                                       WHERE ProductId = @ProductId AND CustomerId = @CustomerId AND status = 'active'";
                                    ExecuteQuery(conn, updateQuery, customerId, productId);
                                }
                                else
                                {
                                    // ✅ Instead of doing nothing, ensure the quantity stays at 1
                                    string resetQuery = @"UPDATE cart SET Quantity = 1 
                                      WHERE ProductId = @ProductId AND CustomerId = @CustomerId AND status = 'active'";
                                    ExecuteQuery(conn, resetQuery, customerId, productId);
                                }
                            }
                        }
                    }
                    else if (e.CommandName == "RemoveProduct")
                    {
                        string query = "DELETE FROM cart WHERE CustomerId = @CustomerId AND ProductId = @ProductId AND status = 'active'";
                        ExecuteQuery(conn, query, customerId, productId);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('An error occurred while processing your request.');", true);
                }
            }

            // Refresh the cart and update the master page cart count
            gvCart.DataBind();
            LoadCart();
            UpdateCartItemCountInMaster();
            UpdatePanelCart.Update();
        }

        private void LoadCart()
        {
            string customerId = Session["CustomerId"].ToString();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = @"SELECT c.ProductId, c.Quantity, c.status, 
                                    p.ImageUrl, p.ProductName, p.Price, 
                                    (p.Price * c.Quantity) AS Total
                             FROM cart c 
                             LEFT JOIN bakeryproducts p ON c.ProductId = p.ProductId 
                             WHERE c.CustomerId = @CustomerId AND c.status = 'active'";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                int totalItems = 0;
                decimal totalAmount = 0;

                foreach (DataRow row in dt.Rows)
                {
                    bool productExists = !row.IsNull("ProductName");

                    if (productExists)
                    {
                        totalItems += Convert.ToInt32(row["Quantity"]);
                        totalAmount += Convert.ToDecimal(row["Total"]);
                    }
                }

                litTotalItems.Text = totalItems.ToString();
                litTotalAmount.Text = totalAmount.ToString("C");

                if (dt.Rows.Count > 0)
                {
                    gvCart.DataSource = dt;
                    gvCart.DataBind();
                    pnlEmptyCart.Visible = false;
                    gvCart.Visible = true;
                    btnProceedToBuy.Visible = (totalItems > 0);
                    cartSummary.Visible = true;
                }
                else
                {
                    pnlEmptyCart.Visible = true;
                    gvCart.Visible = false;
                    btnProceedToBuy.Visible = false;
                    cartSummary.Visible = false;
                }

                UpdateCartItemCountInMaster();
            }
        }

        protected void RemoveFromCart(object sender, CommandEventArgs e)
        {
            string productId = e.CommandArgument.ToString();
            string customerId = Session["CustomerId"].ToString();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "DELETE FROM cart WHERE ProductId = @ProductId AND CustomerId = @CustomerId";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadCart();
        }


        protected void CheckCartStock()
        {
            string customerId = Session["CustomerId"]?.ToString();
            if (string.IsNullOrEmpty(customerId)) return;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"SELECT c.ProductId, c.Quantity, p.StockQuantity 
                             FROM cart c 
                             JOIN bakeryproducts p ON c.ProductId = p.ProductId 
                             WHERE c.CustomerId = @CustomerId AND c.status = 'active'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                List<string> toRemove = new List<string>();
                Dictionary<string, int> toUpdate = new Dictionary<string, int>();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string productId = reader["ProductId"].ToString();
                        int cartQuantity = Convert.ToInt32(reader["Quantity"]);
                        int stockQuantity = Convert.ToInt32(reader["StockQuantity"]);

                        if (stockQuantity == 0)
                        {
                            toRemove.Add(productId);
                        }
                        else if (cartQuantity > stockQuantity)
                        {
                            toUpdate[productId] = stockQuantity;
                        }
                    }
                }

                // ✅ Bulk Remove
                if (toRemove.Count > 0)
                {
                    string removeQuery = $"DELETE FROM cart WHERE ProductId IN ({string.Join(",", toRemove.Select(id => $"'{id}'"))}) AND CustomerId = @CustomerId AND status = 'active'";
                    MySqlCommand removeCmd = new MySqlCommand(removeQuery, conn);
                    removeCmd.Parameters.AddWithValue("@CustomerId", customerId);
                    removeCmd.ExecuteNonQuery();
                }

                // ✅ Bulk Update
                if (toUpdate.Count > 0)
                {
                    string updateQuery = "UPDATE cart SET Quantity = CASE ";
                    foreach (var item in toUpdate)
                    {
                        updateQuery += $"WHEN ProductId = '{item.Key}' THEN {item.Value} ";
                    }
                    updateQuery += $"END WHERE ProductId IN ({string.Join(",", toUpdate.Keys.Select(id => $"'{id}'"))}) AND CustomerId = @CustomerId AND status = 'active'";

                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@CustomerId", customerId);
                    updateCmd.ExecuteNonQuery();
                }
            }
        }
            private void ExecuteQuery(MySqlConnection conn, string query, string customerId, string productId)
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateCartItemCountInMaster()
        {
            var master = Master as Site1;
            master?.UpdateCartItemCount();
        }

        protected void btnProceedToBuy_Click(object sender, EventArgs e)
        {
            Response.Redirect("Checkout.aspx");
        }

        protected void btnContinueShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx"); // Redirect to menu or product browsing page
        }
    }
}
