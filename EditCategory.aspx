<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditCategory.aspx.cs" Inherits="BakeryShop.EditCategory" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Edit Category - YK Bakery</title>
    <link href="editcategory_style.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="edit-category-container">
        <h2>Edit Category</h2>

        <!-- Category ID (Read-Only) -->
        <div class="form-group">
            <label>Category ID:</label>
            <asp:Label ID="lblCategoryId" runat="server"></asp:Label>
        </div>

        <!-- Category Name Input -->
        <div class="form-group">
            <label>Category Name:</label>
            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="textbox" AutoPostBack="true" OnTextChanged="txtCategoryName_TextChanged"></asp:TextBox>
        </div>

        <!-- Category Slug (Read-Only) -->
        <div class="form-group">
            <label>Category Slug:</label>
            <asp:TextBox ID="txtCategorySlug" runat="server" CssClass="textbox" ReadOnly="true"></asp:TextBox>
        </div>

        <!-- Buttons -->
        <div class="form-group">
            <asp:Button ID="btnUpdate" runat="server" Text="Update Category" CssClass="btn btn-update" OnClick="btnUpdate_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-cancel" PostBackUrl="Categories.aspx" />
        </div>
    </div>
</asp:Content>
