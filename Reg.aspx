<%@ Page Language="C#" MasterPageFile="~/User.Master" AutoEventWireup="true" CodeBehind="Reg.aspx.cs" Inherits="BakeryShop.Reg" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <title>Registration - YK Bakery</title>
    <link href="reg_style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="sign-up">
        <asp:Label ID="LblTitle" runat="server" Text="Create an account" CssClass="title"></asp:Label>
        
        <asp:Label ID="LblFirstName" runat="server" Text="First Name" CssClass="label"></asp:Label>
        <asp:TextBox ID="TxtFirstName" runat="server" Placeholder="First Name" CssClass="textbox"></asp:TextBox>
        <asp:RequiredFieldValidator ID="ReqFirstName" runat="server" ControlToValidate="TxtFirstName" ErrorMessage="Please enter your first name." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegexFirstName" runat="server" ControlToValidate="TxtFirstName" ErrorMessage="The first letter must be capital, followed by lowercase letters with no spaces." ForeColor="Red" ValidationExpression="^[A-Z][a-z]+$" Display="Dynamic" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>
        
        <asp:Label ID="LblLastName" runat="server" Text="Last Name" CssClass="label"></asp:Label>
        <asp:TextBox ID="TxtLastName" runat="server" Placeholder="Last Name" CssClass="textbox"></asp:TextBox>
        <asp:RequiredFieldValidator ID="ReqLastName" runat="server" ControlToValidate="TxtLastName" ErrorMessage="Please enter your last name." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegexLastName" runat="server" ControlToValidate="TxtLastName" ErrorMessage="The first letter must be capital, followed by lowercase letters with no spaces." ForeColor="Red" ValidationExpression="^[A-Z][a-z]+$" Display="Dynamic" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>
        
        <asp:Label ID="LblEmail" runat="server" Text="Email" CssClass="label"></asp:Label>
        <asp:TextBox ID="TxtEmail" runat="server" Placeholder="Email" CssClass="textbox" TextMode="Email"></asp:TextBox>
        <asp:RequiredFieldValidator ID="ReqEmail" runat="server" ControlToValidate="TxtEmail" ErrorMessage="Please enter your email." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegexEmail" runat="server" ControlToValidate="TxtEmail" ErrorMessage="Please enter a valid email address." ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>
        
        <asp:Label ID="LblPhoneNumber" runat="server" Text="Phone Number" CssClass="label"></asp:Label>
        <asp:TextBox ID="TxtPhoneNumber" runat="server" Placeholder="Phone Number" CssClass="textbox" TextMode="Number"></asp:TextBox>
        <asp:RequiredFieldValidator ID="ReqPhoneNumber" runat="server" ControlToValidate="TxtPhoneNumber" ErrorMessage="Please enter your phone number." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegexPhoneNumber" runat="server" ControlToValidate="TxtPhoneNumber" ErrorMessage="Please enter a valid 10-digit phone number." ValidationExpression="^\d{10}$" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>
        
        <asp:Label ID="LblPassword" runat="server" Text="Password" CssClass="label"></asp:Label>
        <asp:TextBox ID="TxtPassword" runat="server" Placeholder="Password" CssClass="textbox" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="ReqPassword" runat="server" ControlToValidate="TxtPassword" ErrorMessage="Please enter your password." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegexPassword" runat="server" ControlToValidate="TxtPassword" ErrorMessage="Password must be 10 characters with A-Z, a-z, 0-9 & special (@$!%*?&)." ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10}$" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>
        
        <asp:Label ID="LblError" runat="server" CssClass="label" ForeColor="Red"></asp:Label>
        
        <div class="button-container">
            <asp:Button ID="BtnSignUp" runat="server" Text="Sign Up" CssClass="button" ToolTip="Sign Up" OnClick="BtnSignUp_Click" ValidationGroup="FormValidation" />
        </div>
        
        <asp:Label ID="LblSuccess" runat="server" CssClass="label"></asp:Label>
        
        <div class="context-container">
            <asp:Label ID="LblExistingAccount" runat="server" Text="Already have an account? "></asp:Label>
            <asp:LinkButton ID="BtnLoginRedirect" runat="server" CssClass="linkbutton" ToolTip="Login" OnClick="LnkLogin_Click">Login</asp:LinkButton>
        </div>
    </div>
</asp:Content>
