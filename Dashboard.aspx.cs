using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace BakeryShop
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMetrics();
                LoadRecentOrders();
            }
        }

        private void LoadMetrics()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // Total Orders
                    MySqlCommand cmdTotalOrders = new MySqlCommand("SELECT COUNT(*) FROM Orders", con);
                    lblTotalOrders.Text = cmdTotalOrders.ExecuteScalar().ToString();

                    // Total Sales
                    MySqlCommand cmdTotalSales = new MySqlCommand("SELECT SUM(TotalAmount) FROM Orders WHERE OrderStatus = 'Delivered'", con);
                    object totalSales = cmdTotalSales.ExecuteScalar();
                    lblTotalSales.Text = totalSales != DBNull.Value ? totalSales.ToString() : "0";

                    // Total Customers
                    MySqlCommand cmdTotalCustomers = new MySqlCommand("SELECT COUNT(*) FROM Customer", con);
                    lblTotalCustomers.Text = cmdTotalCustomers.ExecuteScalar().ToString();

                    // Pending Orders
                    MySqlCommand cmdPendingOrders = new MySqlCommand("SELECT COUNT(*) FROM Orders WHERE OrderStatus = 'Pending'", con);
                    lblPendingOrders.Text = cmdPendingOrders.ExecuteScalar().ToString();

                    // Delivered Orders
                    MySqlCommand cmdDeliveredOrders = new MySqlCommand("SELECT COUNT(*) FROM Orders WHERE OrderStatus = 'Delivered'", con);
                    lblDeliveredOrders.Text = cmdDeliveredOrders.ExecuteScalar().ToString();

                    // Cancelled Orders
                    MySqlCommand cmdCancelledOrders = new MySqlCommand("SELECT COUNT(*) FROM Orders WHERE OrderStatus = 'Cancelled'", con);
                    lblCancelledOrders.Text = cmdCancelledOrders.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., log the error)
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        private void LoadRecentOrders()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    string query = @"SELECT OrderId, CustomerId, OrderDate, OrderStatus FROM Orders
                                     ORDER BY OrderDate DESC
                                     LIMIT 5";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvRecentOrders.DataSource = dt;
                    gvRecentOrders.DataBind();
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., log the error)
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
