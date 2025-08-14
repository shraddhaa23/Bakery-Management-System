<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="BakeryShop.Categories" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Categories - YK Bakery</title>
    <link href="categories_style.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="categories-container">
        <h2>Manage Categories</h2>

        <!-- Add Category Form -->
        <div class="add-category">
            <h3>Add Category</h3>
            <asp:Label ID="lblMessage" runat="server" CssClass="message-label"></asp:Label>
            <div class="form-group">
                <asp:Label ID="lblCategoryName" runat="server" Text="Category Name:" CssClass="label"></asp:Label>
                <asp:TextBox ID="txtCategoryName" runat="server" CssClass="textbox"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Category Name is required." ControlToValidate="txtCategoryName" Display="Dynamic" ForeColor="Red" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[a-zA-Z\s]+$" ErrorMessage="Category Name can only contain letters & spaces." ControlToValidate="txtCategoryName" Display="Dynamic" ForeColor="Red" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
            </div>
            <asp:Button ID="btnAddCategory" runat="server" Text="Add Category" OnClick="btnAddCategory_Click" CssClass="btn btn-add" ValidationGroup="FormValidation" />
        </div>

        <hr />

        <!-- Category List -->
        <div class="category-list">
            <h3>Category List</h3>
            <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False" DataKeyNames="CategoryId"
                OnRowCommand="gvCategories_RowCommand" OnPageIndexChanging="gvCategories_PageIndexChanging"
                AllowPaging="true" PageSize="10" CssClass="category-table">
                <Columns>
                    <asp:BoundField DataField="CategoryId" HeaderText="Category ID" />
                    <asp:BoundField DataField="CategoryName" HeaderText="Category Name" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditCategory" CssClass="btn btn-edit" CommandArgument='<%# Eval("CategoryId") %>' />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteCategory" CssClass="btn btn-delete" CommandArgument='<%# Eval("CategoryId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>