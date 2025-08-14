using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace BakeryShop
{
    public partial class Reg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TxtFirstName.Text = string.Empty;
                TxtLastName.Text = string.Empty;
                TxtEmail.Text = string.Empty;
                TxtPassword.Text = string.Empty;
                TxtPhoneNumber.Text = string.Empty;
            }
        }
        protected void BtnSignUp_Click(object sender, EventArgs e)
        {
            // Retrieve and trim all inputs
            string firstName = TxtFirstName.Text.Trim();
            string lastName = TxtLastName.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string password = TxtPassword.Text.Trim();
            string phoneNumber = TxtPhoneNumber.Text.Trim();

            // Validate inputs
            string validationMessage = ValidateInput(firstName, lastName, email, password, phoneNumber);
            if (!string.IsNullOrEmpty(validationMessage))
            {
                LblError.ForeColor = System.Drawing.Color.Red;
                LblError.Text = validationMessage;
                return;
            }

            // Hash the password
            string hashedPassword = HashPassword(password);

            // Database connection string
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Customer (FirstName, LastName, Email, Password, PhoneNumber) " +
                                   "VALUES (@FirstName, @LastName, @Email, @Password, @PhoneNumber)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                        cmd.ExecuteNonQuery();

                        TxtFirstName.Text = string.Empty;
                        TxtLastName.Text = string.Empty;
                        TxtEmail.Text = string.Empty;
                        TxtPassword.Text = string.Empty;
                        TxtPhoneNumber.Text = string.Empty;

                        string script = "alert('Account created successfully. Click OK to login.'); window.location='Login.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
                    }
                }
            }
            catch (MySqlException ex)
            {
                LblError.ForeColor = System.Drawing.Color.Red;
                LblError.Text = "Database Error: " + ex.Message;
            }
            catch (Exception ex)
            {
                LblError.ForeColor = System.Drawing.Color.Red;
                LblError.Text = "An error occurred: " + ex.Message;
            }
        }

        private string ValidateInput(string firstName, string lastName, string email, string password, string phoneNumber)
        {
            if (string.IsNullOrEmpty(firstName))
                return "Please enter your first name.";

            if (!Regex.IsMatch(firstName, @"^[A-Z][a-z]+$"))
                return "The first letter of the first name must be capital, followed by lowercase letters.";

            if (string.IsNullOrEmpty(lastName))
                return "Please enter your last name.";

            if (!Regex.IsMatch(lastName, @"^[A-Z][a-z]+$"))
                return "The first letter of the last name must be capital, followed by lowercase letters.";

            if (string.IsNullOrEmpty(email))
                return "Please enter your email.";

            if (!Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                return "Please enter a valid email address.";

            if (string.IsNullOrEmpty(phoneNumber))
                return "Please enter your phone number.";

            if (!Regex.IsMatch(phoneNumber, @"^\d{10}$"))
                return "Please enter a valid 10-digit phone number.";

            if (string.IsNullOrEmpty(password))
                return "Please enter your password.";

            // Check password against the required pattern
            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10}$"))
                return "Password must be 10 characters with A-Z, a-z, 0-9 & special (@$!%*?&).";

            return null; // Validation successful
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        protected void LnkLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}
