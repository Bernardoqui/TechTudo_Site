<%@ Page Title="" Language="C#" MasterPageFile="~/AdminHeader.Master" AutoEventWireup="true" CodeBehind="admin_Orders.aspx.cs" Inherits="TechTudo.admin_Orders" EnableEventValidation="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="orders">
     <%-- <div class="search_order">
         <button>Procurar</button>
         <input class="tb_search" placeholder="Procurar Venda">
      </div>--%>

      <div class="panel">



          <asp:Repeater ID="rptPedidos" runat="server" OnItemDataBound="rptPedidos_ItemDataBound">
                <ItemTemplate>
                    <div class="dropdown" onclick="toggleDropdown(this)">

                        <span class="panel_info">
                            <p> User id : <br> <label><%# Eval("CodUser") %></label></p>
                            <p> Nome : <br> <label><%# Eval("NomeUser") %></label></p>
                            <p> Contacto : <br><label><%# Eval("Contacto") %></label></p>
                            <p> Email :<br><label><%# Eval("Email") %></label></p>
                            <p> Morada : <br><label><%# Eval("Morada") %></label></p>
                            <p> Metodo de Pagamento : <br> <label><%# Eval("Metodo") %></label></p>
                            <p> Preço Total : <br><label><%# Eval("PrecoFinal") %></label></p>
                            <p> Quatidade :<br> <label><%# Eval("QuantidadeTotal") %></label></p>
                            <p> Data :<br><%# Eval("DataPedido") %></p>
                            <p>
                                Status:<br />
                                <asp:Label ID="lblEstadoPedido" runat="server" style="color: green; padding: 0;" Text='<%# Eval("EstadoPedido") %>'></asp:Label>
                                <asp:Button ID="btn_concluir" CssClass="option-btn" runat="server" Text="Concluir" Visible="false" OnCommand="concluir" CommandName="concl" CommandArgument='<%# Eval("IdPedido") %>'/>
                            </p>

                        </span>


                        <ul class="slide dropdown-content">

                            <asp:Repeater ID="rptItens" runat="server">
                                <ItemTemplate>
                                
                                       <li>

                                          <asp:Image class="product-img" ID="Image1" runat="server" ImageUrl='<%# Eval("imgProduto") != null ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("imgProduto")) : "/img/default.png" %>' />
                                          <p> Id Produto : <br> <label><%# Eval("CodProduto") %></label></p>
                                          <p> Nome Produto : <br> <label><%# Eval("NomeProduto") %></label></p>
                                          <p> Categoria : <br> <label><%# Eval("Categoria") %></label></p>
                                          <p> Quantidade : <br><label><%# Eval("QuantidadeItem") %></label></p>
                                          <p> Preço : <br><label><%# Eval("PrecoUnitario") %></label></p>
   
                                       </li>
                                       
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>


                    </div>
                </ItemTemplate>
            </asp:Repeater>

      </div>
   </div>

   <script src="/js/admin_script.js"></script>

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
