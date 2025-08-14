<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bill.aspx.cs" Inherits="BakeryShop.Bill" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Invoice Receipt</title>
    <link rel="shortcut icon" href="/Images/favicon.ico" type="image/x-icon"/>
    <link href="Bill_style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="billForm" runat="server">
        <div class="invoice-container">
        <h1>Invoice Receipt</h1>
        <div class="invoice-section">
            <h2>Order Details</h2>
            <table class="invoice-table">
                <tr><td>Invoice ID:</td><td><asp:Label ID="lblInvoiceID" runat="server" /></td></tr>
                <tr><td>Customer Name:</td><td><asp:Label ID="lblCustomerName" runat="server" /></td></tr>
                <tr><td>Order ID:</td><td><asp:Label ID="lblOrderID" runat="server" /></td></tr>
                <tr><td>Order Date:</td><td><asp:Label ID="lblOrderDate" runat="server" /></td></tr>
                <tr><td>Total Amount:</td><td><asp:Label ID="lblTotalAmount" runat="server" /></td></tr>
                <tr><td>Shipping Address:</td><td><asp:Label ID="lblDeliveryAddress" runat="server" /></td></tr>
            </table>
            </div>

            <div class="invoice-section">
            <h2>Order Items</h2>
            <asp:GridView ID="gvOrderDetails" runat="server" AutoGenerateColumns="False" CssClass="invoice-grid">
                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="Product" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="Price" HeaderText="Price" />
                    <asp:BoundField DataField="Total" HeaderText="Total" />
                </Columns>
            </asp:GridView>
            </div>
            
            <div class="button-container">
            <asp:Button ID="btnExportPDF" runat="server" Text="Export to PDF" CssClass="invoice-button" OnClick="btnExportPDF_Click" />
            <asp:Button ID="btnCS" runat="server" Text="Continue Shopping" CssClass="invoice-button" OnClick="btnCS_Click"/>
            </div>
        </div>
    </form>
</body>
</html>
