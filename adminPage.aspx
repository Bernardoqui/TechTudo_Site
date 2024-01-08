<%@ Page Title="" Language="C#" MasterPageFile="~/AdminHeader.Master" AutoEventWireup="true" CodeBehind="adminPage.aspx.cs" Inherits="TechTudo.adminPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    


    <section class="dashboard">

   <h1 class="title">dashboard</h1>

   <div class="box-container">

      <div class="box">

         <h3><asp:Label ID="lbl_valorPendente" runat="server" Text="Label"></asp:Label></h3>
         <p>Total Pendentes</p>
      </div>

      <div class="box">

         <h3><asp:Label ID="lbl_valorCompletos" runat="server" Text="Label"></asp:Label></h3>
         <p>Total Completos</p>
      </div>

      <div class="box">
         <h3>
             <asp:Label ID="lbl_pedidos" runat="server" Text="Label"></asp:Label></h3>
         <p>Pedidos Feitos</p>
      </div>

      <div class="box">
         <h3>
             <asp:Label ID="lbl_produtos" runat="server" Text="Label"></asp:Label></h3>
         <p>Produtos Ativos</p>
      </div>

      <div class="box">
         <h3>
             <asp:Label ID="lbl_users" runat="server" Text="Label"></asp:Label></h3>
         <p>Normal Users</p>
      </div>

       <div class="box">
           <h3>
               <asp:Label ID="lbl_revendedores" runat="server" Text="Label"></asp:Label></h3>
           <p>Revendedor Users</p>
       </div>

      <div class="box">
         <h3>
             <asp:Label ID="lbl_admins" runat="server" Text="Label"></asp:Label></h3>
         <p>Admin Users</p>
      </div>

      <div class="box">
         <h3>
             <asp:Label ID="lbl_contas" runat="server" Text="Label"></asp:Label></h3>
         <p>Total Contas</p>
      </div>

   </div>

   <div class="main_admin">

      <div>
          <div class="user newUser">
    <div class="title-case">
        <h1 class="title">Nova Conta</h1>
    </div>

    <div class="funtions-case">
        <div class="user-section">
            <h1>Nome</h1>
            <asp:TextBox ID="tb_nome" class="txt_user" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="newUser_validator" runat="server" ErrorMessage="Nome Requerido" Font-Bold="True" ForeColor="Red" ControlToValidate="tb_nome" Font-Size="Large">^</asp:RequiredFieldValidator>
        </div>
        <div class="user-section">
            <h1>Email</h1>
            <asp:TextBox ID="tb_email" class="txt_user" runat="server"></asp:TextBox>
            <div style="display: flex">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="newUser_validator" runat="server" ErrorMessage="Email Invalido" ControlToValidate="tb_email" Font-Bold="True" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Font-Size="Large">^</asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="newUser_validator" runat="server" ErrorMessage="Email Requerido" Font-Bold="True" ForeColor="Red" ControlToValidate="tb_email" Font-Size="Large">^</asp:RequiredFieldValidator>
                <asp:CustomValidator ID="emailExistente" ValidationGroup="newUser_validator" runat="server" ErrorMessage="Email já Registado" Font-Bold="True" ForeColor="Red" ControlToValidate="tb_email" Font-Size="Large">^</asp:CustomValidator>
            </div>

        </div>
        <div class="user-section">
            <h1>Password</h1>
            <div class="form-group">
                <asp:TextBox ID="tb_pass" class="form-field" type="password" runat="server"></asp:TextBox>
                <asp:Button ID="btn_gerarPass" class="btn-field" runat="server" Text="Gerar" OnClick="btn_gerarPass_Click"/>
            </div>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="newUser_validator" runat="server" ErrorMessage="Password Requerida" Font-Bold="True" ForeColor="Red" ControlToValidate="tb_pass" Font-Size="Large">^</asp:RequiredFieldValidator>
        </div>

        <div class="user-section">
            <h1>Tipo de User</h1>

            <div class="select-cont">
                <asp:DropDownList ID="ddl_typeUser" class="select" runat="server" DataSourceID="SqlDataSource1" DataTextField="type_user" DataValueField="type_user"></asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:TechTudoConnectionString %>' SelectCommand="SELECT DISTINCT  [type_user] FROM [Users]"></asp:SqlDataSource>
            </div>
            <h3 style="color: transparent">1</h3>
        </div>
        <div class="user-section">
            <asp:Button ID="btn_criarUser" ValidationGroup="newUser_validator" class="btn_user" runat="server" Text="Criar" OnClick="btn_criarUser_Click" />
        </div>
        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="newUser_validator" runat="server" CssClass="custom-validation-summary" DisplayMode="List" ForeColor="Red" />
    </div>
</div>
      </div>
               
        

       <div class="user newCoupon">
         <div class="title-case">
            <h1 class="title">Novo Cupão</h1>
         </div>

         <div class="funtions-case">
            <div class="user-section">
               <h1>Código</h1>
                <asp:TextBox ID="tb_codigo" class="txt_user" runat="server"></asp:TextBox>
            </div>
             <div class="user-section">
                 <h1>% de Desconto</h1>
                 <asp:TextBox ID="tb_desconto" class="txt_user" runat="server"></asp:TextBox>
             </div>
            <div class="user-section">
               <h1>Tipo de Produto</h1>
                <div class="select-cont">
                    <asp:DropDownList ID="ddl_productType" class="select" runat="server" DataSourceID="SqlDataSource2" DataTextField="categoria" DataValueField="cod_categoria"></asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:TechTudoConnectionString %>' SelectCommand="SELECT * FROM [categorias_produtos]"></asp:SqlDataSource>
                </div>
            </div>
             <div class="user-section">
                 <h1>Data de Começo</h1>
                 <asp:TextBox ID="tb_dataComeco" class="txt_user calendar" runat="server" TextMode="Date"></asp:TextBox>
             </div>
             <div class="user-section">
                 <h1>Data de Termino</h1>
                 <asp:TextBox ID="tb_dataTermino" class="txt_user calendar" runat="server" TextMode="Date"></asp:TextBox>
             </div>
            <div class="user-section">
                 <asp:Button ID="btn_cupao" class="btn_user" runat="server" Text="Criar" OnClick="btn_cupao_Click"/>
            </div>
         </div>

      </div>

   </div>

</section>

<script src="/js/admin_script.js"></script>

</asp:Content>

