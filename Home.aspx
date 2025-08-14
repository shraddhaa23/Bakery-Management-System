<%@ Page Language="C#" MasterPageFile="~/User.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BakeryShop.Home" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <title>Home - YK Bakery</title>
    <link href="home_style.css" rel="stylesheet" type="text/css" />
	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <section class="home" id="home">
        <div class="slides-container">
            <div class="slide active">
                <div class="content-heading">
                    <span>We're delighted to have you at YK Bakery!</span>
                </div>
                <div class="img">
                    <img src="/Images/Bakery1.png" alt="Bakery Image">
                </div>
            </div>
            <div class="slide">
                <div class="content-heading">
                    <span>Deliciously Yours</span>
                </div>
                <div class="img">
                    <img src="/Images/Bakery2.png" alt="Bakery Image">
                </div>
            </div>
            <div class="slide">
                <div class="content-heading">
                    <span>Endless Cravings</span>
                </div>
                <div class="img">
                    <img src="/Images/Bakery3.png" alt="Bakery Image">
                </div>
            </div>
        </div>
        <div id="prev-slide" class="fas fa-angle-left"></div>
        <div id="next-slide" class="fas fa-angle-right"></div>
    </section>


    <div class="heading">
        <span>CHEF'S FAVORITE</span>
    </div>

    <section class="banner-container">
        <div class="banner">
            <img src="/Images/Rawa_mawa_cake.png" alt="Cake">
            <div class="content">
                <span>Cakes</span>
                <a href="Menu.aspx?CategorySlug=cakes" class="btn">Buy now</a>
            </div>
        </div>
        <div class="banner">
            <img src="/Images/pineapple_pastry.jpg" alt="Pastry">
            <div class="content">
                <span>Pastries</span>
                <a href="Menu.aspx?CategorySlug=pastries" class="btn">Buy now</a>
            </div>
        </div>
        <div class="banner">
            <img src="/Images/Coconut_biscuits.png" alt="Biscuits">
            <div class="content">
                <span>Biscuits</span>
                <a href="Menu.aspx?CategorySlug=biscuits" class="btn">Buy now</a>
            </div>
        </div>
    </section>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const slides = document.querySelectorAll(".slide");
            let index = 0;

            function showSlide(nextIndex) {
                slides[index].classList.remove("active");
                index = (nextIndex + slides.length) % slides.length;
                slides[index].classList.add("active");
            }

            function next() {
                showSlide(index + 1);
            }

            function prev() {
                showSlide(index - 1);
            }

            document.getElementById("prev-slide").addEventListener("click", prev);
            document.getElementById("next-slide").addEventListener("click", next);

        });
    </script>
</asp:Content>