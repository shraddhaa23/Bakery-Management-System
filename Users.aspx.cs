using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace BakeryShop
{
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT CustomerId, FirstName, LastName, Email, PhoneNumber FROM Customer";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvUsers.DataSource = dt;
                    gvUsers.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading users: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditUser")
            {
                string customerId = e.CommandArgument.ToString();
                Response.Redirect($"EditUser.aspx?CustomerId={customerId}");
            }
            else if (e.CommandName == "DeleteUser")
            {
                string customerId = e.CommandArgument.ToString();
                DeleteUser(customerId);
            }
        }

        private void DeleteUser(string customerId)
        {
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
            {
                conn.Open();

                // Delete order items first
                string deleteOrderItemsQuery = "DELETE FROM orderitems WHERE OrderId IN (SELECT OrderId FROM orders WHERE CustomerId = @CustomerId)";
                MySqlCommand deleteOrderItemsCmd = new MySqlCommand(deleteOrderItemsQuery, conn);
                deleteOrderItemsCmd.Parameters.AddWithValue("@CustomerId", customerId);
                deleteOrderItemsCmd.ExecuteNonQuery();

                // Delete invoices (use OrderId reference)
                string deleteInvoiceQuery = "DELETE FROM invoice WHERE OrderId IN (SELECT OrderId FROM orders WHERE CustomerId = @CustomerId)";
                MySqlCommand deleteInvoiceCmd = new MySqlCommand(deleteInvoiceQuery, conn);
                deleteInvoiceCmd.Parameters.AddWithValue("@CustomerId", customerId);
                deleteInvoiceCmd.ExecuteNonQuery();

                // Delete orders after invoices are removed
                string deleteOrdersQuery = "DELETE FROM orders WHERE CustomerId = @CustomerId";
                MySqlCommand deleteOrdersCmd = new MySqlCommand(deleteOrdersQuery, conn);
                deleteOrdersCmd.Parameters.AddWithValue("@CustomerId", customerId);
                deleteOrdersCmd.ExecuteNonQuery();

                // Delete cart items
                string deleteCartQuery = "DELETE FROM cart WHERE CustomerId = @CustomerId";
                MySqlCommand deleteCartCmd = new MySqlCommand(deleteCartQuery, conn);
                deleteCartCmd.Parameters.AddWithValue("@CustomerId", customerId);
                deleteCartCmd.ExecuteNonQuery();

                // Finally, delete the customer record
                string deleteCustomerQuery = "DELETE FROM customer WHERE CustomerId = @CustomerId";
                MySqlCommand deleteCustomerCmd = new MySqlCommand(deleteCustomerQuery, conn);
                deleteCustomerCmd.Parameters.AddWithValue("@CustomerId", customerId);
                deleteCustomerCmd.ExecuteNonQuery();

                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('User deleted successfully');", true);
            }

            LoadUsers();  // Refresh the GridView
        }



        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            LoadUsers(); // Reload the user data
        }
    }
}
