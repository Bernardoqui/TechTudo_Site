    <%@ Page Title="" Language="C#" MasterPageFile="~/PagesTemplate.Master" AutoEventWireup="true" CodeBehind="homePage.aspx.cs" Inherits="TechTudo.homePage" EnableEventValidation="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main>
        <div class="carousel">
            <a href="#">
                <div tabindex="0" class="carousel--item one">
                    <h1>Periféricos</h1>
                </div>
            </a>
            <a href="#">
                <div tabindex="0" class="carousel--item two">
                    <h1>Desktops</h1>
                </div>
            </a>
            <a href="#">
                <div tabindex="0" class="carousel--item three">
                    <h1>Imagem</h1>
                </div>
            </a>
            <a href="#">
                <div tabindex="0" class="carousel--item four">
                    <h1>Som</h1>
                </div>
            </a>
        </div>

        <div class="slider-container">
            <div class="slider">
                <a href="#" class="slide one"></a>
                <a href="#" class="slide two"></a>
                <a href="#" class="slide three"></a>
            </div>
        </div>

        <h1 class="title_destaques">DESTAQUES</h1>

        <div class="separator2"></div>

        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdateDestaques" runat="server" class="container-items" UpdateMode="Conditional">
            <ContentTemplate>
                <h1 class="backHome"><a href="explorePage.aspx?filter=all">&leftarrow; Explorar</a></h1>

                <asp:Repeater ID="rpt_cardDetails" runat="server">
                    <ItemTemplate>

                        <div class="container">
                            <div class="card">
                                <div class="card-head">
                                    <a href="productPage.aspx?product=<%# Eval("cod").ToString() %>">
                                        <asp:Image class="product-img" ID="Image1" runat="server" ImageUrl='<%# Eval("img") != null ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("img")) : "/img/default.png" %>' />
                                    </a>
                                </div>
                                <div class="card-body">
                                    <div class="product-desc">
                                        <span class="product-title">
                                            <a href="productPage.aspx?product='<%# Eval("cod") %>'"><%# Eval("nome") %></a><p>#<%# Eval("cod") %></p>
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
                    <AlternatingItemTemplate>
                        <div class="container">
                            <div class="card">
                                <div class="card-head">
                                    <a href="productPage.aspx?product='<%# Eval("cod") %>'">
                                        <asp:Image class="product-img" ID="Image1" runat="server" ImageUrl='<%# Eval("img") != null ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("img")) : "/img/default.png" %>' />
                                    </a>
                                </div>
                                <div class="card-body">
                                    <div class="product-desc">
                                        <span class="product-title">
                                            <a href="#"><%# Eval("nome") %></a><p>#<%# Eval("cod") %></p>
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
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="pagination">
            <asp:LinkButton ID="lnkPrev" CssClass="btn_pagination" runat="server" OnClick="lnkPrev_Click">Anterior</asp:LinkButton>
            <asp:LinkButton ID="lnkNext" CssClass="btn_pagination" runat="server" OnClick="lnkNext_Click">Proximo</asp:LinkButton>
        </div>


        <div class="separator2"></div>

<div class="sub_title">
    <h1>Subscreva Já!!</h1>
</div>


<div class="sub_container">
    <div class="content">
        <div class="subscription">

            <input id="emailInput" class="add-email" type="email" placeholder="subscribe@me.now">
            <button class="submit-email btnSubscrever" type="button">
                <span class="before-submit">Subscribe</span>
                <span class="after-submit">Thank you for subscribing!</span>
            </button>
        </div>
    </div>
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

    <script type="text/javascript">
        var listaCodigos = [];

        function addToCart(codigo) {
            listaCodigos.push(codigo);

            console.log(listaCodigos);
        }
    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            $('.btnSubscrever').click(function () {
                var emailInput = document.getElementById('emailInput');
                var emailValue = emailInput.value;

                $.ajax({
                    type: 'POST',
                    url: 'homePage.aspx/Subscrever',
                    data: JSON.stringify({ email: emailValue }),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (response) {
                        
                    },
                    error: function () {
                        alert('Erro ao subscrever');
                    }
                });
            });
        });

        $(document).ready(function () {
            $('.btnAdicionar').click(function () {
                // Obter o ID do produto
                var produtoId = $(this).data('produtoid');

                $.ajax({
                    type: 'POST',
                    url: 'homePage.aspx/AdicionarAoCarrinho',
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

    <script>
        document.querySelector(".submit-email").addEventListener("mousedown", (e) => {
            e.preventDefault();

            // Obter o valor da caixa de texto
            const emailInput = document.querySelector(".add-email").value;

            // Expressão regular para validar um email
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

            // Verificar se o email é válido
            if (emailRegex.test(emailInput)) {
                // Se for válido, realizar a ação
                document.querySelector(".subscription").classList.add("done");
            } else {
                // Se não for válido, você pode exibir uma mensagem de erro ou realizar outra ação
                console.log("Por favor, insira um email válido");
            }
        });
    </script>
    

</asp:Content>
