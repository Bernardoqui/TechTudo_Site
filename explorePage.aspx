<%@ Page Title="" Language="C#" MasterPageFile="~/PagesTemplate.Master" AutoEventWireup="true" CodeBehind="explorePage.aspx.cs" Inherits="TechTudo.explorePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main>
        <div class="container-items2">
            

            <div class="container-filtro">

                <asp:Repeater ID="Repeater1" runat="server" EnableViewState="true">

                    <ItemTemplate>
                        <div class="body-filtro">
                            <div class="filtro-botao">
                                <fieldset>
                                    <legend><%# Eval("NomeCaracteristica") %></legend>
                                    <br>
                                    <asp:Repeater ID="Repeater2" runat="server" DataSource='<%# Eval("Valores") %>' EnableViewState="true">
                                        <ItemTemplate>
                                            <label for="checkbox-3">
                                               <asp:CheckBox ID="cb_filtr" runat="server" class="cb_filtro" name='<%# Container.DataItem %>' onclick="scrollToTop()"/>
                                               <%# Container.DataItem %>
                                            </label>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <br>
                                </fieldset>
                            </div>
                        </div>
                    </ItemTemplate>

                </asp:Repeater>

                <asp:Button ID="btn_filtrar" runat="server" CssClass="btn_filtrar" Text="Filtrar" OnClick="btn_filtrar_Click"/>

            </div>

            <div class="ordenate">
                <div class="select-cont">
                    <asp:DropDownList ID="ddl_ordenar" class="select" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_ordenar_SelectedIndexChanged">
                        <asp:ListItem Value="1">Ordenar por:</asp:ListItem>
                        <asp:ListItem Value="2">Pre&#231;o Menor</asp:ListItem>
                        <asp:ListItem Value="3">Pre&#231;o Maior</asp:ListItem>
                        <asp:ListItem Value="4">Alfabetica</asp:ListItem>
                    </asp:DropDownList>
                </div>



                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <asp:UpdatePanel ID="UpdateProdutos" class="container-produto" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Repeater ID="rpt_cardDetails" runat="server">
                            <ItemTemplate>
                                <div class="container_explore">
                                    <div class="card">
                                        <div class="card-head">
                                            <a href="productPage.aspx?product=<%# Eval("cod").ToString() %>"">
                                                <asp:Image class="product-img" ID="Image1" runat="server" ImageUrl='<%# Eval("img") != null ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("img")) : "/img/default.png" %>' />
                                            </a>
                                        </div>
                                        <div class="card-body">
                                            <div class="product-desc">
                                                <span class="product-title">
                                                    <a href="productPage.aspx?product=<%# Eval("cod").ToString() %>""><%# Eval("nome") %></a>
                                                </span>
                                                <span class="product-caption"><%# Eval("categoria") %></span>
                                                <span class="product-rating">
                                                    <i class="fa fa-star"></i>
                                                    <i class="fa fa-star"></i>
                                                    <i class="fa fa-star"></i>
                                                    <i class="fa fa-star"></i>
                                                    <i class="fa fa-star grey"></i>
                                                </span>
                                            </div>
                                            <div>
    <% if (TechTudo.globalClass.type_user == "Revendedor")
        { %>
    <span class="product-price-promot"><b><%# ((decimal)Eval("price") * (1 - 0.20m)).ToString("0.00") %></b>€</span>
    <span class="product-price-old"><b><%# Eval("price") %></b>€</span>
    <% }
        else
        { %>
    <span class="product-price"><b><%# Eval("price") %></b>€</span>
    <% } %>
