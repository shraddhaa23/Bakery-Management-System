using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace BakeryShop
{
    public partial class EditProducts : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        string productId;

        protected void Page_Load(object sender, EventArgs e)
        {
            productId = Request.QueryString["ProductId"];
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(productId))
                {
                    Response.Redirect("Products.aspx");
                }
                BindCategories();
                LoadProductDetails();
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
                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryId";
                ddlCategory.DataBind();
            }
        }

        private void LoadProductDetails()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT * FROM bakeryproducts WHERE ProductId = @ProductId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtProductName.Text = reader["ProductName"].ToString();
                        ddlCategory.SelectedValue = reader["CategoryId"].ToString();
                        txtProductDescription.Text = reader["Description"].ToString();
                        txtPrice.Text = reader["Price"].ToString();
                        txtStock.Text = reader["StockQuantity"].ToString();
                        imgProduct.ImageUrl = reader["ImageUrl"].ToString();
                    }
                    else
                    {
                        Response.Redirect("Products.aspx");
                    }
                }
            }
        }

        protected void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text.Trim();
            string description = txtProductDescription.Text.Trim();
            string categoryId = ddlCategory.SelectedValue;
            string priceStr = txtPrice.Text.Trim();  // Store as string
            string stockStr = txtStock.Text.Trim();  // Store as string

            // Validate Inputs
            string validationError = ValidateProductInput(productName, categoryId, description, priceStr, stockStr);
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
                string query = "UPDATE bakeryproducts SET ProductName=@ProductName, CategoryId=@CategoryId, " +
                               "Description=@Description, Price=@Price, StockQuantity=@StockQuantity, ImageUrl=@ImageUrl WHERE ProductId=@ProductId";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductName", productName);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@StockQuantity", stock);
                cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Product details updated successfully!');", true);
            Response.Redirect("Products.aspx");
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

            // If no new image is uploaded, get the existing image from the database
            return GetExistingImage();
        }

        private string GetExistingImage()
        {
            string existingImage = "";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT ImageUrl FROM bakeryproducts WHERE ProductId = @ProductId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    existingImage = result.ToString();
                }
            }

            return string.IsNullOrEmpty(existingImage) ? "~/ProductImages/default.png" : existingImage;
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
    }
}
