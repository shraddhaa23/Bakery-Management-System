using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace BakeryShop
{
    public partial class Orders : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // Load orders only on the initial page load (not on postbacks)
            if (!IsPostBack)
            {
                //gvOrders.EditIndex = -1;
                LoadOrders();
            }
        }

        // This method is used to load orders into the GridView
        private void LoadOrders()
        {
            //gvOrders.EditIndex = -1; // Reset edit mode before binding data
            // Get the connection string from web.config
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            // Using the MySQL connection to retrieve the orders from the database
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // SQL query to fetch orders along with customer information
                string query = @"SELECT o.OrderId, c.FirstName, c.LastName, o.OrderDate, o.OrderStatus 
                         FROM Orders o 
                         JOIN Customer c ON o.CustomerId = c.CustomerId 
                         ORDER BY o.OrderDate DESC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind the data to the GridView
                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }
        protected void gvOrders_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvOrders.EditIndex = -1; // Reset edit mode
            LoadOrders(); // Refresh the GridView
        }


        // This method is triggered when a command button in the GridView is clicked (e.g., Process, Deliver, Cancel)
        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Get the OrderId passed as the CommandArgument
            string orderId = e.CommandArgument.ToString();

            // Get the connection string from web.config
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            // Open connection to the database
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string updateQuery = "";

                // Fetch the current order status before updating
                string selectQuery = "SELECT OrderStatus FROM Orders WHERE OrderId = @OrderId";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                selectCmd.Parameters.AddWithValue("@OrderId", orderId);
                string currentStatus = selectCmd.ExecuteScalar()?.ToString();

                // Determine which action the user wants to take based on CommandName
                switch (e.CommandName)
                {
                    case "Process":
                        if (currentStatus == "Cancelled" || currentStatus == "Delivered")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Cannot mark as Processing. Order is either Cancelled or Delivered.');", true);
                        }
                        updateQuery = "UPDATE Orders SET OrderStatus = 'Processing' WHERE OrderId = @OrderId";
                        break;

                    case "Deliver":
                        if (currentStatus == "Cancelled")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Cannot mark as Delivered. Order is Cancelled.');", true);
                        }
                        updateQuery = "UPDATE Orders SET OrderStatus = 'Delivered' WHERE OrderId = @OrderId";
                        break;

                    case "Cancel":
                        if (currentStatus == "Processing")
                        {
                           ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Cannot Cancel an order that is Processing.');", true);
                        }
                        updateQuery = "UPDATE Orders SET OrderStatus = 'Cancelled' WHERE OrderId = @OrderId";
                        break;
                }

                // If a valid action was found, execute the update query
                if (!string.IsNullOrEmpty(updateQuery))
                {
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    // Execute the query and get the result (rows affected)
                    int result = cmd.ExecuteNonQuery();

                    // Display a success or failure message
                    if (result > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Order status updated successfully!');", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to update order status.');", true);
                    }
                }
            }

            // Refresh the GridView to show updated order statuses
            LoadOrders();
        }

        protected void gvOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrders.PageIndex = e.NewPageIndex;
            LoadOrders();
        }
    }
}
