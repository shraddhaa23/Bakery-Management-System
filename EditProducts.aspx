<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditProducts.aspx.cs" Inherits="BakeryShop.EditProducts" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Edit Product - YK Bakery</title>
    <link href="editproducts_style.css" rel="stylesheet" type="text/css" />
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
    <div class="edit-product-container">
        <h2>Edit Product</h2>

        <!-- Product Name -->
        <div class="form-group">
            <label>Product Name:</label>
            <asp:TextBox ID="txtProductName" runat="server" CssClass="textbox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Product Name is required." ControlToValidate="txtProductName" Display="Dynamic" ForeColor="Red" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[a-zA-Z\s]+$" ErrorMessage="Product Name can only contain letters & spaces." ControlToValidate="txtProductName" Display="Dynamic" ForeColor="Red" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
        </div>

        <!-- Category Dropdown -->
        <div class="form-group">
            <label>Category:</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="dropdown"></asp:DropDownList>
        </div>

        <!-- Product Description -->
        <div class="form-group">
            <label>Description:</label>
            <asp:TextBox ID="txtProductDescription" runat="server" CssClass="textbox" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Description is required." ControlToValidate="txtProductDescription" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="^[a-zA-Z0-9\s]+$" ErrorMessage="Description can only contain letters, numbers & spaces." ControlToValidate="txtProductDescription" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
        </div>

        <!-- Price -->
        <div class="form-group">
            <label>Price:</label>
            <asp:TextBox ID="txtPrice" runat="server" CssClass="textbox"></asp:TextBox>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Price is required." ControlToValidate="txtPrice" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RequiredFieldValidator>
               <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationExpression="^\d+(\.\d+)?$"
ErrorMessage="Price can only contain whole & decimal numbers" ControlToValidate="txtPrice" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
        </div>

        <!-- Stock Quantity -->
        <div class="form-group">
            <label>Stock Quantity:</label>
            <asp:TextBox ID="txtStock" runat="server" CssClass="textbox"></asp:TextBox>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Stock Quantity is required." ControlToValidate="txtStock" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RequiredFieldValidator>
               <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ValidationExpression="^\d+$"
ErrorMessage="Stock Quantity can only contain whole numbers" ControlToValidate="txtStock" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
         </div>

        <!-- Current Product Image -->
        <div class="form-group">
            <label>Current Image:</label>
            <asp:Image ID="imgProduct" Width="100" runat="server" CssClass="img-preview" />
        </div>

        <!-- Upload New Image -->
        <div class="form-group">
            <label>Upload New Image:</label>
            <div class="custom-file-upload">
                <label for="fuProductImage" class="custom-file-label">
                    <img src="/Images/upload.png" alt="Upload Icon"> 
                    <span id="fileNameText">Choose File</span>
                </label>
                <asp:FileUpload ID="fuProductImage" runat="server" CssClass="file-input" onchange="showFileName(this)" />
                <asp:CustomValidator ID="cvFileExtension" runat="server" ControlToValidate="fuProductImage" ClientValidationFunction="validateFileExtension" OnServerValidate="cvFileExtension_ServerValidate" ErrorMessage="Only .png, .jpg, .jpeg, and .gif files are allowed." Display="Dynamic" CssClass="validation-message" ValidationGroup="FormValidation" ForeColor="Red" />
            </div>
        </div>
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" />
        <!-- Update Button -->
        <div class="form-group">
            <asp:Button ID="btnUpdate" runat="server" Text="Update Product" CssClass="btn btn-update" OnClick="btnUpdateProduct_Click" ValidationGroup="FormValidation" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-cancel" PostBackUrl="Products.aspx" />
        </div>
    </div>
</asp:Content>
