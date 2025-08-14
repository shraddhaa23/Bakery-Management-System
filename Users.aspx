<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="BakeryShop.Users" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Users - YK Bakery</title>
    <link href="users_style.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="users-container">
        <header>
            <h1>Manage Users</h1>
        </header>

        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" CssClass="users-table" OnRowCommand="gvUsers_RowCommand" OnPageIndexChanging="gvUsers_PageIndexChanging"
       AllowPaging="true" PageSize="10" DataKeyNames="CustomerId">
    <Columns>
        <asp:BoundField DataField="CustomerId" HeaderText="Customer ID" ReadOnly="True" />
        <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
        <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
        <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" SortExpression="PhoneNumber" />

        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditUser"
                    CssClass="btn btn-edit" CommandArgument='<%# Eval("CustomerId") %>' />

                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteUser"
                    CssClass="btn btn-delete" CommandArgument='<%# Eval("CustomerId") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" />
    </div>
</asp:Content>
