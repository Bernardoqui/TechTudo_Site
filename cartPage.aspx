<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cartPage.aspx.cs" Inherits="TechTudo.cartPage" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>TechTudo</title>
    <link href="/css/public.css" rel="stylesheet"/>
    <script src="https://kit.fontawesome.com/6fac992168.js" crossorigin="anonymous"></script>
    <link href='https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' rel='stylesheet'/>
    <link href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css' rel='stylesheet'/>
    <script type='text/javascript' src='https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js'></script>
    <script type='text/javascript' src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js'></script>
</head>
<body>
    <form id="form1" runat="server">

        <a href="homePage.aspx"><img class="logo-cart"  src="img/logo1.png"/></a>


        <div class='snippet-body'>
            <div class="card">
                <div class="row display">
                    <div class="col-md-8 cart">
                        <div class="title">
                            <div class="row">
                                <div class="col">
                                    <h4><b>Carrinho</b></h4>
                                </div>
                            </div>
                        </div>


                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdateCart" runat="server" UpdateMode="Conditional" class="items_cart">
                            <ContentTemplate>
                                <asp:Repeater ID="rpt_products" runat="server">
                                    <ItemTemplate>
                                        <div class="row border-top">
                                            <div class="row main align-items-center">
                                                <div class="col-2">
                                                    <img class="img-fluid" src='<%# Eval("img") != null ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("img")) : "/img/default.png" %>'>
                                                </div>
                                                <div class="col">
                                                    <div class="row text-muted"><%# Eval("categoria") %></div>
                                                    <div class="row"><%# Eval("nome") %></div>
                                                </div>
                                                <div class="col">
                                                    <asp:Button ID="btn_sub" class="quant" runat="server" Text="-" OnCommand="editProduto"  CommandName="sub" CommandArgument='<%# Eval("cod") %>'/>
                                                    <asp:Label ID="lbl_quantidade" CssClass="border" runat="server" Text='<%# Eval("quantidade") %>'></asp:Label>
                                                    <asp:Button ID="btn_plus" class="quant" runat="server" Text="+" OnCommand="editProduto" CommandName="plus" CommandArgument='<%# Eval("cod") %>'/>
                                                </div>
                                                <div class="col">
                                                    &euro; <%# Eval("price") %>
                                                    <asp:Button ID="btn_eliminar" CssClass="btn_removeCart" runat="server" Text="&#10005;" OnCommand="editProduto" CommandName="Eliminar" CommandArgument='<%# Eval("cod") %>' />
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ContentTemplate>
                        </asp:UpdatePanel>



                        <div class="back-to-shop"><a href="homePage.aspx">&leftarrow;<span class="text-muted"> Voltar</span></a></div>
                    </div>
                <div class="col-md-4 summary">
                    <div>
                        <h5><b>Sumario</b></h5>
                    </div>
                    
                    <div class="row">
                        <div class="col text-right">TOTAL: <asp:Label ID="lbl_itemsTotal" runat="server" Text="Label"></asp:Label>  &euro;</div>
                    </div>
                    <br />
                    <div>
                        <br />
                        <br />
                        <div class="shipping">
                            <h1>
                                <i class="fas fa-shipping-fast"></i>
                                Detalhes de Entrega
                            </h1>
                            <br />
                            <div class="street">
                                <div>
                                    <label for="f-name">Nome</label>
                                    <asp:TextBox ID="tb_nome" runat="server" type="text" name="f-name"></asp:TextBox>
                                </div>
                            </div>
                            <div class="street">
                                <label for="name">Morada</label>
                                <asp:TextBox ID="tb_morada" runat="server" type="text" name="address"></asp:TextBox>
                            </div>
                            <div class="address-info">
                                <div>
                                    <label for="f-name">Porta/Andar</label>
                                    <asp:TextBox ID="tb_porta" runat="server" type="text" name="f-name"></asp:TextBox>
                                </div>
                                <div>
                                    <label for="city">Cidade</label>
                                    <asp:TextBox ID="tb_cidade" runat="server" type="text" name="city"></asp:TextBox>
                                </div>
                                <div>
                                    <label for="zip">Cod-Postal</label>
                                    <asp:TextBox ID="tb_postal" runat="server" type="text" name="zip"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="name">
                                <div>
                                    <label for="name">Contacto</label>
                                    <asp:TextBox ID="tb_contacto" runat="server" type="number" name="contacto"></asp:TextBox>
                                </div>
                                <div>
                                    <label for="name">Nif</label>
                                    <asp:TextBox ID="tb_nif" runat="server" type="number" name="nif"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <br />
                        
                        <% if (TechTudo.globalClass.type_user != "Revendedor")
                            {  %>
                        <p class="code_title">Cupão de Desconto</p>
                        <div class="form-group">
                            <asp:TextBox ID="tb_code" class="form-field" type="text" runat="server"></asp:TextBox>
                            <asp:Button ID="btn_aplicar" ValidationGroup="codeValidate" class="btn-field" runat="server" Text="Aplicar" OnClick="btn_aplicar_Click"/>
                        </div>
                        <asp:CustomValidator ID="cv_code" runat="server" ValidationGroup="codeValidate" ControlToValidate="tb_code" Font-Bold="True" ForeColor="Red"></asp:CustomValidator>
                        <% } %>

                        <fieldset>
                            <h1>
                                <i class="far fa-credit-card"></i> Metodo de Pagamento
                            </h1>
                            <br />
                                <div class="form__radios">
                                    <div class="form__radio">
                                        <label for="visa">
                                            <i class="fa-brands fa-cc-visa fa-2xl" style="color: #c11f1f;"></i>
                                            Visa Payment</label>
                                        <asp:RadioButton ID="rb_visa" runat="server" GroupName="payment" Checked="true" />
                                    </div>

                                    <div class="form__radio">
                                        <label for="paypal">
                                            <i class="fa-brands fa-paypal fa-2xl" style="color: #005eff;"></i>
                                            PayPal</label>
                                        <asp:RadioButton ID="rb_payapl" runat="server" name="payment-method" GroupName="payment" />
                                    </div>

                                    <div class="form__radio">
                                        <label for="mastercard">
                                            <i class="fa-brands fa-cc-mastercard fa-2xl" style="color: #d09c0b;"></i>
                                            Master Card</label>
                                        <asp:RadioButton ID="rb_mastercard" runat="server" name="payment-method" GroupName="payment" />
                                    </div>
                                </div>
                        </fieldset>

                    </div>
                    <br />
                    <br />
                    <div>
                        <asp:UpdatePanel ID="UpdatePrices" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <h2 class="prices_title">Detalhes</h2>
                                <br />
                                <table class="prices_table">
                                    <tbody class="prices">
                                        <tr>
                                            <td>Portes </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_portes" runat="server" Text="0"></asp:Label>&euro;</td>
                                        </tr>

                                        <% if (TechTudo.globalClass.desconto > 0)
                                            {  %>
                                        <tr>
                                            <td>Desconto </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_desconto" runat="server" Text="0"></asp:Label></td>
                                        </tr>
                                        <% } %>
                                        <tr>
                                            <td>Preço</td>
                                            <td align="right">
                                                <asp:Label ID="lbl_preco" runat="server" Text="0"></asp:Label>&euro;</td>
                                        </tr>
                                    </tbody>
                                </table>

                                <div class="row" style="border-top: 1px solid rgba(0,0,0,.1); padding: 2vh 0;">
                                    <div class="col">VALOR TOTAL</div>
                                    <div class="col text-right">
                                        <asp:Label ID="lbl_valorTotal" runat="server" Text="0"></asp:Label></div>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    <asp:Button ID="btn_checkout" class="btn" runat="server" Text="CHECKOUT" OnClick="btn_checkout_Click" />
                </div>
            </div>
        </div>

    </div>

        <script type='text/javascript'
            src='https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js'></script>
        <script type='text/javascript' src='#'></script>
        <script type='text/javascript' src='#'></script>
        <script type='text/javascript' src='#'></script>
        <script type='text/javascript' src='#'></script>
        <script type='text/javascript'>var myLink = document.querySelector('a[href="#"]');
            myLink.addEventListener('click', function (e) {
                e.preventDefault();
            });
        </script>
    </form>
</body>
</html>
