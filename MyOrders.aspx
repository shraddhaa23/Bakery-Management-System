<%@ Page Language="C#" MasterPageFile="~/User.Master" AutoEventWireup="true" CodeBehind="MyOrders.aspx.cs" Inherits="BakeryShop.MyOrders" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <title>My Orders - YK Bakery</title>
    <link href="MyOrders_style.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="center-container">
    <h2>Your Orders</h2>

    <asp:Label ID="lblNoOrders" runat="server" Text="You don't have any orders yet. <a href='Menu.aspx'> Start shopping now</a> to find something you love!" Visible="false" CssClass="no-orders-msg" />
    <asp:Repeater ID="rptOrders" runat="server">
    <ItemTemplate>
        <div class='order-summary <%# Eval("OrderStatus", "{0}").ToLower() %>'>
            <h4 class="order-id">Order ID: <%# Eval("OrderId") %></h4>
            <p>Order Date: <%# Eval("OrderDate", "{0:dd-MM-yyyy}") %></p>
            <p>Order Status: <strong><%# Eval("OrderStatus") %></strong></p>

            <table class="table">
                <thead>
                    <tr>
                        <th>Product</th>
                        <th>Quantity</th>
                        <th>Price</th>
                        <th>Total</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptProducts" runat="server" DataSource='<%# Eval("Products") %>'>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ProductName") %></td>
                                <td><%# Eval("Quantity") %></td>
                                <td><%# Eval("Price", "{0:C}") %></td>
                                <td><%# Eval("Total", "{0:C}") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

            <p><strong>Order Total: <%# Eval("OrderTotal", "{0:C}") %></strong></p>

            <asp:Button ID="btnCancelOrder" runat="server" CssClass="cancel-btn" Text="Cancel Order"
                CommandArgument='<%# Eval("OrderId") %>' OnClick="CancelOrder_Click" 
                Visible='<%# Eval("OrderStatus").ToString() == "Pending" %>' />

            <hr />
        </div>
    </ItemTemplate>
</asp:Repeater>

    <!-- Confirmation Popup -->
    <asp:Panel ID="pnlConfirm" runat="server" CssClass="popup" Visible="false">
        <div class="popup-content">
            <p>Do you really want to cancel the order?</p>
            <asp:Button ID="btnYes" runat="server" CssClass="yes-btn" Text="Yes" OnClick="ConfirmCancel_Click" UseSubmitBehavior="false" />
            <asp:Button ID="btnNo" runat="server" CssClass="no-btn" Text="No" OnClick="ClosePopup_Click" />
        </div>
    </asp:Panel>
</div>
</asp:Content>
