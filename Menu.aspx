<%@ Page Language="C#" MasterPageFile="~/User.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="BakeryShop.Menu" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <title>Menu - YK Bakery</title>
    <link href="Menu_style.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="menu-products-container">
        <h2>Products</h2>
        <asp:Label ID="lblCategoryName" runat="server" CssClass="menu-category-name"></asp:Label>
        <asp:Label ID="lblProducts" runat="server" CssClass="no-products-message" Visible="false"></asp:Label>
        <div class="menu-price-filter" id="menu_price_filter" runat="server">
            <label for="txtPriceFrom">Price From:</label>
            <asp:TextBox ID="txtPriceFrom" runat="server" CssClass="textbox" />
            
            <label for="txtPriceTo">Price To:</label>
            <asp:TextBox ID="txtPriceTo" runat="server" CssClass="textbox" />
            
            <asp:Button ID="btnApplyFilter" runat="server" Text="Apply Filter" OnClick="btnApplyFilter_Click"/>
        </div>
        <asp:Label ID="lblMessage" runat="server" CssClass="notification-label" Visible="false"></asp:Label>
        <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" CssClass="menu-products-grid" OnRowCommand="gvProducts_RowCommand">
            <Columns>
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />
                <asp:BoundField DataField="Description" HeaderText="Product Description" SortExpression="Description" />
                <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" DataFormatString="{0:C}" />
                <asp:ImageField DataImageUrlField="ImageUrl" HeaderText="Product Image" ControlStyle-Width="100px" />
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CssClass="menu-action-button" CommandName="AddToCart" CommandArgument='<%# Eval("ProductId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>