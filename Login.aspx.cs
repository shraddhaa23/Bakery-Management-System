using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace BakeryShop
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TxtEmail.Text = string.Empty;
                TxtPassword.Text = string.Empty;
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            
            string email = TxtEmail.Text;
            string password = TxtPassword.Text;
            string userType = DdlLoginType.SelectedValue;

            
            string validationMessage = ValidateInput(email, password);
            if (!string.IsNullOrEmpty(validationMessage))
            {
                LblError.ForeColor = System.Drawing.Color.Red;
                LblError.Text = validationMessage;
                return;
            }

            
            if (userType == "Admin")
            {
                string adminId = ValidateAdminLogin(email, password);
                if (!string.IsNullOrEmpty(adminId))
                {
                    Session["AdminId"] = adminId;
                    Response.Redirect("Dashboard.aspx");
                }
                else
                {
                    LblError.ForeColor = System.Drawing.Color.Red;
                    LblError.Text = "Invalid admin email or password.\nPlease Try Again!";
                }
            }
            else 
            {
                string customerId = ValidateCustomerLogin(email, password);
                if (!string.IsNullOrEmpty(customerId))
                {
                    Session["CustomerId"] = customerId;
                    Response.Redirect("Home.aspx");
                }
                else
                {
                    LblError.ForeColor = System.Drawing.Color.Red;
                    LblError.Text = "Invalid customer email or password.\nPlease Try Again!";
                }
            }
        }

        private string ValidateInput(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                return "Please enter your email.";

            if (!Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                return "Please enter a valid email address.";

            if (string.IsNullOrEmpty(password))
                return "Please enter your password.";

            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10}$"))
                return "Password must be 10 characters with A-Z, a-z, 0-9 & special (@$!%*?&).";

            return null; 
        }

        private string ValidateAdminLogin(string email, string password)
        {
            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT AdminId, Password FROM Admin WHERE Email = @Email";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Email exists
                            {
                                string storedPassword = reader["Password"].ToString();
                                if (password == storedPassword) 
                                {
                                    return reader["AdminId"].ToString(); 
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    LblError.ForeColor = System.Drawing.Color.Red;
                    LblError.Text = "An error occurred while processing the admin login." + ex.Message;
                }
            }

            return null; 
        }

        private string ValidateCustomerLogin(string email, string password)
        {
            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CustomerId, Password FROM Customer WHERE Email = @Email";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["Password"].ToString();
                                string customerId = reader["CustomerId"].ToString();

                                if (VerifyPassword(password, storedPassword))
                                {
                                    return customerId;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LblError.ForeColor = System.Drawing.Color.Red;
                    LblError.Text = "An error occurred while processing the customer login: " + ex.Message;
                }
            }
            return null;
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return HashPassword(inputPassword) == storedPassword;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        protected void LnkSignUp_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reg.aspx");
        }
    }
}
