<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="active_account.aspx.cs" Inherits="TechTudo.active_account" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="/css/public.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/6fac992168.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.9.0/css/all.css" integrity="sha384-i1LQnF23gykqWXg6jxC2ZbCbUMxyw5gLZY6UiUS98LYV5unm8GWmfkIS6jqJfb4E" crossorigin="anonymous">
</head>
<body>
    <form id="form1" runat="server">

        
        <div class="wrapper">
            <div action="#" class="card-content">
                <div class="container">
                    <div class="image">
                        <i class="fa-solid fa-exclamation"></i>
                    </div>
                    <h1>Ativação</h1>
                    <p>Ativação da sua conta no nosso Site!</p>
                </div>
                <div class="form-input">
                    <label for="email"></label>
                    <asp:TextBox ID="tb_email" class="subscribe-tb" type="email" placeholder="Your Email" runat="server"></asp:TextBox>
                    <asp:Button ID="btn_active" class="subscribe-btn" runat="server" Text="Ativar" OnClick="btn_active_Click"/>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
