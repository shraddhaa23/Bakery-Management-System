using System;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;

namespace BakeryShop
{
    public partial class Bill : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.QueryString["InvoiceId"] != null)
            {
                string invoiceId = Request.QueryString["InvoiceId"];  // Use string for InvoiceId

                if (!string.IsNullOrEmpty(invoiceId))
                {
                    LoadInvoice(invoiceId);  // Pass the InvoiceId as a string
                }
                else
                {
                    Response.Write("Invalid Invoice ID");
                }
            }
        }

        private void LoadInvoice(string invoiceId)
        {
            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                // Query to load invoice and customer details
                string query = @"SELECT i.InvoiceId,CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,o.OrderId, i.InvoiceDate, 
                o.TotalAmount,o.DeliveryAddress
                FROM 
                Invoice i
                JOIN 
                Customer c ON i.CustomerId = c.CustomerId
                JOIN 
                Orders o ON o.OrderId = i.OrderId  
                WHERE 
                i.InvoiceId = @InvoiceId
                ORDER BY 
                o.OrderDate DESC LIMIT 1";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblInvoiceID.Text = reader["InvoiceId"].ToString();
                        lblCustomerName.Text = reader["CustomerName"].ToString();
                        lblOrderID.Text = reader["OrderId"].ToString();
                        DateTime invoiceDate = Convert.ToDateTime(reader["InvoiceDate"]);
                        lblOrderDate.Text = invoiceDate.ToString("dd MMMM yyyy", new CultureInfo("en-US"));
                        decimal totalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                        lblTotalAmount.Text = totalAmount.ToString("C", new CultureInfo("en-IN"));
                        lblDeliveryAddress.Text = reader["DeliveryAddress"].ToString();
                    }
                }

                // Query to load order items into GridView (without Sr.No)
                string itemsQuery = @"
        SELECT 
            p.ProductName, 
            oi.Quantity, 
            oi.Price, 
            (oi.Quantity * oi.Price) AS Total 
        FROM 
            OrderItems oi
        JOIN 
            BakeryProducts p ON oi.ProductId = p.ProductId
        WHERE 
            oi.OrderId = (SELECT i.OrderId 
                          FROM Invoice i 
                          WHERE i.InvoiceId = @InvoiceId)";

                MySqlCommand itemsCmd = new MySqlCommand(itemsQuery, conn);
                itemsCmd.Parameters.AddWithValue("@InvoiceId", invoiceId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(itemsCmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                adapter.Fill(dt);

                // Bind order items to GridView
                gvOrderDetails.DataSource = dt;
                gvOrderDetails.DataBind();
            }
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            // Initialize PDF document
            Document pdfDoc = new Document(PageSize.A4, 50, 50, 25, 25);
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter.GetInstance(pdfDoc, memoryStream);

            pdfDoc.Open();

            // **Add Bakery Logo (Make sure the image path is correct)**
            string logoPath = Server.MapPath("~/Images/bakeryicon.png"); // Adjust path as needed
            if (File.Exists(logoPath))
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                logo.ScaleToFit(80f, 80f);
                logo.Alignment = Element.ALIGN_LEFT;
                pdfDoc.Add(logo);
            }

            // **Add Bakery Name and Address**
            Paragraph bakeryInfo = new Paragraph("YK Bakery\nShop No. 6, Gangapur Road, Nashik, \nPhone: +91 93725 78292\nEmail: contact@ykbakery.com\n\n",
                FontFactory.GetFont("Arial", 12, Font.BOLD));
            pdfDoc.Add(bakeryInfo);

            // **Add Title**
            Paragraph title = new Paragraph("Invoice Receipt", FontFactory.GetFont("Arial", 20, Font.BOLD));
            title.Alignment = Element.ALIGN_CENTER;
            pdfDoc.Add(title);

            // **Add Invoice Details**
            pdfDoc.Add(new Paragraph("Invoice Details:", FontFactory.GetFont("Arial", 14, Font.BOLD)));
            pdfDoc.Add(new Paragraph($"Invoice ID: {lblInvoiceID.Text}"));
            pdfDoc.Add(new Paragraph($"Customer Name: {lblCustomerName.Text}"));
            pdfDoc.Add(new Paragraph($"Order ID: {lblOrderID.Text}"));
            pdfDoc.Add(new Paragraph($"Order Date: {lblOrderDate.Text}"));
            pdfDoc.Add(new Paragraph($"Total Amount: {lblTotalAmount.Text}"));
            pdfDoc.Add(new Paragraph($"Shipping Address: {lblDeliveryAddress.Text}"));
            pdfDoc.Add(new Paragraph(" "));

            // **Add Table for Order Items**
            PdfPTable table = new PdfPTable(4); // 4 columns: Product Name, Quantity, Price, Total
            table.WidthPercentage = 100;
            table.AddCell("Product Name");
            table.AddCell("Quantity");
            table.AddCell("Price");
            table.AddCell("Total");

            decimal totalAmount = 0; // To accumulate the total amount of the order

            // Add order item rows from GridView
            foreach (GridViewRow row in gvOrderDetails.Rows)
            {
                string productName = row.Cells[0].Text;  // Product Name
                string quantity = row.Cells[1].Text;    // Quantity
                string price = row.Cells[2].Text;       // Price
                string total = row.Cells[3].Text;       // Total

                table.AddCell(productName);
                table.AddCell(quantity);
                table.AddCell(price);
                table.AddCell(total);

                decimal itemTotal = Convert.ToDecimal(total.Replace("₹", "")); // Remove currency symbol for calculation
                totalAmount += itemTotal;
            }

            // Add table to the document
            pdfDoc.Add(table);

            // **Total Amount to Pay (Bold & Right-Aligned)**
            Paragraph totalParagraph = new Paragraph($"\nTotal Amount to Pay: {totalAmount.ToString("C", new CultureInfo("en-IN"))}",
                FontFactory.GetFont("Arial", 12, Font.BOLD))
            {
                Alignment = Element.ALIGN_RIGHT
            };
            pdfDoc.Add(totalParagraph);

            // **Add Thank You Message at the Bottom**
            Paragraph thankYou = new Paragraph("\nThank you for shopping with us!\nWe hope you enjoy our fresh and delicious baked goods.\nVisit us again!",
                FontFactory.GetFont("Arial", 10, Font.ITALIC, BaseColor.DARK_GRAY))
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(thankYou);

            // **Add Terms & Conditions / Refund Policy**
            Paragraph termsHeader = new Paragraph("\nTerms & Conditions / Refund Policy", FontFactory.GetFont("Arial", 12, Font.BOLD));
            pdfDoc.Add(termsHeader);

            Paragraph termsContent = new Paragraph(
                "1. Orders once placed cannot be canceled unless they are in the 'Pending' state.\n" +
                "2. We do not provide refunds on perishable bakery items.\n" +
                "3. If you receive a damaged product, please contact us within 24 hours.\n" +
                "4. Delivery times may vary based on location and availability.\n" +
                "5. All payments are Cash on Delivery only.\n",
                FontFactory.GetFont("Arial", 10, Font.NORMAL)
            );
            pdfDoc.Add(termsContent);

            pdfDoc.Close();

            // **Send PDF to browser for download**
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename=Invoice_{lblInvoiceID.Text}.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(memoryStream.ToArray());
            Response.End();
        }


        protected void btnCS_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }
    }
}