</div>

                                            <div class="buy-btn">
                                                <button type="button" class="btnAdicionar btn" data-produtoid='<%# Eval("cod") %>'>
                                                    <i class="fa fa-shopping-cart"></i>
                                                </button>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddl_ordenar" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btn_filtrar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
    </main>


     <div class="banners-container">
     <div class="banners">
         <div class="banner error">
             <div class="banner-icon"><i data-eva="alert-circle-outline" data-eva-fill="#ffffff" data-eva-height="48" data-eva-width="48"></i></div>
             <div class="banner-message">Oops! Something went wrong!</div>
             <div class="banner-close" onclick="hideBanners()"><i data-eva="close-outline" data-eva-fill="#ffffff"></i></div>
         </div>
         <div class="banner success">
             <div class="banner-icon"><i data-eva="checkmark-circle-outline" data-eva-fill="#ffffff" data-eva-height="48" data-eva-width="48"></i></div>
             <div class="banner-message">Artigo adicionado ao carrinho!</div>
             <div class="banner-close" onclick="hideBanners()"><i data-eva="close-outline" data-eva-fill="#ffffff"></i></div>
         </div>
         <div class="banner info">
             <div class="banner-icon"><i data-eva="info-outline" data-eva-fill="#ffffff" data-eva-height="48" data-eva-width="48"></i></div>
             <div class="banner-message">Here is some useful information</div>
             <div class="banner-close" onclick="hideBanners()"><i data-eva="close-outline" data-eva-fill="#ffffff"></i></div>
         </div>
     </div>
 </div>

 <script src="https://unpkg.com/eva-icons" onload="eva.replace()"></script>

 <script>

     function esperarTresSegundos() {
         return new Promise(resolve => {
             setTimeout(() => {
                 console.log("Após 3 segundos");
                 // Coloque aqui o código que você quer executar após a espera
                 resolve();
             }, 3000);
         });
     }

     const hideBanners = () => {
         document
             .querySelectorAll(".banner.visible")
             .forEach((b) => b.classList.remove("visible"));
     };

     const showBanner = async (selector) => {
         hideBanners();
         // Ensure animation plays even if the same alert type is triggered.
         requestAnimationFrame(() => {
             const banner = document.querySelector(selector);
             banner.classList.add("visible");
         });

         // Espera 3 segundos antes de chamar hideBanners
         await esperarTresSegundos();

         hideBanners();
     };

 </script>


    <script>

        $(".fa").on("mouseover", function () {
            var $this = $(this);
            $this.nextAll().removeClass("fa-star").addClass("fa-star-o");
            $this.prevAll().removeClass("fa-star-o").addClass("fa-star");
            $this.removeClass("fa-star-o").addClass("fa-star");
        });
        $(".fa").one("click", function () {
            var $this = $(this);
            $this.addClass("active").siblings().removeClass("active");
        });
        $(".fa").on("mouseleave", function () {
            var select = $(".active");
            select.nextAll().removeClass("fa-star").addClass("fa-star-o");
            select.prevAll().removeClass("fa-star-o").addClass("fa-star");
            select.removeClass("fa-star-o").addClass("fa-star");
        });


        // Verifica a posição de rolagem e exibe ou oculta o botão
        window.onscroll = function () {
            scrollFunction();
        };

        function scrollFunction() {
            var btn = document.getElementById("scrollTopBtn");
            if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
                btn.style.display = "block";
            } else {
                btn.style.display = "none";
            }
        }

        // Função para rolar a página para o topo
        function scrollToTop() {
            document.body.scrollTop = 0; // Para navegadores Safari
            document.documentElement.scrollTop = 0; // Para outros navegadores
        }

    </script>
    <script src="/js/script.js"></script>
    <script>
        const rangeInput = document.querySelectorAll(".range-input input"),
            priceInput = document.querySelectorAll(".price-input input"),
            range = document.querySelector(".sliderproduto .progress");
        let priceGap = 10;

        priceInput.forEach((input) => {
            input.addEventListener("input", (e) => {
                let minPrice = parseInt(priceInput[0].value),
                    maxPrice = parseInt(priceInput[1].value);

                if (maxPrice - minPrice >= priceGap && maxPrice <= rangeInput[1].max) {
                    if (e.target.className === "input-min") {
                        rangeInput[0].value = minPrice;
                        range.style.left = (minPrice / rangeInput[0].max) * 100 + "%";
                    } else {
                        rangeInput[1].value = maxPrice;
                        range.style.right = 100 - (maxPrice / rangeInput[1].max) * 100 + "%";
                    }
                }
            });
        });

        rangeInput.forEach((input) => {
            input.addEventListener("input", (e) => {
                let minVal = parseInt(rangeInput[0].value),
                    maxVal = parseInt(rangeInput[1].value);

                if (maxVal - minVal < priceGap) {
                    if (e.target.className === "range-min") {
                        rangeInput[0].value = maxVal - priceGap;
                    } else {
                        rangeInput[1].value = minVal + priceGap;
                    }
                } else {
                    priceInput[0].value = minVal;
                    priceInput[1].value = maxVal;
                    range.style.left = (minVal / rangeInput[0].max) * 100 + "%";
                    range.style.right = 100 - (maxVal / rangeInput[1].max) * 100 + "%";
                }
            });
        });



        $(document).ready(function () {
            $('.btnAdicionar').click(function () {
                // Obter o ID do produto
                var produtoId = $(this).data('produtoid');

                $.ajax({
                    type: 'POST',
                    url: 'explorePage.aspx/AdicionarAoCarrinho',
                    data: '{produtoId: ' + produtoId + '}',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (response) {
                        showBanner('.banner.success');
                    },
                    error: function () {
                        alert('Erro ao adicionar o produto ao carrinho.');
                    }
                });
            });
        });

    </script>

</asp:Content>
