<%@ Page Language="C#" MasterPageFile="~/User.Master" AutoEventWireup="true" CodeBehind="Terms.aspx.cs"
    Inherits="BakeryShop.Terms" %>

    <asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
        <title>T&C - YK Bakery</title>
        <link href="style.css" rel="stylesheet" type="text/css" />
    </asp:Content>

    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <h2 class="ql-title">Terms and Conditions</h2>
        <p class="context">By accessing and using the YK Bakery website, you agree to be bound by the following terms and conditions.
            If you do not agree with any of these terms, please refrain from using our website.
        </p>

        <strong class="ql-heading">Use of the Website:</strong>
        <ul class="ql-ul">
            <li>The content on this website is for personal, non-commercial use only. You may not copy, distribute,
                or modify any of the materials without prior permission.</li>
            <li>We reserve the right to modify, suspend, or discontinue any part of the website at any time without
                notice.</li>
        </ul>

        <strong class="ql-heading">Account Responsibility:</strong>
        <ul class="ql-ul">
            <li>When creating an account, you agree to provide accurate, current, and complete information.
                You are responsible for maintaining the confidentiality of your account information
                and for all activities that occur under your account.</li>
            <li>You must notify us immediately of any unauthorized use of your account.</li>
        </ul>

        <strong class="ql-heading">Ordering and Payments:</strong>
        <ul class="ql-ul">
            <li>All orders placed through the website are subject to acceptance. Prices and availability of products are
                subject
                to change without notice.</li>
            <li>Payments for orders are processed securely via third-party payment processors. You are responsible for
                providing accurate payment information.</li>
        </ul>

        <strong class="ql-heading">Delivery and Shipping:</strong>
        <ul class="ql-ul">
            <li>Delivery times are estimated and may vary depending on location and other factors.</li>
            <li>YK Bakery is not liable for any delays caused by third-party shipping services.
            </li>
        </ul>

        <strong class="ql-heading">Returns and Refund:</strong>
        <ul class="ql-ul">
            <li>We offer returns or exchanges on certain items, subject to our return policy. Please refer to the return
                policy for more details.</li>
        </ul>

        <strong class="ql-heading">Modifications to Term:</strong>
        <ul class="ql-ul">
            <li>We may update these Terms and Conditions at any time. Any changes will be posted on this page, and
                the revised terms will take effect immediately upon posting.</li>
        </ul>

        <strong class="ql-heading">Governing Law:</strong>
        <ul class="ql-ul">
            <li>These Terms and Conditions are governed by and construed in accordance with the laws of the country
                in which the website is operated.</li>
        </ul>
    </asp:Content>