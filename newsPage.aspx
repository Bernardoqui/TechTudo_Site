<%@ Page Title="" Language="C#" MasterPageFile="~/PagesTemplate.Master" AutoEventWireup="true" CodeBehind="newsPage.aspx.cs" Inherits="TechTudo.newsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css" rel="stylesheet">

<!-- Bootstrap JS e dependências Popper.js e jQuery -->
<script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main>

        <div class="textoNoticias d-flex align-items-center mb-3" style="color: white; margin-left: 46%;">
            <i class="fa-solid fa-newspaper fa-2x mr-2"></i>
            <h3 class="mb-0 display-2">Noticias</h3>
        </div>
        
        <div class="mainContainerNoticias" style="margin-left: 4%; position: relative; margin-bottom: 10vh; margin-top: auto; left: 5%; width: 85vw; height: auto; display: flex; flex-wrap: wrap; justify-content: center; box-shadow: 0 0 10px rgb(0, 0, 0);  background-color: rgb(36 34 34 / 78%); border-radius: 15px;">


            <div id="newsContainer" class="container-News">

                <asp:Xml ID="Xml1" runat="server" TransformSource="~/newsTech.xslt"></asp:Xml>
            </div>
            

        </div>
    </main>

</asp:Content>
