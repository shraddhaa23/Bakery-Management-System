<%@ Page Language="C#" MasterPageFile="~/User.Master" AutoEventWireup="true" CodeBehind="DeliveryPolicy.aspx.cs" 
    Inherits="BakeryShop.DeliveryPolicy"%>

    <asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
        <title>Delivery Policy - YK Bakery</title>
        <link href="style.css" rel="stylesheet" type="text/css" />
    </asp:Content>

    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <h2 class="ql-title">Delivery Policy</h2>
        <strong class="ql-heading">Delivery Timings:</strong>
        <ul class="ql-ul">
            <li>All Orders will be taken from 9 AM to 10 PM on any given day. </li>
            <li>Orders placed after this time will be delivered the next business day.</li>
        </ul>
        <strong class="ql-heading">Delivery Fee:</strong>
        <ul class="ql-ul">
            <li>Delivery fee charged based on distance from the main kitchen.</li>
        </ul>
        <strong class="ql-heading">Order Tracking:</strong>
        <ul class="ql-ul">
            <li>Customers can track their orders in real time using the 'Track Order' feature on our website.</li>
        </ul>
        <strong class="ql-heading">Delivery Conditions:</strong>
        <ul class="ql-ul">
            <li>Deliveries are made to the provided address; ensure it is correct and accessible.</li>
            <li>If no one is available to accept the delivery, our team will attempt to contact you for instructions.
                If unsuccessful, the order will be returned to our bakery, and a redelivery charge may apply.</li>
        </ul>
        <strong class="ql-heading">Liability:</strong>
        <ul class="ql-ul">
            <li>We strive to deliver your items in perfect condition. However, we are not liable for delays
                or damages caused by unforeseen circumstances such as extreme weather, strikes, or other events beyond
                our control.</li>
        </ul>
        <strong class="ql-heading">Customer Support:</strong>
        <ul class="ql-ul">
            <li>Contact customer support for any inquiries or assistance regarding delivery orders.</li>
        </ul>
    </asp:Content>