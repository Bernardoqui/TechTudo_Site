<%@ Page Title="" Language="C#" MasterPageFile="~/PagesTemplate.Master" AutoEventWireup="true" CodeBehind="productPage.aspx.cs" Inherits="TechTudo.productPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://getbootstrap.com/docs/5.3/assets/css/docs.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <main>

    <div class="bg-dark text-white ">
      <div class="containerProduto">

        <div class="container-com-fundo">
          <div class="row">
            <!-- Lado Esquerdo -->
            <div class="col-md-5 text-end lado-esquerdo">
              <!-- Adicione aqui o seu carrossel ou imagens -->
              <div class="carousel-container">
                  <div id="carouselExampleIndicators" class="carousel slide mx-auto" style="max-width: 600px;">

                      <div class="carousel-inner">

                          <asp:Image ID="img_principal" runat="server" class="carousel-item" />

                          <asp:Repeater ID="rpt_imgs" runat="server">
                              <ItemTemplate>

                                  <asp:Image ID="img_secundaria" runat="server" class="carousel-item" ImageUrl='<%# "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Container.DataItem) %>' />
                              
                              </ItemTemplate>
                          </asp:Repeater>

                      </div>

                  </div>
              </div>
            </div>

            <!-- Lado Direito -->
              <div class="col-md-7 lado-direito text-center ">

                  <h3 class="product-name">
                      <asp:Label ID="lbl_title" runat="server" Text=""></asp:Label>
                  </h3>

                  <div class="separator"></div>


                  <div class="d-flex justify-content-center" style="margin: 10px; margin-top: 50px;">

                      <h3 class="product-name">
                          <asp:Label ID="lbl_preco" runat="server" Text=""></asp:Label>
                      </h3>

                      <% 
                          var preco = valor;

                          if (TechTudo.globalClass.type_user == "Revendedor")
                          {
                      %>
                            <span class="product-price-promot"><b><%= (preco * (1 - 0.20m)).ToString("0.00") %></b>€</span>
                            <span class="product-price-old"><b><%= preco.ToString("0.00") %></b>€</span>
                      <%
                          }
                      else
                          {
                      %>
                            <span class="product-price"><b><%= preco.ToString("0.00") %></b>€</span>
                      <%
                          }
                      %>

                      <asp:Button ID="btn_comprar" runat="server" Text="Comprar" OnCommand="funcs" CommandName="compra" class="btn btn-primary"
                          style="padding: 25px 40px; margin-right: 10px; font-size: 18px;"/>

                      <asp:Button ID="btn_favorito" runat="server" Text="Favoritos" OnCommand="funcs" CommandName="favorito" class="btn btn-primary"
                          Style="padding: 25px 40px; margin-right: 10px; font-size: 18px;" />

                  </div>

              </div>


          </div>
        </div>

        <div class="table-responsive especs">
          <h3 style="font-size: x-large;">Especificações do produto</h3>
          <table class="table table-bordered">

            <tbody>

                <tr>
                <th scope="row">Ref. Interna</th>
                <td><asp:Label ID="lbl_cod" runat="server" Text=""></asp:Label></td>
              </tr>

                <asp:Repeater ID="rpt_caract" runat="server">
                    <ItemTemplate>

                        <tr>
                            <th scope="row"><%# ((KeyValuePair<string, string>)Container.DataItem).Key %></th>
                            <td><%# ((KeyValuePair<string, string>)Container.DataItem).Value %></td>
                        </tr>

                    </ItemTemplate>
                </asp:Repeater>

            </tbody>
          </table>
        </div>

        <p class="title">Confira também!</p>

        <div class="separator"></div>

          <div class="produtosRelacionados">


              <asp:Repeater ID="rpt_relacionados" runat="server">
                  <ItemTemplate>

                      <div class="container">
                          <div class="card">
                              <div class="card-head">
                                  <a href="productPage.aspx?product='<%# Eval("CodProduto") %>'">
                                      <asp:Image class="product-img" ID="Image1" runat="server" ImageUrl='<%# Eval("ImgPrincipal") != null ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("ImgPrincipal")) : "/img/default.png" %>' />
                                  </a>
                              </div>
                              <div class="card-body">
                                  <div class="product-desc" style="margin-bottom: 33px;">
                                      <span class="product-title">
                                          <a href="productPage.aspx?product='<%# Eval("CodProduto") %>'"><%# Eval("NomeProduto") %></a>
                                      </span>

                                  </div>
                                  <% if (TechTudo.globalClass.type_user == "Revendedor")
                                      { %>
                                            <span class="product-price-promot"><b><%# ((decimal)Eval("Preco") * (1 - 0.20m)).ToString("0.00") %></b>€</span>
                                            <span class="product-price-old"><b><%# Eval("Preco") %></b>€</span>
                                  <% }
                                      else
                                      { %>
                                            <span class="product-price"><b><%# Eval("Preco") %></b>€</span>
                                   <% } %>
                              </div>
                          </div>
                      </div>

                  </ItemTemplate>
              </asp:Repeater>


          </div>
      </div>
    </div>
  </main>

      <script>
          document.querySelectorAll(".carousel").forEach(element => {

              const items = element.querySelectorAll(".carousel-item");

              // we generate the html for the navigation. By passing item through buttons it will generate as many buttons as items in the array

              const buttonsHtml = Array.from(items, () => {

                  return `<span class="carousel-button"></span>`;
                  // we are using backquotes `` so we can use double quotes"" inside the string for the class name
              });

              // now we insert the buttons
              element.insertAdjacentHTML('beforeend', `<div class="carousel-nav"> 
                ${buttonsHtml.join("")}
                </div>`);


              //   we are going to select the buttons

              const buttons = element.querySelectorAll(".carousel-button");
              //here we are getting the list of all the buttons we've created in the previous step


              //   we are iterating through each button and through each index (i) of the array
              buttons.forEach((button, i) => {
                  button.addEventListener("click", () => {
                      // un-select all the items. we select the item list we created at the beggining
                      // I don't call the class with "".class" because i'm telling it already the object is going to be a class.
                      items.forEach(item => item.classList.remove("item-selected"));
                      buttons.forEach(button => button.classList.remove("button-selected"));

                      //now we select the correct one depending on its index
                      items[i].classList.add("item-selected");
                      button.classList.add("button-selected");
                  });
              });

              //   this stablishes the first state of the carousel. Selects the first item on page load.
              items[0].classList.add("item-selected");
              buttons[0].classList.add("button-selected");
          });



         
      </script>


</asp:Content>
