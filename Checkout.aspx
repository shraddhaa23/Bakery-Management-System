<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="BakeryShop.Checkout" %>

<!DOCTYPE html>
<html>

<head runat="server">
    <title>Checkout - YK Bakery</title>
    <link href="checkout_style.css" rel="stylesheet" />
    <link rel="shortcut icon" href="/Images/favicon.ico" type="image/x-icon">
    <script type="text/javascript">
        function validateTotalLength() {
            var txtFlat = document.getElementById('<%= txtFlat.ClientID %>').value;
            var txtLocality = document.getElementById('<%= txtLocality.ClientID %>').value;
            var txtLandmark = document.getElementById('<%= txtLandmark.ClientID %>').value;
            var txtPincode = document.getElementById('<%= txtPincode.ClientID %>').value;
            var txtCity = document.getElementById('<%= txtCity.ClientID %>').value;
            var txtState = document.getElementById('<%= txtState.ClientID %>').value;

            var totalLength = txtFlat.length + txtLocality.length + txtLandmark.length +
                              txtPincode.length + txtCity.length + txtState.length;

            if (totalLength > 255) {
                alert('The total length of all fields must not exceed 255 characters.');
                return false;
            }
            return true;
        }
    </script>
</head>

<body>
    <form id="form1" runat="server" onsubmit="return validateTotalLength();">
        <div class="checkout-container">
            <asp:ScriptManager ID="ScriptManager1" runat="server" />

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Contact Information Section -->
                    <div class="contact-information">
                        <h3>Contact Information</h3>
                        <asp:Label ID="lblCustomerName" runat="server" CssClass="contact-detail"></asp:Label>
                        <asp:Label ID="lblCustomerEmail" runat="server" CssClass="contact-detail"></asp:Label>
                        <asp:Label ID="lblCustomerPhone" runat="server" CssClass="contact-detail"></asp:Label>
                    </div>

                    <!-- Address Information Section -->
                    <div class="address-information">
                        <h3>Address Information</h3>

                        <!-- Flat No / Building Name -->
                        <div class="input-field-container">
                            <asp:TextBox ID="txtFlat" runat="server" CssClass="input-field"
                                Placeholder="Flat No / Building Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFlat" runat="server" ControlToValidate="txtFlat"
                                Display="Dynamic" ValidationGroup="FormValidation" ErrorMessage="Flat No / Building Name is required."
                                CssClass="validation-message"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revFlat" runat="server"
                                ValidationExpression="^[a-zA-Z0-9\s\-/]+$"
                                ErrorMessage="Flat No / Building Name can only contain letters, numbers, spaces, '-' or '/'."
                                ControlToValidate="txtFlat" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
                        </div>

                        <!-- Locality / Area / Street -->
                        <div class="input-field-container">
                            <asp:TextBox ID="txtLocality" runat="server" CssClass="input-field"
                                Placeholder="Locality / Area / Street"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvLocality" runat="server" ControlToValidate="txtLocality"
                                Display="Dynamic" ValidationGroup="FormValidation" ErrorMessage="Locality / Area / Street is required."
                                CssClass="validation-message"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revLocality" runat="server"
                                ValidationExpression="^[a-zA-Z0-9\s]+$"
                                ErrorMessage="Locality / Area / Street can only contain letters, numbers, and spaces."
                                ControlToValidate="txtLocality" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
                        </div>

                        <!-- Landmark -->
                        <div class="input-field-container">
                            <asp:TextBox ID="txtLandmark" runat="server" CssClass="input-field"
                                Placeholder="Landmark"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvLandmark" runat="server" ControlToValidate="txtLandmark"
                                Display="Dynamic" ValidationGroup="FormValidation" ErrorMessage="Landmark is required." CssClass="validation-message"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revLandmark" runat="server"
                                ValidationExpression="^[a-zA-Z0-9\s\-/,]+$"
                                ErrorMessage="Landmark can only contain letters, numbers, spaces, and special characters like '-' or ','."
                                ControlToValidate="txtLandmark" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
                        </div>

                        <!-- Pincode -->
                        <div class="input-field-container">
                            <asp:TextBox ID="txtPincode" runat="server" CssClass="input-field"
                                Placeholder="Pincode"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPincode" runat="server" ControlToValidate="txtPincode"
                                Display="Dynamic" ValidationGroup="FormValidation" ErrorMessage="Pincode is required." CssClass="validation-message"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvPincode" runat="server" ControlToValidate="txtPincode"
                                MinimumValue="100000" MaximumValue="999999" Type="Integer"
                                ErrorMessage="Pincode must be a 6-digit number." Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RangeValidator>
                        </div>

                        <!-- City -->
                        <div class="input-field-container">
                            <asp:TextBox ID="txtCity" runat="server" CssClass="input-field" Placeholder="City"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity"
                                Display="Dynamic" ValidationGroup="FormValidation" ErrorMessage="City is required." CssClass="validation-message"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revCity" runat="server"
                                ValidationExpression="^[a-zA-Z\s]+$"
                                ErrorMessage="City must contain only alphabetic characters."
                                ControlToValidate="txtCity" ValidationGroup="FormValidation" Display="Dynamic" CssClass="validation-message"></asp:RegularExpressionValidator>
                        </div>

                        <!-- State -->
                        <div class="input-field-container">
                            <asp:TextBox ID="txtState" runat="server" CssClass="input-field" Placeholder="State"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="txtState"
                                Display="Dynamic" ValidationGroup="FormValidation" ErrorMessage="State is required." CssClass="validation-message"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revState" runat="server"
                                ValidationExpression="^[a-zA-Z\s]+$"
                                ErrorMessage="State must contain only alphabetic characters."
                                ControlToValidate="txtState" Display="Dynamic" ValidationGroup="FormValidation" CssClass="validation-message"></asp:RegularExpressionValidator>
                        </div>
                    </div>

                    <!-- Price Details Section -->
                    <div class="price-details">
                        <h3>Price Details</h3>
                        <asp:Label ID="lblTotalItems" runat="server" CssClass="price-detail"></asp:Label>
                        <asp:Label ID="lblSubtotal" runat="server" CssClass="price-detail"></asp:Label>
                    </div>

                    <!-- Payment Method Section -->
                    <div class="payment-method">
                        <h3>Payment Method</h3>
                        <asp:CheckBox ID="chkCashOnDelivery" runat="server" Text="Cash on Delivery"
                            AutoPostBack="True" OnCheckedChanged="chkCashOnDelivery_CheckedChanged" />
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="label" ForeColor="Red"></asp:Label>
                    <!-- Confirm Order Button -->
                    <div class="confirm-order">
                        <asp:Button ID="btnConfirmOrder" runat="server" Text="Confirm Order"
                            OnClick="btnConfirmOrder_Click" ValidationGroup="FormValidation" CssClass="button-confirm" Enabled="false" />
                    </div>

                    <!-- Debugging Label -->
                    <asp:Label ID="lblDebug" runat="server" CssClass="debug-label"></asp:Label>
                </ContentTemplate>

                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="chkCashOnDelivery" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnConfirmOrder" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

        </div>
    </form>
</body>

</html>
