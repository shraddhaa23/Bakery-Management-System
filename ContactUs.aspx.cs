using System;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;


namespace BakeryShop
{
    public partial class ContactUs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            // Get form data entered by the customer
            string customerName = TextBox1.Text;
            string customerEmail = TextBox2.Text;
            string messageContent = TextBox3.Text;

            // Validate Name
            if (string.IsNullOrEmpty(customerName))
            {
                Response.Write("Please enter your name.");
                return;
            }
            if (!Regex.IsMatch(customerName, "^[a-zA-Z\\s]+$"))
            {
                Response.Write("Please enter letters only in the name field.");
                return;
            }

            // Validate Email
            if (string.IsNullOrEmpty(customerEmail))
            {
                Response.Write("Please enter your email.");
                return;
            }
            if (!Regex.IsMatch(customerEmail, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
            {
                Response.Write("Please enter a valid email address (e.g., name@example.com).");
                return;
            }

            // Validate Message
            if (string.IsNullOrEmpty(messageContent))
            {
                Response.Write("Please enter your message.");
                return;
            }

            try
            {
                // Prepare the email message
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(customerEmail); // Customer's email
                mail.To.Add("shraddhanalawade2311@gmail.com"); // Bakery owner's email
                mail.Subject = $"Message from {customerName}";
                mail.Body = $"You have received a message from {customerName} ({customerEmail}):\n\n{messageContent}";

                // Configure SMTP client
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("shraddhanalawade2311@gmail.com", "erqe noaj qbnp jrky"); // Use your Gmail credentials
                smtp.EnableSsl = true; // Enable secure connection

                // Send the email
                smtp.Send(mail);

                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Your message has been sent successfully. Thank you for contacting us!');", true);
      
                TextBox1.Text = "";
                TextBox2.Text = "";
                TextBox3.Text = "";
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('An error occurred: {ex.Message}');</script>");
            }
        }
    }
}