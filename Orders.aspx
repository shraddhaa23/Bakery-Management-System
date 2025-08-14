<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="BakeryShop.Orders" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Orders - YK Bakery</title>
    <link href="orders_style.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="orders-container">
    <h2>Manage Customer Orders</h2>

    <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>

    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" OnRowCancelingEdit="gvOrders_RowCancelingEdit" OnRowCommand="gvOrders_RowCommand" OnPageIndexChanging="gvOrders_PageIndexChanging" CssClass="table" AllowPaging="true" PageSize="10">
    <Columns>
        <asp:BoundField DataField="OrderId" HeaderText="Order ID" />
        <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
        <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
        <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:dd-MM-yyyy}" />
        <asp:BoundField DataField="OrderStatus" HeaderText="Status" />
        
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:Button ID="btnProcess" runat="server" CommandName="Process" Text="Mark as Processing" 
                    CommandArgument='<%# Eval("OrderId") %>' CssClass="btn btn-process" />
                <asp:Button ID="btnDeliver" runat="server" CommandName="Deliver" Text="Mark as Delivered" 
                    CommandArgument='<%# Eval("OrderId") %>' CssClass="btn btn-deliver" />
                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" 
                    CommandArgument='<%# Eval("OrderId") %>' CssClass="btn btn-danger" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</div>
</asp:Content>