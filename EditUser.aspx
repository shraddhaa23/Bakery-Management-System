<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="BakeryShop.EditUser" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Edit User - YK Bakery</title>
    <link href="edituser_style.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="edit-user-container">
        <header>
            <h1>Edit User Information</h1>
        </header>

            <div class="form-group">
                <label for="txtFirstName">First Name</label>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="input-field" placeholder="Enter first name" />
                 <asp:RequiredFieldValidator ID="ReqFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="Please enter your first name." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
                 <asp:RegularExpressionValidator ID="RegexFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="The first letter must be capital, followed by lowercase letters with no spaces." ForeColor="Red" ValidationExpression="^[A-Z][a-z]+$" Display="Dynamic" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>
            </div>

            <div class="form-group">
                <label for="txtLastName">Last Name</label>
                <asp:TextBox ID="txtLastName" runat="server" CssClass="input-field" placeholder="Enter last name" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLastName" ErrorMessage="Please enter your last name." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtLastName" ErrorMessage="The first letter must be capital, followed by lowercase letters with no spaces." ForeColor="Red" ValidationExpression="^[A-Z][a-z]+$" Display="Dynamic" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>
            </div>

            <div class="form-group">
                <label for="txtEmail">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field" placeholder="Enter email" />
                <asp:RequiredFieldValidator ID="ReqEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please enter your email." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegexEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please enter a valid email address." ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>
            </div>

            <div class="form-group">
                <label for="txtPhoneNumber">Phone Number</label>
                <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="input-field" placeholder="Enter phone number" />
                <asp:RequiredFieldValidator ID="ReqPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber" ErrorMessage="Please enter your phone number." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegexPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber" ErrorMessage="Please enter a valid 10-digit phone number." ValidationExpression="^\d{10}$" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>
            </div>

            <div class="form-group">
                <asp:Button ID="btnUpdate" runat="server" Text="Update User" CssClass="btn btn-update" OnClick="btnUpdateUser_Click" ValidationGroup="FormValidation"/>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-cancel" PostBackUrl="Users.aspx" />
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="message-label" />
    </div>
</asp:Content>