using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TechTudo
{
    public partial class admin_Orders : System.Web.UI.Page
    {
        static string conString = ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (globalClass.cod_user == null || globalClass.type_user != "Admin")
                Response.Redirect("sighIn.aspx");
            else if (!IsPostBack)
            {
                BindData();
            }
            

        }

        private void BindData()
        {
            List<Pedido> pedidos = GetPedidosComItensFromDatabase();
            rptPedidos.DataSource = pedidos;
            rptPedidos.DataBind();
        }

        protected void rptPedidos_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item ||
                e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                Repeater rptItens = (Repeater)e.Item.FindControl("rptItens");
                Pedido pedido = (Pedido)e.Item.DataItem;
                rptItens.DataSource = pedido.Itens;
                rptItens.DataBind();



                var estadoPedido = DataBinder.Eval(e.Item.DataItem, "EstadoPedido").ToString();
                var btnConcluir = (Button)e.Item.FindControl("btn_concluir");
                var lblEstadoPedido = (Label)e.Item.FindControl("lblEstadoPedido");

                if (estadoPedido == "Pendente")
                {
                    btnConcluir.Visible = true;
                    lblEstadoPedido.Visible = false;
                }
                else if (estadoPedido == "Concluido")
                {
                    btnConcluir.Visible = false;
                    lblEstadoPedido.Visible = true;
                    lblEstadoPedido.Text = estadoPedido;
                }

            }
        }


        static List<Pedido> GetPedidosComItensFromDatabase()
        {
            List<Pedido> pedidos = new List<Pedido>();

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetPedidos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var pedido = new Pedido
                            {
                                IdPedido = (int)reader["IdPedido"],
                                DataPedido = (DateTime)reader["DataPedido"],
                                EstadoPedido = reader["EstadoPedido"].ToString(),
                                PrecoFinal = (decimal)reader["PrecoFinal"],
                                CodUser = (int)reader["CodUser"],
                                NomeUser = reader["NomeUser"].ToString(),
                                Contacto = (int)reader["Contacto"],
                                Email = reader["Email"].ToString(),
                                Morada = reader["Morada"].ToString(),
                                Metodo = reader["Metodo"].ToString(),
                                QuantidadeTotal = (int)reader["QuantidadeTotal"], // Adicione esta linha
                                Itens = new List<ItemPedido>()
                            };



                            var item = new ItemPedido
                            {
                                ImgProduto = (byte[])reader["ImgProduto"],
                                CodProduto = (int)reader["CodProduto"],
                                Categoria = reader["Categoria"].ToString(),
                                NomeProduto = reader["NomeProduto"].ToString(),
                                QuantidadeItem = (int)reader["QuantidadeItem"],
                                PrecoUnitario = (decimal)reader["PrecoUnitario"]
                            };

                            if (!pedidos.Any(Pedi => Pedi.IdPedido == (int)reader["IdPedido"]))
                            {
                                pedido.Itens.Add(item);

                                pedidos.Add(pedido);
                            }
                            else
                            {
                                pedidos.Last().Itens.Add(item);
                            }
                        }
                    }
                }
            }

            return pedidos;
        }

        protected string GetImageUrl(object imgProduto)
        {
            if (imgProduto != null && imgProduto != DBNull.Value)
            {
                if (imgProduto is byte[])
                {
                    // Se imgProduto for um byte[], converte para string base64
                    return "data:image/jpeg;base64," + Convert.ToBase64String((byte[])imgProduto);
                }
                else if (imgProduto is string)
                {
                    // Se imgProduto for uma string, utiliza diretamente
                    return (string)imgProduto;
                }
            }

            // Caso imgProduto seja null ou de um tipo não suportado, utiliza uma imagem padrão
            return "/img/default.png";
        }


        protected void concluir(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "concl")
            {
                int cod_pedido = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    mycon.Open();
                    using (SqlCommand mycmd = new SqlCommand("concluirPedido", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.AddWithValue("@cod_pedido", cod_pedido);

                        mycmd.ExecuteNonQuery();
                    }
                }
                BindData();
            }
        }


        public class Pedido
        {
            public int IdPedido { get; set; }
            public DateTime DataPedido { get; set; }
            public string EstadoPedido { get; set; }
            public decimal PrecoFinal { get; set; }
            public int CodUser { get; set; }
            public string NomeUser { get; set; }
            public int Contacto { get; set; }
            public string Email { get; set; }
            public string Morada { get; set; }
            public string Metodo { get; set; }
            public int QuantidadeTotal { get; set; }
            public List<ItemPedido> Itens { get; set; }
        }

        public class ItemPedido
        {
            public byte[] ImgProduto { get; set; }
            public int CodProduto { get; set; }
            public string Categoria { get; set; }
            public string NomeProduto { get; set; }
            public int QuantidadeItem { get; set; }
            public decimal PrecoUnitario { get; set; }
        }
    }
}