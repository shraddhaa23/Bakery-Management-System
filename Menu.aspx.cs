using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace BakeryShop
{
    public partial class Menu : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["search"]))
                {
                    string searchQuery = Request.QueryString["search"];
                    SearchProducts(searchQuery);
                }
                else
                {
                    string categorySlug = Request.QueryString["CategorySlug"];
                    if (!string.IsNullOrEmpty(categorySlug))
                    {
                        LoadCategoryAndProducts(categorySlug);
                    }
                    else
                    {
                        LoadAllCategoriesAndProducts();
                    }
                }
            }
        }

        protected string GetAddToCartVisibility(object stockQuantity)
        {
            int stock = stockQuantity != DBNull.Value ? Convert.ToInt32(stockQuantity) : 0;
            return stock > 0 ? "visible" : "hidden";
        }

        private void LoadCategoryAndProducts(string categorySlug)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string categoryQuery = "SELECT CategoryName FROM bakerycategories WHERE CategorySlug = @CategorySlug";
                MySqlCommand cmd = new MySqlCommand(categoryQuery, conn);
                cmd.Parameters.AddWithValue("@CategorySlug", categorySlug);

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    lblCategoryName.Text = "Category: " + result.ToString();
                    LoadProducts(categorySlug);
                }
                else
                {
                    lblCategoryName.Text = "Category not found!";
                }
            }
        }

        private void LoadAllCategoriesAndProducts()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = @"SELECT c.CategoryName, c.CategorySlug, 
                                p.ProductId, p.ProductName, p.Description, p.Price, p.ImageUrl, p.StockQuantity
                         FROM bakerycategories c
                         LEFT JOIN bakeryproducts p ON c.CategoryId = p.CategoryId
                         ORDER BY c.CategoryName";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Assuming you have a Repeater or another control to display categories and their products
                    gvProducts.DataSource = dt;
                    gvProducts.DataBind();
                }
                else
                {
                    lblCategoryName.Text = "No categories or products found!";
                }
            }
        }

        private void LoadProducts(string categorySlug)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = @"SELECT p.ProductId, p.ProductName, p.Description, p.Price, p.ImageUrl, p.StockQuantity
                         FROM bakeryproducts p
                         JOIN bakerycategories c ON p.CategoryId = c.CategoryId
                         WHERE c.CategorySlug = @CategorySlug";

                // Add price filter to the query if values are provided
                if (!string.IsNullOrEmpty(txtPriceFrom.Text) && decimal.TryParse(txtPriceFrom.Text, out decimal priceFrom))
                {
                    query += " AND p.Price >= @PriceFrom";
                }

                if (!string.IsNullOrEmpty(txtPriceTo.Text) && decimal.TryParse(txtPriceTo.Text, out decimal priceTo))
                {
                    query += " AND p.Price <= @PriceTo";
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategorySlug", categorySlug);

                // Add price parameters if filters are provided
                if (!string.IsNullOrEmpty(txtPriceFrom.Text) && decimal.TryParse(txtPriceFrom.Text, out priceFrom))
                {
                    cmd.Parameters.AddWithValue("@PriceFrom", priceFrom);
                }

                if (!string.IsNullOrEmpty(txtPriceTo.Text) && decimal.TryParse(txtPriceTo.Text, out priceTo))
                {
                    cmd.Parameters.AddWithValue("@PriceTo", priceTo);
                }

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    lblMessage.Text = "Oops! No products found within your selected price range. Try adjusting your filter to discover more great items!";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    gvProducts.Visible = false;
                }
                else
                {
                    lblMessage.Visible = false;
                    gvProducts.Visible = true;
                    gvProducts.DataSource = dt;
                    gvProducts.DataBind();
                }
            }
        }

        private void SearchProducts(string searchQuery)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = @"SELECT ProductId, ProductName, Description, Price, ImageUrl, StockQuantity 
                         FROM bakeryproducts 
                         WHERE ProductName LIKE @SearchQuery";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvProducts.DataSource = dt;
                gvProducts.DataBind();

                if (dt.Rows.Count == 0)
                {
                    // Hide the GridView
                    gvProducts.Visible = false;
                    lblProducts.Text = "No products found for '" + searchQuery + "'";
                    lblProducts.Visible = true;
                    menu_price_filter.Visible = false;
                }
                else
                {
                    // Show GridView if products are found
                    gvProducts.Visible = true;
                    lblProducts.Text = "Search Results for: '" + searchQuery + "'";
                    lblProducts.Visible = true;
                    menu_price_filter.Visible = true;
                }
            }
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                string productId = e.CommandArgument.ToString();

                // ✅ Set default quantity (modify this based on user input)
                int quantity = 1; // You can get this from a TextBox if needed.

                AddToCart(productId, quantity);
            }
        }

        // ✅ Fixed AddToCart Method with Quantity Handling
        private void AddToCart(string productId, int quantity)
        {
            try
            {
                // ✅ Ensure the user is logged in
                if (Session["CustomerId"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                string customerId = Session["CustomerId"].ToString(); // ✅ Get Customer ID

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // ✅ Check if the product exists and has enough stock
                    string checkStockQuery = "SELECT StockQuantity FROM bakeryproducts WHERE ProductId = @ProductId";
                    MySqlCommand checkStockCmd = new MySqlCommand(checkStockQuery, conn);
                    checkStockCmd.Parameters.AddWithValue("@ProductId", productId);

                    object stockResult = checkStockCmd.ExecuteScalar();
                    if (stockResult == null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Error: Product not found.');", true);
                    }

                    int availableStock = Convert.ToInt32(stockResult);

                    if (quantity > availableStock)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Not enough stock available.');", true);
                    }

                    // ✅ Check if the product is already in the cart
                    string checkCartQuery = "SELECT Quantity FROM cart WHERE ProductId = @ProductId AND CustomerId = @CustomerId AND status = 'active'";
                    MySqlCommand checkCartCmd = new MySqlCommand(checkCartQuery, conn);
                    checkCartCmd.Parameters.AddWithValue("@ProductId", productId);
                    checkCartCmd.Parameters.AddWithValue("@CustomerId", customerId);

                    object existingQuantityObj = checkCartCmd.ExecuteScalar();
                    int existingQuantity = existingQuantityObj != null ? Convert.ToInt32(existingQuantityObj) : 0;

                    if (existingQuantity > 0)
                    {
                        // ✅ Update existing cart item quantity
                        int newQuantity = existingQuantity + quantity;
                        if (newQuantity > availableStock)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Not enough stock available.');", true);
                        }

                        string updateCartQuery = "UPDATE cart SET Quantity = @Quantity WHERE ProductId = @ProductId AND CustomerId = @CustomerId";
                        MySqlCommand updateCartCmd = new MySqlCommand(updateCartQuery, conn);
                        updateCartCmd.Parameters.AddWithValue("@Quantity", newQuantity);
                        updateCartCmd.Parameters.AddWithValue("@ProductId", productId);
                        updateCartCmd.Parameters.AddWithValue("@CustomerId", customerId);
                        updateCartCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // ✅ Insert new item into cart
                        string insertCartQuery = "INSERT INTO cart (CustomerId, ProductId, Quantity, status) VALUES (@CustomerId, @ProductId, @Quantity, 'active')";
                        MySqlCommand insertCartCmd = new MySqlCommand(insertCartQuery, conn);
                        insertCartCmd.Parameters.AddWithValue("@CustomerId", customerId);
                        insertCartCmd.Parameters.AddWithValue("@ProductId", productId);
                        insertCartCmd.Parameters.AddWithValue("@Quantity", quantity);
                        insertCartCmd.ExecuteNonQuery();
                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Product added to cart successfully!');", true);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error adding to cart: " + ex.Message;
                lblMessage.Visible = true;
            }

        // Update cart count in Master Page
        MasterPage masterPage = this.Master;
            if (masterPage != null)
            {
                ((Site1)masterPage).UpdateCartItemCount();
            }

            // Wait for 1 second and redirect to Cart.aspx
            //Thread.Sleep(1000);
            //Response.Redirect("Cart.aspx");
        }

        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
            string categorySlug = Request.QueryString["CategorySlug"];
            if (!string.IsNullOrEmpty(categorySlug))
            {
                LoadCategoryAndProducts(categorySlug);
            }
        }
    }
}

