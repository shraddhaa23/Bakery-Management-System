using System;
using System.Configuration;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace BakeryShop
{
    public partial class Products : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategories();
                BindProducts();
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

                // Insert default item at the top
                DataRow row = dt.NewRow();
                row["CategoryId"] = 0;  // Dummy value
                row["CategoryName"] = "Select";
                dt.Rows.InsertAt(row, 0);

                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryId";
                ddlCategory.DataBind();
            }
        }


        private void BindProducts()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT p.ProductId, p.ProductName, c.CategoryName,p.Description, p.Price, p.ImageUrl, p.StockQuantity " +
                               "FROM bakeryproducts p JOIN bakerycategories c ON p.CategoryId = c.CategoryId";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text.Trim();
            string categoryId = ddlCategory.SelectedValue;
            string productDescription = txtProductDescription.Text.Trim();
            string priceStr = txtPrice.Text.Trim();  // Store as string
            string stockStr = txtStock.Text.Trim();  // Store as string


            // Validate Inputs
            string validationError = ValidateProductInput(productName, categoryId, productDescription, priceStr, stockStr);
            if (validationError != null)
            {
                lblMessage.Text = validationError;
                return;
            }

            string imageUrl = UploadImage();  // Upload the image and get its URL

            decimal price = Convert.ToDecimal(priceStr);
            int stock = Convert.ToInt32(stockStr);

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = "INSERT INTO bakeryproducts (ProductName, CategoryId, Description, Price, ImageUrl, StockQuantity) " +
                               "VALUES (@ProductName, @CategoryId, @Description, @Price, @ImageUrl, @StockQuantity)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductName", productName);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@Description", productDescription);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                cmd.Parameters.AddWithValue("@StockQuantity", stock);

                cmd.ExecuteNonQuery();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Product details are added successfully');", true);
            BindProducts();  // Refresh product list
            ClearFields();   // Clear input fields
        }

        private string ValidateProductInput(string productName, string categoryId, string productDescription, string priceStr, string stockStr)
        {
            if (string.IsNullOrEmpty(productName))
                return "Please enter a product name.";

            if (!Regex.IsMatch(productName, @"^[a-zA-Z\s]+$"))
                return "Product Name can only contain letters & spaces.";

            if (categoryId == "0")
                return "Please select a valid category.";

            if (string.IsNullOrEmpty(productDescription))
                return "Please enter a product description.";

            if (!Regex.IsMatch(productDescription, @"^[a-zA-Z0-9\s]+$"))
                return "Description can only contain letters, numbers & spaces.";

            if (string.IsNullOrEmpty(priceStr))
                return "Please enter a price.";

            if (!Regex.IsMatch(priceStr, @"^\d+(\.\d{1,2})?$"))
                return "Price can only contain whole & decimal numbers (e.g., 10 or 10.99).";

            if (string.IsNullOrEmpty(stockStr))
                return "Please enter stock quantity.";

            if (!Regex.IsMatch(stockStr, @"^\d+$"))
                return "Stock Quantity can only contain whole numbers.";
            
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM bakeryproducts WHERE ProductName = @ProductName";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductName", productName);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    return "Product Name already exists!";
                }
            }
            return null;  // No validation errors
        }

        private string UploadImage()
        {
            if (fuProductImage.HasFile)
            {
                string folderPath = Server.MapPath("~/ProductImages/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = Path.GetFileName(fuProductImage.FileName);
                string filePath = Path.Combine(folderPath, fileName);
                fuProductImage.SaveAs(filePath);

                return "~/ProductImages/" + fileName;
            }
            return "~/ProductImages/default.png"; // Default image if no file uploaded
        }

        protected void cvFileExtension_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (fuProductImage.HasFile)
            {
                string fileExtension = Path.GetExtension(fuProductImage.FileName).ToLower();
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

                args.IsValid = allowedExtensions.Contains(fileExtension);
            }
            else
            {
                args.IsValid = true; // If no file is uploaded, consider it valid
            }
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditProduct")  // Updated CommandName for users
            {
                string productId = e.CommandArgument.ToString();
                Response.Redirect($"EditProducts.aspx?ProductId={productId}"); // Redirects to EditUser page
            }
            else if (e.CommandName == "DeleteProduct")  // Updated CommandName for users
            {
                string productId = e.CommandArgument.ToString();
                DeleteProduct(productId); // Calls DeleteUser method
            }
        }

        private void DeleteProduct(string productId)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string checkOrderQuery = "SELECT COUNT(*) FROM orderitems WHERE ProductId = @ProductId";
                MySqlCommand checkCmd = new MySqlCommand(checkOrderQuery, conn);
                checkCmd.Parameters.AddWithValue("@ProductId", productId);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Cannot delete product. It exists in previous orders.');", true);
                }
                else
                {
                    // Delete product from cart first
                    string deleteFromCartQuery = "DELETE FROM cart WHERE ProductId = @ProductId";
                    MySqlCommand cmd1 = new MySqlCommand(deleteFromCartQuery, conn);
                    cmd1.Parameters.AddWithValue("@ProductId", productId);
                    cmd1.ExecuteNonQuery();

                    // Now delete the product
                    string deleteProductQuery = "DELETE FROM bakeryproducts WHERE ProductId = @ProductId";
                    MySqlCommand cmd2 = new MySqlCommand(deleteProductQuery, conn);
                    cmd2.Parameters.AddWithValue("@ProductId", productId);
                    cmd2.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Product details are deleted');", true);
                }
            }
            BindProducts();  // Refresh product list
        }


        protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Update the current page index of the GridView
            gvProducts.PageIndex = e.NewPageIndex;

            // Reload the product data
            BindProducts();
        }

        private void ClearFields()
        {
            txtProductName.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtStock.Text = string.Empty;
            ddlCategory.SelectedIndex = 0;
        }
    }
}
