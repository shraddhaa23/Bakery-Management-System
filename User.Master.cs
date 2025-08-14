using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BakeryShop
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadCategories();
            if (!IsPostBack && Session["CustomerId"] != null)
            {
                UpdateCartItemCount();
            }

            if (Session["CustomerId"] == null)
            {
                AccountMenu.Controls.Add(new LiteralControl(
                    "<a href='/Login.aspx'>Login</a><br>" +
                    "<a href='/Reg.aspx'>Register</a>"
                ));
            }
            else
            {
                AccountMenu.Controls.Add(new LiteralControl("<a href='/MyOrders.aspx'>My Orders</a><br>"));
                Button logoutButton = new Button();
                logoutButton.Text = "Logout";
                logoutButton.Click += new EventHandler(LogoutButton_Click);
                logoutButton.CssClass = "logout-button";
                AccountMenu.Controls.Add(logoutButton);
            }
        }

        public void UpdateCartItemCount()
        {
            if (Session["CustomerId"] != null)
            {

                string customerId = Session["CustomerId"].ToString();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    string query = "SELECT SUM(Quantity) FROM cart WHERE CustomerId = @CustomerId AND status = 'active'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);

                    conn.Open();
                    object countObj = cmd.ExecuteScalar();
                    int itemCount = countObj != DBNull.Value ? Convert.ToInt32(countObj) : 0;

                    lblCartItemCount.Text = itemCount.ToString();
                    lblCartItemCount.Visible = itemCount > 0; // Show only if there's at least one item
                }
            }
            else
            {
                lblCartItemCount.Text = "0";
                lblCartItemCount.Visible = false;
            }
        }

        private void LoadCategories()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT CategorySlug, CategoryName FROM bakerycategories";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptCategories.DataSource = dt;
                rptCategories.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                Response.Redirect($"Menu.aspx?search={Server.UrlEncode(searchQuery)}");
            }
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}
