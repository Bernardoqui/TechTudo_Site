<%@ Page Title="" Language="C#" MasterPageFile="~/AdminHeader.Master" AutoEventWireup="true" CodeBehind="adminProducts.aspx.cs" Inherits="TechTudo.adminProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <section class="add-products">
        <div action="" method="POST" enctype="multipart/form-data">

            <h3>adicionar novo produto</h3>

            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdatePanel ID="UpdateAdicionar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h2>Características</h2>
                    <br />

                    <asp:DropDownList ID="ddl_categoria" runat="server" class="box" DataSourceID="SqlDataSource1" DataTextField="categoria" DataValueField="cod_categoria" OnSelectedIndexChanged="ddl_categoria_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Selected="True">---</asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:TechTudoConnectionString %>' ProviderName='<%$ ConnectionStrings:TechTudoConnectionString.ProviderName %>' SelectCommand="SELECT * FROM [categorias_produtos]"></asp:SqlDataSource>


                    <asp:TextBox ID="tb_nome" class="box" runat="server" placeholder="Nome de Produto"></asp:TextBox>

                    <asp:Repeater ID="rpt_caracteristicas" runat="server">

                        <ItemTemplate>

                            <asp:TextBox ID="tb_caract" runat="server" type="text" class="box" placeholder=" <%# Container.DataItem.ToString() %>"></asp:TextBox>

                        </ItemTemplate>

                    </asp:Repeater>

                    <asp:TextBox ID="tb_stock" class="box" runat="server" placeholder="Nº Stock" TextMode="Number"></asp:TextBox>

                    <asp:TextBox ID="tb_preco" class="box" runat="server" placeholder="Preço"></asp:TextBox>



                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddl_categoria" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>

            <div>
                <h2>Imagens</h2>
                <br />
                <div class="div_imgs">

                    <section class="imgs_cont_principal">
                        <h2>Principal</h2>

                        <% if (string.IsNullOrEmpty(img_principal.ImageUrl))
                            { %>
                        <label class="file-principal">
                            <span>+</span>
                            <asp:FileUpload ID="fileUpload_principal" onchange="minhaFuncao_imgPrincipal()" runat="server" AllowMultiple="false" CssClass="fileupload p0" />
                            <asp:Button ID="btn_imgPrincipal" runat="server" Text="Enviar" OnClick="btn_imgPrincipal_Click" />
                        </label>
                        <% }
                        else
                        { %>
                        <asp:ImageButton ID="img_principal" runat="server" CssClass="img_principal" OnClick="img_principal_Click"/>
                        <% } %>
                    </section>

                    <section class="imgs_cont">
                        <h2>Secundarias:     </h2>
                        <br />
                        <asp:Repeater ID="repeaterFiles" runat="server">
                            <ItemTemplate>

                                <asp:Image ID="imgItem" class="img" runat="server" ImageUrl='<%# "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Container.DataItem) %>' />

                            </ItemTemplate>
                        </asp:Repeater>

                        <label class="file-upload">
                            <span>+</span>
                            <asp:FileUpload ID="fileUpload" onchange="minhaFuncao()" CommandName="fileUpload" runat="server" AllowMultiple="true" placeholder="Current Work, Title" CssClass="fileupload p0" />
                            <asp:Button ID="btnSubmit" runat="server" Text="Enviar" OnClick="btnSubmit_Click" />
                        </label>
                    </section>

                </div>
            </div>


            <asp:Button ID="btn_adicionar" class="btn_user" runat="server" Text="Adicionar" OnClick="btn_adicionar_Click" />
        </div>
    </section>


    <section class="show-products">

        <h3>editar produto</h3>

        <div class="box-container">

            <div class="Card">
                <div class="CardInner">
                    <asp:Label ID="lbl_error_search" class="card_label" runat="server" Text="Procurar Produto"></asp:Label>
                    <div class="container">
                        <asp:Button class="btn_icon" ID="btn_search" runat="server" Text="Procurar" OnClick="btn_search_Click" />
                        <div class="InputContainer">
                            <asp:TextBox ID="tb_idProduto" type="number" runat="server" placeholder="por Id"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>


            <div class="box">
                <h2>Imagens</h2>
                <br />
                <div class="boxes">
                    <label class="input">
                        <asp:TextBox ID="tb_nome_update" class="input__field" runat="server"></asp:TextBox>
                        <span class="input__label">Nome do Produto</span>
                    </label>
                    <label class="input">
                        <asp:TextBox ID="tb_price_update" class="input__field" runat="server"></asp:TextBox>
                        <span class="input__label">Preço</span>
                    </label>
                    <label class="input">
                        <asp:TextBox ID="tb_stock_update" class="input__field" runat="server"></asp:TextBox>
                        <span class="input__label">Nº Stock</span>
                    </label>


                    <asp:Repeater ID="rpt_update" runat="server">

                        <ItemTemplate>

                            <label class="input">
                                <asp:TextBox ID="tb_caract_update" class="input__field" runat="server" type="text" Text="<%# ((KeyValuePair<string, string>)Container.DataItem).Value %>"></asp:TextBox>
                                <span class="input__label"><%# ((KeyValuePair<string, string>)Container.DataItem).Key %></span>
                            </label>


                        </ItemTemplate>

                    </asp:Repeater>

                </div>
                <br />
                <br />
                <div>
                    <h2>Imagens</h2>
                    <br />

                    <div class="div_imgs">

                        <section class="imgs_cont_principal">
                            <h2>Principal</h2>

                            <% if (string.IsNullOrEmpty(img_principal_update.ImageUrl)){ %>
                            <label class="file-principal">
                                <span>+</span>
                                <asp:FileUpload ID="fileUpload_principalUpdate" onchange="minhaFuncao_imgPrincipal_update()" runat="server" AllowMultiple="false" placeholder="" CssClass="fileupload p0" />
                                <asp:Button ID="btn_imgPrincipal_update" runat="server" Text="Enviar" OnClick="btn_imgPrincipal_update_Click"/>
                            </label>
                            <% }else{ %>
                                <asp:ImageButton ID="img_principal_update" runat="server" CssClass="img_principal" OnClick="img_principal_update_Click" />
                            <% } %>
                        </section>

                        <section class="imgs_cont">
                            <h2>Secundarias:     </h2>
                            <br />
                            <asp:Repeater ID="rpt_imgs_update" runat="server">
                                <ItemTemplate>

                                    <asp:Image ID="imgItem_update" class="img" runat="server" ImageUrl='<%# "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Container.DataItem) %>' />

                                </ItemTemplate>
                            </asp:Repeater>

                            <label class="file-upload">
                                <span>+</span>
                                <asp:FileUpload ID="fileUpload_update" runat="server" onchange="minhaFuncao_update()" AllowMultiple="True" CssClass="fileupload p0" />
                                <asp:Button ID="btn_imgs_update" runat="server" Text="Enviar" OnClick="btn_imgs_update_Click" />
                            </label>
                        </section>

                    </div>

                </div>

                <div class="buttons">
                    <asp:Button ID="btn_update" class="option-btn" runat="server" Text="Update" OnClick="btn_update_Click" />

                    <div class="select-cont">
                        <asp:DropDownList ID="ddl_ativacao" class="select" runat="server" onchange="alterarCorDropDownList(this);">
                            <asp:ListItem Value="True" CssClass="list-item">Ativo</asp:ListItem>
                            <asp:ListItem Value="False" CssClass="list-item">Desativo</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>

            </div>

        </div>


    </section>

    <script src="/js/admin_script.js"></script>
    <script>
        // Obtém referências para o botão e o ícone
        const aspButton = document.getElementById('aspButton');
        const icon = document.getElementById('icon');

        // Adiciona um ouvinte de evento de clique ao ícone
        icon.addEventListener('click', function () {
            // Aciona o clique do botão
            aspButton.click();
        });
    </script>

    <script type="text/javascript">
        function displayImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    var imagePreview = document.getElementById("imagePreview");
                    var preview = document.getElementById("preview");

                    imagePreview.style.display = "block";
                    preview.src = e.target.result;
                };

                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>

    <script type="text/javascript">
        function minhaFuncao_imgPrincipal() {
            // Verifica se um arquivo foi selecionado no controle FileUpload
            var fileUploadControl = document.getElementById('<%= fileUpload_principal.ClientID %>');

            if (fileUploadControl.files.length > 0) {

                var botao = document.getElementById('<%= btn_imgPrincipal.ClientID %>');

                botao.click();
            } else {
                // Nenhum arquivo foi selecionado, você pode exibir uma mensagem de erro, se necessário
                alert("Nenhum arquivo foi selecionado.");
            }
        }

        function minhaFuncao() {
            // Verifica se um arquivo foi selecionado no controle FileUpload
            var fileUploadControl = document.getElementById('<%= fileUpload.ClientID %>');

            if (fileUploadControl.files.length > 0) {

                var botao = document.getElementById('<%= btnSubmit.ClientID %>');

                botao.click();
            } else {
                // Nenhum arquivo foi selecionado, você pode exibir uma mensagem de erro, se necessário
                alert("Nenhum arquivo foi selecionado.");
            }
        }

        function minhaFuncao_update() {
            // Verifica se um arquivo foi selecionado no controle FileUpload
            var fileUploadControl = document.getElementById('<%= fileUpload_update.ClientID %>');

            if (fileUploadControl.files.length > 0) {

                var botao = document.getElementById('<%= btn_imgs_update.ClientID %>');

                botao.click();
            } else {
                // Nenhum arquivo foi selecionado, você pode exibir uma mensagem de erro, se necessário
                alert("Nenhum arquivo foi selecionado.");
            }
        }

        function minhaFuncao_imgPrincipal_update() {
            // Verifica se um arquivo foi selecionado no controle FileUpload
            var fileUploadControl = document.getElementById('<%= fileUpload_principalUpdate.ClientID %>');

            if (fileUploadControl.files.length > 0) {

                var botao = document.getElementById('<%= btn_imgPrincipal_update.ClientID %>');

                botao.click();
            } else {
                // Nenhum arquivo foi selecionado, você pode exibir uma mensagem de erro, se necessário
                alert("Nenhum arquivo foi selecionado.");
            }
        }

        function alterarCorDropDownList(dropDownList) {
            var selectedValue = dropDownList.value;
            var dropdownItems = dropDownList.getElementsByTagName("option");

            for (var i = 0; i < dropdownItems.length; i++) {
                dropdownItems[i].style.backgroundColor = "";
                dropdownItems[i].style.color = "";
            }

            if (selectedValue === "True") {
                dropDownList.style.backgroundColor = "#05a418"; 
            } else if (selectedValue === "False") {
                dropDownList.style.backgroundColor = "#bc392b"; 
            }
        }

    </script>

</asp:Content>
