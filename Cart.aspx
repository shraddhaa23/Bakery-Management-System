<%@ Page Language="C#" MasterPageFile="~/User.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs"
    Inherits="BakeryShop.Cart" %>

    <asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
        <title>Cart - YK Bakery</title>
        <link href="cart_style.css" rel="stylesheet" type="text/css" />
    </asp:Content>

    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanelCart" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="cart-page">
            <div class="cart-container">
                <h2 class="cart-heading">Your Cart</h2>
                
                <asp:Panel ID="pnlNotLoggedIn" runat="server" Visible="false">
                    <p class="cart-content">You must log in to view your cart.</p>
                    <asp:LinkButton ID="lnkLogin" class="cart-content" runat="server" PostBackUrl="Login.aspx">Login Now</asp:LinkButton>
                </asp:Panel>

                <asp:Panel ID="pnlEmptyCart" runat="server" Visible="false">
                    <p class="cart-content">Oops! Your cart is empty.</p>
                    <asp:LinkButton ID="lnkShopNow" class="cart-content" runat="server" PostBackUrl="Menu.aspx">Shop Now</asp:LinkButton>
                </asp:Panel>

                <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" CssClass="cart-grid"
                    OnRowCommand="gvCart_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Photo">
                            <ItemTemplate>
                                <img src='<%# ResolveUrl(Eval("ImageUrl").ToString()) %>' alt="Product Image"
                                    class="cart-product-image" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                        <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDecrease" runat="server" CssClass="quantity" CommandName="DecreaseQuantity"
                                    CommandArgument='<%# Eval("ProductId") %>'>−</asp:LinkButton>
                                <span><%# Eval("Quantity") %></span>
                                <asp:LinkButton ID="btnIncrease" runat="server" CssClass="quantity" CommandName="IncreaseQuantity"
                                    CommandArgument='<%# Eval("ProductId") %>'>+</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C}" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnRemove" runat="server" CommandName="RemoveProduct"
                                    CommandArgument='<%# Eval("ProductId") %>'>Remove</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <div class="cart-summary" runat="server" id="cartSummary" visible="false">
                    <p>
                        <strong>Subtotal (<asp:Literal ID="litTotalItems" runat="server" /> items):</strong>
                        <span><asp:Literal ID="litTotalAmount" runat="server" /></span>
                    </p>
                    <asp:Button ID="btnProceedToBuy" runat="server" Text="Proceed to Buy" CssClass="checkout-button"
                        OnClick="btnProceedToBuy_Click" />
                    <asp:Button ID="btnContinueShopping" runat="server" Text="Continue Shopping" CssClass="shopping-btn"
                        OnClick="btnContinueShopping_Click" />
                </div>
            </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>