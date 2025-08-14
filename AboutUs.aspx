<%@ Page Language="C#" MasterPageFile="~/User.Master" AutoEventWireup="true" CodeBehind="AboutUs.aspx.cs" Inherits="BakeryShop.AboutUs" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <title>About Us - YK Bakery</title>
    <link href="AboutUs_style.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="image-container">
            <img src="/Images/back.jpg" alt="Bakery Image" />
            <div class="text-overlay">
               Behind the Oven: Our Story
            </div>
     </div>
    <div class="container">
        <h1>Welcome to YK Bakery</h1>
        <div class="welcome-text">
        <p>At YK Bakery, we welcome you to experience the joy of freshly baked delights. From irresistible cakes to artisan bread and delicate pastries, our creations are made to bring happiness to your table. Whether you're celebrating a special occasion or treating yourself to a simple indulgence, every bite reflects our passion for baking and commitment to excellence. Combining innovation with traditional techniques, YK Bakery ensures a delightful experience in every flavor and texture.</p>
        </div>
        <div class="center-image">
            <img src="/Images/welcome.jpg" alt="Freshly baked goods">
        </div>

        <h2>How It All Started</h2>
        <div class="section">
            <img src="/Images/history.png" alt="Founders baking">
            <div class="text">
                <p>Founded in 1995 by a devoted couple, YK Bakery began as a small kitchen venture inspired by their shared love for baking. What started with small orders for friends and family quickly grew into a flourishing bakery, spreading joy through fresh ingredients and timeless recipes. Built on dedication, creativity, and a promise of quality, YK Bakery has become a cherished name and is now preparing to open new branches to reach even more communities.</p>
            </div>
        </div>

        <h2>Craftsmanship</h2>
        <div class="section">
            <div class="text">
                <p>At YK Bakery, our bakers are artisans who pour their hearts into every creation. Led by our head baker, whose years of experience and creativity set the tone for perfection, we ensure every item is made with care and precision. Craftsmanship is at the core of our philosophy. From hand-selecting the finest ingredients to preparing everything from scratch, every product reflects our dedication to quality and artistry. With a focus on creating textures and flavors that delight, we make each bake a true masterpiece.</p>
            </div>
            <img src="/Images/craftmanship.png" alt="Baker crafting bread">
        </div>
    </div>
</asp:Content>