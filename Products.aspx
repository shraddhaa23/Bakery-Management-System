<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="BakeryShop.Products" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Products - YK Bakery</title>
    <link href="products_style.css" rel="stylesheet" type="text/css" />
    <script>
    function showFileName(input) {
        if (input.files.length > 0) {
            document.getElementById("fileNameText").innerText = input.files[0].name;
        } else {
            document.getElementById("fileNameText").innerText = "Choose File";
        }
    }
        function validateFileExtension(source, args) {
            var fileInput = document.getElementById('<%= fuProductImage.ClientID %>');
            if (fileInput.value) {
                var allowedExtensions = /(\.jpg|\.jpeg|\.png|\.gif)$/i;
                args.IsValid = allowedExtensions.test(fileInput.value);
            } else {
                args.IsValid = true; // If no file selected, allow form submission
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="products-container">
        <h2>Manage Products</h2>

        <!-- Add Product Form -->
        <div class="add-product">
            <h3>Add Product</h3>
            <div class="form-group">
                <asp:Label Text="Product Name:" runat="server" CssClass="label" />
                <asp:TextBox ID="txtProductName" runat="server" CssClass="textbox" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Product Name is required." ControlToValidate="txtProductName" Display="Dynamic" ForeColor="Red" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[a-zA-Z\s]+$" ErrorMessage="Product Name can only contain letters & spaces." ControlToValidate="txtProductName" Display="Dynamic" ForeColor="Red" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
            </div>
            <div class="form-group">
                <asp:Label Text="Category:" runat="server" CssClass="label" />
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="dropdown"></asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label Text="Description:" runat="server" CssClass="label" />
                <asp:TextBox ID="txtProductDescription" runat="server" CssClass="textbox" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Description is required." ControlToValidate="txtProductDescription" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="^[a-zA-Z0-9\s]+$" ErrorMessage="Description can only contain letters, numbers & spaces." ControlToValidate="txtProductDescription" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
            </div>
            <div class="form-group">
                <asp:Label Text="Price:" runat="server" CssClass="label" />
                <asp:TextBox ID="txtPrice" runat="server" CssClass="textbox" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Price is required." ControlToValidate="txtPrice" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationExpression="^\d+(\.\d+)?$"
 ErrorMessage="Price can only contain whole & decimal numbers" ControlToValidate="txtPrice" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
            </div>
            <div class="form-group">
                <asp:Label Text="Stock Quantity:" runat="server" CssClass="label" />
                <asp:TextBox ID="txtStock" runat="server" CssClass="textbox" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Stock Quantity is required." ControlToValidate="txtStock" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ValidationExpression="^\d+$"
 ErrorMessage="Stock Quantity can only contain whole numbers" ControlToValidate="txtStock" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
            </div>
            <div class="form-group">
                <asp:Label Text="Upload Image:" runat="server" CssClass="label" />
                 <!-- Custom File Upload Button -->
                <div class="custom-file-upload">
                    <label for="fuProductImage" class="custom-file-label">
                        <img src="/Images/upload.png" alt="Upload Icon"> 
                        <span id="fileNameText">Choose File</span>
                    </label>
                    <asp:FileUpload ID="fuProductImage" runat="server" CssClass="file-input" onchange="showFileName(this)" />
                    <asp:CustomValidator ID="cvFileExtension" runat="server" ControlToValidate="fuProductImage" ClientValidationFunction="validateFileExtension" OnServerValidate="cvFileExtension_ServerValidate"
     ErrorMessage="Only .png, .jpg, .jpeg, and .gif files are allowed."
    Display="Dynamic" CssClass="validation-message" ValidationGroup="FormValidation" ForeColor="Red" />
                </div>
            </div>
            <asp:Button ID="btnAddProduct" Text="Add Product" runat="server" OnClick="btnAddProduct_Click" CssClass="btn btn-add" ValidationGroup="FormValidation"/>
        </div>

        <hr />

        <!-- Product List -->
        <div class="product-list">
            <h3>Product List</h3>
            <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" DataKeyNames="ProductId" 
                OnRowCommand="gvProducts_RowCommand"
                OnPageIndexChanging="gvProducts_PageIndexChanging" AllowPaging="true" PageSize="10" CssClass="product-table">
                <Columns>
                    <asp:BoundField DataField="ProductId" HeaderText="Product ID" />
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                    <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                    <asp:ImageField DataImageUrlField="ImageUrl" HeaderText="Product Image" ControlStyle-Width="100px" />
                    <asp:BoundField DataField="StockQuantity" HeaderText="Stock" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-edit" CommandName="EditProduct" CommandArgument='<%# Eval("ProductId") %>' />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-delete" CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" />
    </div>
</asp:Content>
