<%@ Page Language="C#" MasterPageFile="~/User.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BakeryShop.Login" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <title>Login - YK Bakery</title>
    <link href="login_style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="login">
        <asp:Label ID="LblTitle" runat="server" Text="Login" CssClass="title"></asp:Label>
        
        <asp:Label ID="LblEmail" runat="server" CssClass="label" Text="Email"></asp:Label>
        <asp:TextBox ID="TxtEmail" runat="server" Placeholder="Email" CssClass="textbox" TextMode="Email"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RfvEmail" runat="server" ControlToValidate="TxtEmail" ErrorMessage="Please enter your email." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please enter a valid email address (e.g., name@example.com)." ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>

        <asp:Label ID="LblPassword" runat="server" CssClass="label" Text="Password"></asp:Label>
        <asp:TextBox ID="TxtPassword" runat="server" Placeholder="Password" TextMode="Password" CssClass="textbox"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RfvPassword" runat="server" ControlToValidate="TxtPassword" ErrorMessage="Please enter your password." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RevPassword" runat="server" ControlToValidate="TxtPassword" ErrorMessage="Password must be 10 characters with A-Z, a-z, 0-9 & special (@$!%*?&)." ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10}$" ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>

        <asp:Label ID="LblLoginType" runat="server" CssClass="label" Text="Login as:"></asp:Label>
        <asp:DropDownList ID="DdlLoginType" runat="server" CssClass="dropdown">
            <asp:ListItem Text="Customer" Value="Customer" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
        </asp:DropDownList>

        <asp:Label ID="LblError" runat="server" CssClass="label" ForeColor="Red"></asp:Label>

        <div class="button-container">
            <asp:Button ID="BtnLogin" runat="server" Text="Login" CssClass="button" ToolTip="Login" OnClick="BtnLogin_Click" ValidationGroup="FormValidation"/>
        </div>

        <div class="context-container">
            <asp:Label ID="LblNewCustomer" runat="server" Text="New Customer?"></asp:Label>
            <asp:LinkButton ID="LnkSignUp" runat="server" CssClass="linkbutton" ToolTip="Sign Up" OnClick="LnkSignUp_Click">Sign Up!</asp:LinkButton>
        </div>
    </div>
</asp:Content>
