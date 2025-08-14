using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace BakeryShop
{
    public partial class Categories : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategories();
            }
        }

        private void BindCategories()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT CategoryId, CategoryName FROM bakerycategories";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvCategories.DataSource = dt;
                gvCategories.DataBind();
            }
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            string categoryName = txtCategoryName.Text.Trim();

            string validationError = ValidateCategoryInput(categoryName);
            if (validationError != null)
            {
                lblMessage.Text = validationError;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string categorySlug = GenerateSlug(categoryName);

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = "INSERT INTO bakerycategories (CategoryName, CategorySlug) VALUES (@CategoryName, @CategorySlug)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                cmd.Parameters.AddWithValue("@CategorySlug", categorySlug);

                cmd.ExecuteNonQuery();
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Category added successfully!');", true);
            BindCategories();
        }

        private string ValidateCategoryInput(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return "Category Name is required!";
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(categoryName, @"^[a-zA-Z\s]+$"))
            {
                return "Category Name can only contain letters and spaces!";
            }

            // Check for duplicate category (optional)
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM bakerycategories WHERE CategoryName = @CategoryName";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryName", categoryName);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    return "Category Name already exists!";
                }
            }

            return null; // Validation passed
        }

        protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditCategory")
            {
                string categoryId = e.CommandArgument.ToString();
                Response.Redirect($"EditCategory.aspx?CategoryId={categoryId}");
            }
            else if (e.CommandName == "DeleteCategory")
            {
                string categoryId = e.CommandArgument.ToString();
                DeleteCategory(categoryId);
            }
        }

        private void DeleteCategory(string categoryId)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                try
                {
                    // Step 1: Check if any product in this category exists in orderitems
                    string checkOrderQuery = @"SELECT COUNT(*) 
                                       FROM orderitems 
                                       WHERE ProductId IN (SELECT ProductId FROM bakeryproducts WHERE CategoryId = @CategoryId)";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkOrderQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@CategoryId", categoryId);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            // Restrict deletion if products from this category are in orders
                            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage",
                                "alert('Cannot delete category. Some products in this category have been ordered.');", true);
                            return; // Exit function, preventing deletion
                        }
                    }

                    // Step 2: Delete cart entries first
                    string deleteCartQuery = "DELETE FROM cart WHERE ProductId IN (SELECT ProductId FROM bakeryproducts WHERE CategoryId = @CategoryId)";
                    using (MySqlCommand cmdDeleteCart = new MySqlCommand(deleteCartQuery, conn))
                    {
                        cmdDeleteCart.Parameters.AddWithValue("@CategoryId", categoryId);
                        cmdDeleteCart.ExecuteNonQuery();
                    }

                    // Step 3: Delete products from bakeryproducts (only if no orders exist)
                    string deleteProductsQuery = "DELETE FROM bakeryproducts WHERE CategoryId = @CategoryId";
                    using (MySqlCommand cmdDeleteProducts = new MySqlCommand(deleteProductsQuery, conn))
                    {
                        cmdDeleteProducts.Parameters.AddWithValue("@CategoryId", categoryId);
                        cmdDeleteProducts.ExecuteNonQuery();
                    }

                    // Step 4: Delete the category
                    string deleteCategoryQuery = "DELETE FROM bakerycategories WHERE CategoryId = @CategoryId";
                    using (MySqlCommand cmdDeleteCategory = new MySqlCommand(deleteCategoryQuery, conn))
                    {
                        cmdDeleteCategory.Parameters.AddWithValue("@CategoryId", categoryId);
                        cmdDeleteCategory.ExecuteNonQuery();
                    }

                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Category deleted successfully!');", true);
                }
                catch (Exception ex)
                {
                    ShowMessage("An error occurred while deleting the category: " + ex.Message);
                }
            }

            BindCategories();  // Refresh category list
        }


        private void ShowMessage(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Alert", $"alert('{message}');", true);
        }

        protected void gvCategories_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Update the current page index of the GridView
            gvCategories.PageIndex = e.NewPageIndex;

            // Reload the product data
            BindCategories();
        }
        private string GenerateSlug(string input)
        {
            string slug = input.ToLower().Trim();
            slug = Regex.Replace(slug, @"\s+", "-");        // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", ""); // Remove invalid characters
            return slug;
        }
    }
}
