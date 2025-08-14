using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace BakeryShop
{
    public partial class EditUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if CustomerId is provided
                if (Request.QueryString["CustomerId"] != null)
                {
                    string customerId = Request.QueryString["CustomerId"];
                    LoadUserDetails(customerId);
                }
                else
                {
                    lblMessage.Text = "No user selected.";
                }
            }
        }

        private void LoadUserDetails(string customerId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CustomerId, FirstName, LastName, Email, PhoneNumber FROM Customer WHERE CustomerId = @CustomerId";
                MySqlCommand cmd = new MySqlCommand(query, conn);  
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                MySqlDataReader reader = cmd.ExecuteReader();  
                if (reader.Read())
                {
                    txtFirstName.Text = reader["FirstName"].ToString();
                    txtLastName.Text = reader["LastName"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtPhoneNumber.Text = reader["PhoneNumber"].ToString();
                }
                else
                {
                    lblMessage.Text = "User not found.";
                }
            }
        }

        protected void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["CustomerId"] != null)
            {
                string customerId = Request.QueryString["CustomerId"];
                string firstName = txtFirstName.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string phoneNumber = txtPhoneNumber.Text.Trim();

                
                string validationMessage = ValidateInput(firstName, lastName, email, phoneNumber);
                if (!string.IsNullOrEmpty(validationMessage))
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = validationMessage;
                    return;
                }

                // Update the user's information
                string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connectionString)) 
                {
                    conn.Open();
                    string query = "UPDATE Customer SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber WHERE CustomerId = @CustomerId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);  
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('User details updated successfully!');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Error updating user details.');", true);
                    }
                    Response.Redirect("Users.aspx");
                }
            }
        }

        private string ValidateInput(string firstName, string lastName, string email, string phoneNumber)
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

            return null; // Validation successful
        }
    }
}
