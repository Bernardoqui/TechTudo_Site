<%@ Page Title="" Language="C#" MasterPageFile="~/AdminHeader.Master" AutoEventWireup="true" CodeBehind="adminUtilizadores.aspx.cs" Inherits="TechTudo.adminUtilizadores" EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="users">

        <h1 class="title">users account</h1>


            <div class="card">
                <div class="table-title">
                </div>
                <div class="button-container">
                    <asp:Button ID="btn_search" class="search" runat="server" Text="Procurar" OnClick="btn_search_Click" />

                    <asp:TextBox ID="tb_idUser" CssClass="tb_id" runat="server" placeholder="Nome / Email / Tipo"></asp:TextBox>
                </div>

                <div class="table-concept">
                    <input class="table-radio" type="radio" name="table_radio" id="table_radio_0" checked="checked" />
                    <div class="table-display">
                    </div>

                    <asp:Repeater ID="rpt_users" runat="server">

                        <HeaderTemplate>
                            <table>
                                <thead>
                                    <tr>
                                        <th><b>Id</b></th>
                                        <th><b>Criada em</b></th>
                                        <th><b>Nome</b></th>
                                        <th><b>Email</b></th>
                                        <th><b>Tipo</b></th>
                                    </tr>
                                </thead>
                            
                        </HeaderTemplate>

                        <ItemTemplate>

                            <tbody>
                                <tr>
                                    <tr class="table-row">
                                        <td>
                                            <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_data" runat="server" Text='<%# Eval("data") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tb_nome" runat="server" placeholder="Nome" Text='<%# Eval("nome") %>' CssClass="cell"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tb_email" runat="server" Text='<%# Eval("email") %>' CssClass="cell"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tb_type" runat="server" Text='<%# Eval("type") %>' CssClass="cell"></asp:TextBox>
                                        </td>
                                        <td>
                                            <div class="button-container">

                                                <asp:Button ID="btn_desative" runat="server" Text="Desativar" CssClass="danger" title="Delete Selected"
                                                    Visible='<%# Convert.ToBoolean(Eval("ativo")) %>' OnCommand="editUser" CommandName="desativar" CommandArgument='<%# Eval("id") %>' />
                                                <asp:Button ID="btn_ativar" runat="server" Text="Ativar" CssClass="active" title="Delete Selected"
                                                    Visible='<%# !Convert.ToBoolean(Eval("ativo")) %>' OnCommand="editUser" CommandName="ativar" CommandArgument='<%# Eval("id") %>'/>

                                                <asp:Button ID="btn_save" runat="server" Text="Salvar" class="primary" title="Save Data" OnCommand="editUser" CommandName="save" CommandArgument='<%# Eval("id") %>'/>
                                            </div>
                                        </td>
                                    </tr>
                                </tr>

                            </tbody>
                        </ItemTemplate>

                        <FooterTemplate>
                            </table>
                        </FooterTemplate>

                    </asp:Repeater>
                </div>
            </div>

    </section>

    <script src="/js/admin_script.js"></script>

</asp:Content>
