<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="recuperarPw.aspx.cs" Inherits="TechTudo.recuperarPw" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/public.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/6fac992168.js" crossorigin="anonymous"></script>
</head>
<body>
    <form id="form1" runat="server">


        <% if (string.IsNullOrEmpty(Request.QueryString["email"]))
            {  %>

        <div class="wrapper">
            <div action="#" class="card-content">
                <div class="container">
                    <div class="image">
                        <i class="fa-solid fa-exclamation"></i>
                    </div>
                    <h1>Recuperação de Palavra-Passe</h1>
                </div>
                <div class="form-input">
                    <label for="email"></label>
                    <asp:TextBox ID="tb_email" class="subscribe-tb" type="email" placeholder="Your Email" runat="server"></asp:TextBox>
                    <asp:Button ID="btn_active" class="subscribe-btn" runat="server" Text="Avançar" OnClick="btn_active_Click" />
                </div>
                <asp:Label ID="lbl_Error" runat="server" Text="" CssClass="custom-validation-summary" ForeColor="Red"></asp:Label>
            </div>
        </div>

        <% }
            else
            {  %>

        <div class="wrapper">
            <div action="#" class="card-content">
                <div class="container">
                    <div class="image">
                        <i class="fa-solid fa-exclamation"></i>
                    </div>
                    <h1>Recuperação de Palavra-Passe</h1>
                </div>
                <div class="form-input">
                    <label for="email"></label>
                    <asp:TextBox ID="tb_newPass" ValidationGroup="valitators" class="subscribe-tb" type="password" placeholder="Nova Palavra-Passe" runat="server"></asp:TextBox>
                    <div>
                        <asp:CustomValidator ID="ValidatePassword" ValidationGroup="valitators" class="validator" runat="server" ErrorMessage="Password Fraca" ControlToValidate="tb_newPass" ForeColor="#FF3300" OnServerValidate="ValidatePassword_ServerValidate" Font-Bold="True" Text="^">^</asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="valitators" runat="server" ErrorMessage="Password Requerida" ControlToValidate="tb_newPass" Font-Bold="True" ForeColor="Red">^</asp:RequiredFieldValidator>
                    </div>

                    <asp:TextBox ID="tb_newPass2" ValidationGroup="valitators" class="subscribe-tb" type="password" placeholder="Confirme a Palavra-Passe" runat="server"></asp:TextBox>
                    <div>
                        <asp:CompareValidator ID="ComparePass" ValidationGroup="valitators" runat="server" ErrorMessage="Confirmação Invalida" ControlToCompare="tb_newPass" ControlToValidate="tb_newPass2" Font-Bold="True" ForeColor="Red">^</asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="valitators" runat="server" ErrorMessage="Password Requerida" ControlToValidate="tb_newPass2" Font-Bold="True" ForeColor="Red">^</asp:RequiredFieldValidator>
                    </div>

                    <asp:Button ID="btn_guardar" ValidationGroup="valitators" class="subscribe-btn" runat="server" Text="Guardar" OnClick="btn_guardar_Click" />

                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="valitators" runat="server" ForeColor="#FF3300" Font-Bold="False" CssClass="custom-validation-summary" DisplayMode="List" />
                </div>
            </div>
        </div>

        <% } %>
    </form>
</body>
</html>
