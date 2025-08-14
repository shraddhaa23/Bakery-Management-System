<%@ Page Language="C#" MasterPageFile="~/User.Master" AutoEventWireup="true" CodeBehind="Disclaimer.aspx.cs"
    Inherits="BakeryShop.Disclaimer" %>

    <asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
        <title>Disclaimer - YK Bakery</title>
        <link href="style.css" rel="stylesheet" type="text/css" />
    </asp:Content>

    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <h2 class="ql-title">Disclaimer</h2>
        <p class="context">The information provided on the YK Bakery website is intended for general informational purposes only.
            While we strive to ensure the accuracy and completeness of the content, we make no representations or
            warranties
            regarding the accuracy, reliability, or suitability of the information for any purpose.</p>
        <strong class="ql-heading">No Warranty:</strong>
        <ul class="ql-ul">
            <li>YK Bakery makes no warranties or representations about the completeness, reliability, or accuracy of the
                content
                on this website. The website and its services are provided "as is" without any warranties.</li>
        </ul>
        <strong class="ql-heading">Limitations of Liability:</strong>
        <ul class="ql-ul">
            <li>YK Bakery will not be liable for any direct, indirect, incidental, or consequential damages arising from
                the
                use of this website, including but not limited to product defects, errors, or omissions in the content.
            </li>
        </ul>
        <strong class="ql-heading">External Links:</strong>
        <ul class="ql-ul">
            <li>Our website may contain links to external sites. We are not responsible for the content, privacy
                policies,
                or practices of third-party websites.</li>
        </ul>
        <strong class="ql-heading">Product Availability:</strong>
        <ul class="ql-ul">
            <li>Product availability and prices are subject to change without notice. We do our best to ensure accurate
                product listings, but we do not guarantee availability.</li>
        </ul>
        <strong class="ql-heading">Health and Safety:</strong>
        <ul class="ql-ul">
            <li>Any health-related claims regarding the products on our website should be verified with a
                healthcare professional. YK Bakery is not responsible for any health or allergy-related issues.</li>
        </ul>
        <strong class="ql-heading">Changes to the Disclaimer:</strong>
        <ul class="ql-ul">
            <li>We may update or change the Disclaimer at any time. Please review this page periodically for updates.
            </li>
        </ul>
    </asp:Content>