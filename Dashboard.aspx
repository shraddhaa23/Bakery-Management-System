<%@ Page Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="BakeryShop.Dashboard" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Dashboard - YK Bakery</title>
    <link href="dashboard_style.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-page">
    <div class="container mt-4">
        <!-- First Row: Total Orders, Total Sales, Total Customers -->
        <div class="row row-flex">
            <div class="col-lg-4">
                <div class="card text-center bg-light shadow">
                    <div class="card-body">
                        <h5>Total Orders</h5>
                        <asp:Label ID="lblTotalOrders" runat="server" CssClass="h3 text-primary"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="card text-center bg-light shadow">
                    <div class="card-body">
                        <h5>Total Sales (₹)</h5>
                        <asp:Label ID="lblTotalSales" runat="server" CssClass="h3 text-primary"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="card text-center bg-light shadow">
                    <div class="card-body">
                        <h5>Total Customers</h5>
                        <asp:Label ID="lblTotalCustomers" runat="server" CssClass="h3 text-primary"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <!-- Second Row: Pending Orders, Delivered Orders, Cancelled Orders -->
        <div class="row row-flex">
            <div class="col-lg-4">
                <div class="card text-center bg-warning text-white shadow">
                    <div class="card-body">
                        <h5>Pending Orders</h5>
                        <asp:Label ID="lblPendingOrders" runat="server" CssClass="h4"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="card text-center bg-success text-white shadow">
                    <div class="card-body">
                        <h5>Delivered Orders</h5>
                        <asp:Label ID="lblDeliveredOrders" runat="server" CssClass="h4"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="card text-center bg-danger text-white shadow">
                    <div class="card-body">
                        <h5>Cancelled Orders</h5>
                        <asp:Label ID="lblCancelledOrders" runat="server" CssClass="h4"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <!-- Recent Orders -->
        <div class="row">
            <div class="col-lg-12">
                <div class="card shadow">
                    <div class="card-body">
                        <h5>Recent Orders</h5>
                        <asp:GridView ID="gvRecentOrders" runat="server" CssClass="table table-bordered"></asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>
