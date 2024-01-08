<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sighIn.aspx.cs" Inherits="TechTudo.sighIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/public.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/6fac992168.js" crossorigin="anonymous"></script>
</head>
<body class="createAccount-body">
    <form id="form1" runat="server">

        <h1 class="backHome"><a href="homePage.aspx">&leftarrow; Voltar</a></h1>
        <div class="container" id="container">

        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                
                    <div class="form-container sign-up-container">
                        <div class="form">
                            <h1>Login</h1>
                            <br />
                            <%--<div class="social-container">
                                <a href="#" class="social"><i class="fab fa-google-plus-g"></i></a>
                            </div>--%>
                            <%--<span>ou use a sua conta</span>--%>

                            <asp:TextBox ID="tb_email_Login" class="textBox_sign" runat="server" placeholder="Email" type="email"></asp:TextBox>

                            <asp:TextBox ID="tb_pass_Login" class="textBox_sign" runat="server" type="password" placeholder="Password"></asp:TextBox>

                            <a href="recuperarPW.aspx">Esqueceu a sua Palavra-Passe?</a>

                            <asp:Button ID="btn_login" class="btn_sign" runat="server" Text="Login" OnClick="btn_login_Click" />
                            
                            <asp:Label ID="lbl_loginError" runat="server" Text="" CssClass="custom-validation-summary" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btn_login" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>


        <div class="form-container sign-in-container">
            <div class="form">
                <h1>Criar Conta</h1>
                <br />
                <%--<div class="social-container">
                    <a href="#" class="social"><i class="fab fa-google-plus-g"></i></a>
                </div>--%>
                <%--<span>ou use o seu email</span>--%>
                <asp:TextBox ID="tb_email_Regist" class="textBox_sign" runat="server" placeholder="Email" type="email"></asp:TextBox>
                <div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="login_valitators" runat="server" ErrorMessage="Email Invalido" ControlToValidate="tb_email_Regist" Font-Bold="True" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">^</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="login_valitators" runat="server" ErrorMessage="Email Requerido" Font-Bold="True" ForeColor="Red" ControlToValidate="tb_email_Regist">^</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="emailExistente" ValidationGroup="login_valitators" runat="server" ErrorMessage="Email já Registado" Font-Bold="True" ForeColor="Red">^</asp:CustomValidator>
                </div>
                <asp:TextBox ID="tb_pass_Regist" class="textBox_sign" type="password" runat="server" placeholder="Password"></asp:TextBox>
                <div>
                    <asp:CustomValidator ID="ValidatePassword" ValidationGroup="login_valitators" class="validator" runat="server" ErrorMessage="Password Fraca" ControlToValidate="tb_pass_Regist" ForeColor="#FF3300" OnServerValidate="ValidatePassword_ServerValidate" Font-Bold="True" Text="^">^</asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="login_valitators" runat="server" ErrorMessage="Password Requerida" ControlToValidate="tb_pass_Regist" Font-Bold="True" ForeColor="Red">^</asp:RequiredFieldValidator>
                </div>
                <asp:TextBox ID="tb_passConfirm" class="textBox_sign" type="password" runat="server" placeholder="Confirme Password"></asp:TextBox>
                <div>
                    <asp:CompareValidator ID="ComparePass" ValidationGroup="login_valitators" runat="server" ErrorMessage="Confirmação Invalida" ControlToCompare="tb_passConfirm" ControlToValidate="tb_pass_Regist" Font-Bold="True" ForeColor="Red">^</asp:CompareValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="login_valitators" runat="server" ErrorMessage="Confirme Password" ControlToValidate="tb_passConfirm" Font-Bold="True" ForeColor="Red">^</asp:RequiredFieldValidator>
                </div>


                <asp:Button ID="btn_createAccount" ValidationGroup="login_valitators" class="btn_sign" runat="server" Text="Criar Conta" OnClick="btn_createAccount_Click" />

                <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="login_valitators" runat="server" ForeColor="#FF3300" Font-Bold="False" CssClass="custom-validation-summary" DisplayMode="List" />
                <asp:Label ID="lbl_aut" runat="server" Text="" CssClass="custom-validation-summary" ForeColor="Red"></asp:Label>
            </div>
        </div>

        <div class="overlay-container">
            <div class="overlay">
                <div class="overlay-panel overlay-left">
                    <img src="/img/logo1.png" />
                    <h1>Seja Bem Vindo!</h1>
                    <p>Insira seus dados pessoais e comece sua jornada conosco</p>

                    <button class="btn_sign ghost" type="button" id="signIn">Sign In</button>
                </div>
                <div class="overlay-panel overlay-right">
                    <img src="/img/logo1.png" />
                    <h1>Bem Vindo De Volta!</h1>
                    <p>Para se manter conectado conosco, faça login com suas informações pessoais</p>

                    <button class="btn_sign ghost" type="button" id="signUp">Sign Up</button>
                </div>
            </div>
        </div>
        </div>

        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
        <script>
            const signUpButton = document.getElementById('signUp');
            const signInButton = document.getElementById('signIn');
            const container = document.getElementById('container');

            signUpButton.addEventListener('click', () => {
                container.classList.add("right-panel-active");
            });

            signInButton.addEventListener('click', () => {
                container.classList.remove("right-panel-active");
            });
        </script>
    </form>
</body>
</html>
