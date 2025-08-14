using System;
using System.Configuration;
using System.Web.UI;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace BakeryShop
{
    public partial class EditCategory : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string categoryId = Request.QueryString["CategoryId"];
                if (!string.IsNullOrEmpty(categoryId))
                {
                    LoadCategoryDetails(categoryId);
                }
                else
                {
                    Response.Redirect("Categories.aspx");
                }
            }
        }

        private void LoadCategoryDetails(string categoryId)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT CategoryId, CategoryName, CategorySlug FROM bakerycategories WHERE CategoryId = @CategoryId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblCategoryId.Text = reader["CategoryId"].ToString();
                        txtCategoryName.Text = reader["CategoryName"].ToString();
                        txtCategorySlug.Text = reader["CategorySlug"].ToString();
                    }
                    else
                    {
                        Response.Redirect("Categories.aspx");
                    }
                }
            }
        }

        protected void txtCategoryName_TextChanged(object sender, EventArgs e)
        {
            txtCategorySlug.Text = GenerateSlug(txtCategoryName.Text);
        }

        private string GenerateSlug(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return "";

            // Convert to lowercase, remove spaces, and special characters
            string slug = categoryName.Trim().ToLower();
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-");

            return slug;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string categoryId = lblCategoryId.Text;
            string categoryName = txtCategoryName.Text;
            string categorySlug = txtCategorySlug.Text;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "UPDATE bakerycategories SET CategoryName = @CategoryName, CategorySlug = @CategorySlug WHERE CategoryId = @CategoryId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                cmd.Parameters.AddWithValue("@CategorySlug", categorySlug);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Category details updated successfully!');", true);
            Response.Redirect("Categories.aspx");
        }
    }
}
