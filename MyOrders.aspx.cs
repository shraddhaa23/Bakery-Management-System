using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace BakeryShop
{
    public partial class MyOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        private void LoadOrders()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string ordersQuery = @"SELECT o.OrderId, o.OrderDate, o.OrderStatus, SUM(oi.Quantity * oi.Price) AS OrderTotal
                               FROM Orders o
                               LEFT JOIN OrderItems oi ON o.OrderId = oi.OrderId
                               WHERE o.CustomerId = @CustomerId
                               GROUP BY o.OrderId, o.OrderDate, o.OrderStatus
                               ORDER BY o.OrderDate DESC";

                MySqlCommand ordersCmd = new MySqlCommand(ordersQuery, conn);
                ordersCmd.Parameters.AddWithValue("@CustomerId", Session["CustomerId"]);
                MySqlDataAdapter ordersAdapter = new MySqlDataAdapter(ordersCmd);
                DataTable ordersTable = new DataTable();
                ordersAdapter.Fill(ordersTable);

                List<Order> ordersList = new List<Order>();

                foreach (DataRow order in ordersTable.Rows)
                {
                    string orderId = order["OrderId"].ToString();
                    DateTime orderDate = Convert.ToDateTime(order["OrderDate"]);
                    string orderStatus = order["OrderStatus"].ToString();
                    decimal orderTotal = order["OrderTotal"] != DBNull.Value ? Convert.ToDecimal(order["OrderTotal"]) : 0;

                    string itemsQuery = @"SELECT p.ProductName, oi.Quantity, oi.Price, 
                                         (oi.Quantity * oi.Price) AS Total 
                                  FROM OrderItems oi 
                                  JOIN bakeryproducts p ON oi.ProductId = p.ProductId 
                                  WHERE oi.OrderId = @OrderId";

                    MySqlCommand itemsCmd = new MySqlCommand(itemsQuery, conn);
                    itemsCmd.Parameters.AddWithValue("@OrderId", orderId);
                    MySqlDataAdapter itemsAdapter = new MySqlDataAdapter(itemsCmd);
                    DataTable itemsTable = new DataTable();
                    itemsAdapter.Fill(itemsTable);

                    Order orderObj = new Order
                    {
                        OrderId = orderId,
                        OrderDate = orderDate,
                        OrderStatus = orderStatus,
                        OrderTotal = orderTotal,
                        Products = itemsTable
                    };

                    ordersList.Add(orderObj);
                }

                if (ordersList.Count > 0)
                {
                    rptOrders.DataSource = ordersList;
                    rptOrders.DataBind();
                    lblNoOrders.Visible = false; // Hide the message
                }
                else
                {
                    rptOrders.DataSource = null;
                    rptOrders.DataBind();
                    lblNoOrders.Visible = true; // Show the message
                }
            }
        }


        protected void CancelOrder_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Session["CancelOrderId"] = btn.CommandArgument;
            pnlConfirm.Visible = true;
        }

        protected void ConfirmCancel_Click(object sender, EventArgs e)
        {
            string orderId = Session["CancelOrderId"].ToString();
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // ✅ Check current order status
                    string checkStatusQuery = "SELECT OrderStatus FROM Orders WHERE OrderId = @OrderId";
                    using (MySqlCommand checkStatusCmd = new MySqlCommand(checkStatusQuery, conn, transaction))
                    {
                        checkStatusCmd.Parameters.AddWithValue("@OrderId", orderId);
                        string status = checkStatusCmd.ExecuteScalar()?.ToString();

                        if (status == "Pending")
                        {
                            Dictionary<string, int> orderItems = new Dictionary<string, int>();

                            // ✅ Retrieve ordered items to restore stock
                            string getOrderQuery = "SELECT ProductId, Quantity FROM OrderItems WHERE OrderId = @OrderId";
                            using (MySqlCommand getOrderCmd = new MySqlCommand(getOrderQuery, conn, transaction))
                            {
                                getOrderCmd.Parameters.AddWithValue("@OrderId", orderId);
                                using (MySqlDataReader reader = getOrderCmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string productId = reader["ProductId"].ToString();
                                        int quantity = Convert.ToInt32(reader["Quantity"]);
                                        orderItems[productId] = quantity;
                                    }
                                } // ✅ Reader automatically closed here
                            }

                            // ✅ Restore stock quantity for each product
                            foreach (var item in orderItems)
                            {
                                string restoreStockQuery = "UPDATE bakeryproducts SET StockQuantity = StockQuantity + @Quantity WHERE ProductId = @ProductId";
                                using (MySqlCommand restoreStockCmd = new MySqlCommand(restoreStockQuery, conn, transaction))
                                {
                                    restoreStockCmd.Parameters.AddWithValue("@Quantity", item.Value);
                                    restoreStockCmd.Parameters.AddWithValue("@ProductId", item.Key);
                                    restoreStockCmd.ExecuteNonQuery();
                                }
                            }

                            // ✅ Mark the order as 'Cancelled'
                            string cancelQuery = "UPDATE Orders SET OrderStatus = 'Cancelled' WHERE OrderId = @OrderId";
                            using (MySqlCommand cancelCmd = new MySqlCommand(cancelQuery, conn, transaction))
                            {
                                cancelCmd.Parameters.AddWithValue("@OrderId", orderId);
                                cancelCmd.ExecuteNonQuery();
                            }

                            transaction.Commit(); // ✅ Commit transaction

                            pnlConfirm.Visible = false;
                            Response.Write("<script>alert('Your order has been cancelled, and stock has been restored.');</script>");
                        }
                        else
                        {
                            pnlConfirm.Visible = false;
                            Response.Write("<script>alert('Your order cannot be cancelled as it is already processed.');</script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Response.Write("<script>alert('An error occurred: " + ex.Message + "');</script>");
                }
            }

            LoadOrders(); // ✅ Refresh order list
        }

        protected void ClosePopup_Click(object sender, EventArgs e)
        {
            pnlConfirm.Visible = false;
        }

        public class Order
        {
            public string OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public string OrderStatus { get; set; }
            public decimal OrderTotal { get; set; }
            public DataTable Products { get; set; }
        }
    }
}
