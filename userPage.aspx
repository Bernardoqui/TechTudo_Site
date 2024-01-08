<%@ Page Title="" Language="C#" MasterPageFile="~/PagesTemplate.Master" AutoEventWireup="true" CodeBehind="userPage.aspx.cs" Inherits="TechTudo.userPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" integrity="...">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
        <div class="bg-dark text-white ">
    <div class="perfil">

        <div class="container-lg rounded mt-5 mb-5">

            <div class="mx-auto text-center w-full">
                <div class="p-3 py-5">
                    <div class="d-flex">
                        <div class="m-5">
                            <div class="d-flex align-items-center mb-3">
                                <i class="fa-solid fa-user"></i>
                                <h3 class="text-right">Informações do Utilizador</h3>
                            </div>
                            <div class="separator3"></div>
                            <div class="row mt-2">

                                <label class="label">Email</label>
                                <asp:Label ID="tb_email" runat="server" class="label-email" Text=""></asp:Label>

                                <div class="col-md-12">
                                    <label class="labels">Nome</label>
                                    <%--<input type="text" class="form-control txt_user" placeholder="Insira o seu Nome" value="">--%>
                                    <asp:TextBox ID="tb_nome" runat="server" class="form-control txt_user" placeholder="Nome"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="infoUser" Text="^" ForeColor="Red" ControlToValidate="tb_nome"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    <label class="labels">Telemovel</label>
                                    <%--<input type="text"class="form-control txt_user" placeholder="Insira nº de telemovel"value="">--%>
                                    <asp:TextBox ID="tb_contacto" runat="server" class="form-control txt_user" placeholder="Telemovel"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Text="^" ForeColor="Red" ControlToValidate="tb_contacto" ValidationGroup="infoUser"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    <label class="labels">NIF</label>
                                    <%--<input type="text" class="form-control txt_user" placeholder="insira o seu NIF" value="">--%>
                                    <asp:TextBox ID="tb_nif" runat="server" class="form-control txt_user" placeholder="Nif"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Text="^" ForeColor="Red" ControlToValidate="tb_nif" ValidationGroup="infoUser"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                        <div class="m-5">

                            <div class="d-flex align-items-center mb-3">
                                <i class="fa-solid fa-truck"></i>
                                <h3 class="text-right">Informações de Entraga</h3>
                            </div>
                            <div class="separator3"></div>
                            <div class="row mt-2">
                                <div class="col-md-12"><label class="labels">Morada</label>
                                    <%--<input type="text" class="form-control txt_user" placeholder="Insira a morada" value="">--%>
                                    <asp:TextBox ID="tb_morada" runat="server" class="form-control txt_user" placeholder="Morada"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Text="^" ForeColor="Red" ControlToValidate="tb_morada" ValidationGroup="infoUser"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6"><label class="labels">Código Postal</label>
                                    <%--<input type="text" class="form-control txt_user" placeholder="Insira o Código Postal" value="">--%>
                                    <asp:TextBox ID="tb_postal" runat="server" class="form-control txt_user" placeholder="Código Postal"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Text="^" ForeColor="Red" ControlToValidate="tb_postal" ValidationGroup="infoUser"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    <label class="labels">Localidade</label>
                                    <%--<input type="text" class="form-control txt_user" placeholder="Insira a sua localidade" value="">--%>
                                    <asp:TextBox ID="tb_cidade" runat="server" class="form-control txt_user" placeholder="Localidade"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Text="^" ForeColor="Red" ControlToValidate="tb_cidade" ValidationGroup="infoUser"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    <label class="labels">Andar/Porta</label>
                                    <%--<input type="text" class="form-control txt_user" placeholder="Insira o Andar" value="">--%>
                                    <asp:TextBox ID="tb_porta" runat="server" class="form-control txt_user" placeholder="Andar / Porta"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" Text="^" ForeColor="Red" ControlToValidate="tb_porta" ValidationGroup="infoUser"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="mt-5 text-center">
                        <asp:Button ID="btn_guardarInfo" class="btn btn-primary profile-button m-4" runat="server" Text="Salvar Configurações" OnClick="btn_guardarInfo_Click" ValidationGroup="infoUser"/>
                    </div>

                    <div class="separator3"></div>

                    <div class="cont-pass">

                        <asp:ScriptManager ID="ScriptManager1" runat="server" />
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="row mt-2 child-cont-pass" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>

                                <div class="d-flex align-items-center mb-3">
                                    <i class="fa-solid fa-key"></i>
                                    <h3 class="text-right">Alterar a Palavra Passe</h3>
                                </div>
                                <div class="separator3"></div>
                                <div class="col-md-12">
                                    <label class="labels">Palavra Passe atual</label>
                                    <%-- <input type="text" class="form-control txt_user" placeholder="Palavra Passe atual" value="">--%>
                                    <asp:TextBox ID="tb_oldPass" type="password" class="form-control txt_user" ValidationGroup="valitators" placeholder="Palavra Passe atual" runat="server"></asp:TextBox>
                                    <div class="validadores">
                                        <asp:CustomValidator ID="cv_existPass" runat="server" ErrorMessage="Password errada" ForeColor="Red" Text="^"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="valitators" runat="server" Text="^" ErrorMessage="Password Invalida" ForeColor="Red" ControlToValidate="tb_oldPass"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <label class="labels">Palavra Passe nova</label>
                                    <%-- <input type="text" class="form-control txt_user" value="" placeholder="Palavra Passe nova">--%>

                                    <asp:TextBox ID="tb_newPass" ValidationGroup="valitators" class="form-control txt_user" type="password" placeholder="Nova Palavra-Passe" runat="server"></asp:TextBox>
                                    <div class="validadores">
                                        <asp:CustomValidator ID="ValidatePassword" ValidationGroup="valitators" class="validator" runat="server" ErrorMessage="Password Fraca" ControlToValidate="tb_newPass" ForeColor="#FF3300" OnServerValidate="ValidatePassword_ServerValidate" Font-Bold="True" Text="^">^</asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="valitators" runat="server" ErrorMessage="Password Nova Requerida" ControlToValidate="tb_newPass" Font-Bold="True" ForeColor="Red">^</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <label class="labels">Confirmar Palavra Passe nova</label>
                                    <%--<input type="text" class="form-control txt_user" placeholder="Confirmar Palavra Passe nova" value="">--%>
                                    <asp:TextBox ID="tb_newPass2" ValidationGroup="valitators" class="form-control txt_user" type="password" placeholder="Confirme a Palavra-Passe" runat="server"></asp:TextBox>
                                    <div class="validadores">
                                        <asp:CompareValidator ID="ComparePass" ValidationGroup="valitators" runat="server" ErrorMessage="Confirmação Invalida" ControlToCompare="tb_newPass" ControlToValidate="tb_newPass2" Font-Bold="True" ForeColor="Red">^</asp:CompareValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="valitators" runat="server" ErrorMessage="Confirmação Requerida" ControlToValidate="tb_newPass2" Font-Bold="True" ForeColor="Red">^</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="mt-5 text-center">
                                    <%--<button class="btn btn-primary profile-button m-4" type="button">Mudar Palavra-passe</button>--%>
                                    <asp:Button ID="btn_guardar" ValidationGroup="valitators" class="btn btn-primary profile-button m-4" runat="server" Text="Guardar" OnClick="btn_guardar_Click1" UseSubmitBehavior="false"/>
                                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="valitators" runat="server" ForeColor="#FF3300" Font-Bold="False" CssClass="custom-validation-summary" DisplayMode="List" />
                                </div>

                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btn_guardar" />
                            </Triggers>

                        </asp:UpdatePanel>


                        <asp:Button ID="btn_logout" CssClass="btn_logout" runat="server" Text="Terminar Sessão" OnClick="btn_logout_Click"/>

                    </div>


                    <br>

                    <div class="d-flex align-items-center mb-3">
                        <i class="fa-solid fa-bag-shopping"></i>
                        <h3 class="text-right">Pedidos</h3>
                    </div>
                    <div class="separator3"></div>

                    <div class="orders">
                        <asp:Repeater ID="rptPedidos" runat="server" OnItemDataBound="rptPedidos_ItemDataBound">
                            <ItemTemplate>
                                <div class="dropdown" onclick="toggleDropdown(this)">

                                    <span class="panel_info">
                                        <p>
                                            Morada :
                                            <br>
                                            <label><%# Eval("Morada") %></label>
                                        </p>
                                        <p>
                                            Metodo de Pagamento :
                                            <br>
                                            <label><%# Eval("Metodo") %></label>
                                        </p>
                                        <p>
                                            Preço Total :
                                            <br>
                                            <label><%# Eval("PrecoFinal") %></label>
                                        </p>
                                        <p>
                                            Quatidade :<br>
                                            <label><%# Eval("QuantidadeTotal") %></label>
                                        </p>
                                        <p>
                                            Data :<br>
                                            <%# Eval("DataPedido") %>
                                        </p>
                                        <p>
                                            Status :<br>
                                            <%# Eval("EstadoPedido") %>
                                        </p>
                                    </span>


                                    <ul class="slide dropdown-content">

                                        <asp:Repeater ID="rptItens" runat="server">
                                            <ItemTemplate>

                                                <li>

                                                    <asp:Image class="" ID="Image1" runat="server" ImageUrl='<%# Eval("imgProduto") != null ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("imgProduto")) : "/img/default.png" %>' />
                                                    <p>
                                                        Id Produto :
                                                        <br>
                                                        <label><%# Eval("CodProduto") %></label>
                                                    </p>
                                                    <p>
                                                        Nome Produto :
                                                        <br>
                                                        <label><%# Eval("NomeProduto") %></label>
                                                    </p>
                                                    <p>
                                                        Categoria :
                                                        <br>
                                                        <label><%# Eval("Categoria") %></label>
                                                    </p>
                                                    <p>
                                                        Quantidade :
                                                        <br>
                                                        <label><%# Eval("QuantidadeItem") %></label>
                                                    </p>
                                                    <p>
                                                        Preço :
                                                        <br>
                                                        <label><%# Eval("PrecoUnitario") %></label>
                                                    </p>

                                                </li>

                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>


                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>

            </div>
        </div>
    </div>
        </div>

    <div class="banners-container">
        <div class="banners">
            <div class="banner error">
                <div class="banner-icon"><i data-eva="alert-circle-outline" data-eva-fill="#ffffff" data-eva-height="48" data-eva-width="48"></i></div>
                <div class="banner-message">Oops! Something went wrong!</div>
                <div class="banner-close" onclick="hideBanners()"><i data-eva="close-outline" data-eva-fill="#ffffff"></i></div>
            </div>
            <div class="banner success">
                <div class="banner-icon"><i data-eva="checkmark-circle-outline" data-eva-fill="#ffffff" data-eva-height="48" data-eva-width="48"></i></div>
                <div class="banner-message">Sucesso!</div>
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


    <script src="/js/script.js"></script>
    <script>
        function toggleDropdown(dropdown) {
            dropdown.classList.toggle('active');
        }
  
        document.addEventListener('click', function (e) {
           const dropdowns = document.querySelectorAll('.dropdown');
           dropdowns.forEach(function (dropdown) {
              if (!dropdown.contains(e.target)) {
                 dropdown.classList.remove('active');
              }
           });
        });
    </script>

</asp:Content>
