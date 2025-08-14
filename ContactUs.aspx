<%@ Page Language="C#" MasterPageFile="~/User.Master" AutoEventWireup="true" CodeBehind="ContactUs.aspx.cs" Inherits="BakeryShop.ContactUs" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <title>Contact Us - YK Bakery</title>
    <link href="ContactUs_style.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="CUPanel" runat="server" CssClass="cu-panel">
        <asp:Label ID="Label1" runat="server" Text="Get In Touch" CssClass="title"></asp:Label>
        <p class="label">Do you have anything in your mind to let us know? Kindly don't delay to connect to us by means of our contact form.</p>
        <asp:Label ID="Label2" runat="server" Text="Name" CssClass="label"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" Placeholder="Name" CssClass="textbox"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" ErrorMessage="Please enter your name." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBox1" ErrorMessage="Please enter letters only." ForeColor="Red" Display="Dynamic" ValidationExpression="^[a-zA-Z\s]+$" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>
        <asp:Label ID="Label3" runat="server" Text="Email" CssClass="label"></asp:Label>
        <asp:TextBox ID="TextBox2" runat="server" TextMode="Email" Placeholder="Email" CssClass="textbox"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox2" ErrorMessage="Please enter your email." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TextBox2" ErrorMessage="Please enter a valid email address (e.g., name@example.com)." ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" ValidationGroup="FormValidation"></asp:RegularExpressionValidator>
        <asp:Label ID="Label4" runat="server" Text="Message" CssClass="label"></asp:Label>
        <asp:TextBox ID="TextBox3" runat="server" Placeholder="Message" TextMode="MultiLine" CssClass="textbox"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox3" ErrorMessage="Please enter your message." ForeColor="Red" Display="Dynamic" ValidationGroup="FormValidation"></asp:RequiredFieldValidator>
        <asp:Label ID="lblError" runat="server" CssClass="label" ForeColor="Red"></asp:Label>
        <div class="button-container">
            <asp:Button ID="Button1" runat="server" Text="Send Message" ToolTip="Send Message" OnClick="Button1_Click" CssClass="button" ValidationGroup="FormValidation" />
        </div>
        <asp:Label ID="lblSuccess" runat="server" CssClass="label"></asp:Label>
    </asp:Panel>
</asp:Content>

