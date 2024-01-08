using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.Mvc;
using System.Drawing;
using static TechTudo.explorePage;
using System.Web.WebPages;
using Org.BouncyCastle.Ocsp;
using System.Text.RegularExpressions;
using System.Web.Services;
using iText.Kernel.Colors;

namespace TechTudo
{
    public partial class explorePage : System.Web.UI.Page
    {
        public static string conString = ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString;

        string filtro = "";

        string nome_produto = "";

        public List<string> filtros = new List<string>();

        protected List<Caracteristica> ExtrairCaracteristicas(string categoria)
        {
            string connectionString = conString;
            List<Caracteristica> listaCaracteristicas = new List<Caracteristica>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("ObterCaracteristicasPorCategoria", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nomeCategoria", categoria);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string pattern = "%%(.*?): (.*?)%%";
                            MatchCollection matches = Regex.Matches(reader["caracteristicas"].ToString(), pattern);

                            foreach (Match match in matches)
                            {
                                string nomeCaracteristica = match.Groups[1].Value.Trim();
                                string valorCaracteristica = match.Groups[2].Value.Trim();

                                Caracteristica caracteristicaExistente = listaCaracteristicas.Find(c => c.NomeCaracteristica == nomeCaracteristica);

                                if (caracteristicaExistente != null)
                                {
                                    if (!caracteristicaExistente.Valores.Contains(valorCaracteristica))
                                    {
                                        caracteristicaExistente.Valores.Add(valorCaracteristica);
                                    }
                                }
                                else
                                {
                                    Caracteristica novaCaracteristica = new Caracteristica
                                    {
                                        NomeCaracteristica = nomeCaracteristica,
                                        Valores = { valorCaracteristica }
                                    };
                                    listaCaracteristicas.Add(novaCaracteristica);
                                }
                            }
                        }
                    }
                }
            }
            return listaCaracteristicas;
        }

        static bool ExtractValuesIfPresent(string input, string targetValue)
        {
            Regex regex = new Regex($"%%(.*?): {targetValue}%%");

            MatchCollection matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    return true;
                }
            }

            return false;
        }


        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Caracteristica caracteristica = (Caracteristica)e.Item.DataItem;
                Repeater Repeater2 = (Repeater)e.Item.FindControl("Repeater2");

                Repeater2.DataSource = caracteristica.Valores;
                Repeater2.DataBind();
            }
        }


        protected void DataBin()
        {
            filtro = Request.QueryString["filter"];

            nome_produto = Request.QueryString["produto"];


            using (SqlConnection mycon = new SqlConnection(conString))
            {
                List<Produto> lst_produtoInfo = new List<Produto>();

                mycon.Open();
                using (SqlCommand mycmd = new SqlCommand("explorar_produtos", mycon))
                {
                    mycmd.CommandType = CommandType.StoredProcedure;
                    mycmd.Parameters.AddWithValue("@Ordenacao", 0);

                    using (SqlDataReader dr = mycmd.ExecuteReader())
                    {
                        var produto = new Produto();

                        while (dr.Read())
                        {
                            produto = new Produto();

                            produto.cod = dr.GetInt32(0);
                            produto.nome = dr.GetString(1);
                            produto.rating = dr.GetInt32(2);
                            produto.price = dr.GetDecimal(3);
                            produto.categoria = dr.GetString(4);
                            if (!dr.IsDBNull(5))
                            {
                                produto.img = dr.GetSqlBytes(5).Value;
                            }

                            produto.caracteristicas = dr.GetString(6);

                            if (filtros.Any())
                            {
                                foreach (string fil in filtros)
                                {
                                    if (ExtractValuesIfPresent(produto.caracteristicas, fil))
                                    {
                                        if (dr.GetString(1) != null && nome_produto != null)
                                        {
                                            if (dr.GetString(1).IndexOf(nome_produto, StringComparison.OrdinalIgnoreCase) >= 0)
                                            {
                                                lst_produtoInfo.Add(produto);
                                            }
                                        }
                                        if (filtro == "all")
                                        {
                                            lst_produtoInfo.Add(produto);
                                        }
                                        if (filtro == dr.GetString(4))
                                        {
                                            lst_produtoInfo.Add(produto);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (dr.GetString(1) != null && nome_produto != null)
                                {
                                    if (dr.GetString(1).IndexOf(nome_produto, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        lst_produtoInfo.Add(produto);
                                    }
                                }
                                if (filtro == "all")
                                {
                                    lst_produtoInfo.Add(produto);
                                }
                                if (filtro == dr.GetString(4))
                                {
                                    lst_produtoInfo.Add(produto);
                                }
                            }


                        }
                    }
                    rpt_cardDetails.DataSource = lst_produtoInfo;
                    rpt_cardDetails.DataBind();
                    lst_produtoInfo.Clear();
                }
            }

            if (!filtro.IsEmpty())
            {
                List<Caracteristica> listaCaracteristicas = ExtrairCaracteristicas(filtro);

                // Configurar o DataSource e chamar DataBind()
                Repeater1.DataSource = listaCaracteristicas;
                Repeater1.DataBind();

                if (filtro == "all")
                {
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        connection.Open();

                        List<string> categorias = new List<string>();

                        using (SqlCommand cmd = new SqlCommand("ObterCategorias", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    categorias.Add(reader["categoria"].ToString());
                                }
                            }


                            List<Caracteristica> lista = new List<Caracteristica>();

                            Caracteristica novaCaracteristica = new Caracteristica
                            {
                                NomeCaracteristica = "Categorias",
                                Valores = categorias
                            };
                            lista.Add(novaCaracteristica);


                            Repeater1.DataSource = lista;
                            Repeater1.DataBind();
                        }
                    }
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                filtros.Clear();
                DataBin();
            }
        }


        protected void ddl_ordenar_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Produto> lst_produtoInfo = new List<Produto>();

            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                using (SqlCommand mycmd = new SqlCommand("explorar_produtos", mycon))
                {
                    mycmd.CommandType = CommandType.StoredProcedure;
                    mycmd.Parameters.AddWithValue("@Ordenacao", ddl_ordenar.SelectedValue);

                    using (SqlDataReader dr = mycmd.ExecuteReader())
                    {
                        var produto = new Produto();

                        while (dr.Read())
                        {
                            produto = new Produto();

                            produto.cod = dr.GetInt32(0);
                            produto.nome = dr.GetString(1);
                            produto.rating = dr.GetInt32(2);
                            produto.price = dr.GetDecimal(3);
                            produto.categoria = dr.GetString(4);
                            if (!dr.IsDBNull(5))
                            {
                                produto.img = dr.GetSqlBytes(5).Value;
                            }

                            if (dr.GetString(1) != null && nome_produto != null)
                            {
                                if (dr.GetString(1).IndexOf(nome_produto, StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    lst_produtoInfo.Add(produto);
                                }
                            }
                            if (filtro == "all")
                            {
                                lst_produtoInfo.Add(produto);
                            }

                        }
                    }
                }
            }

            rpt_cardDetails.DataSource = lst_produtoInfo;
            rpt_cardDetails.DataBind();
            lst_produtoInfo.Clear();

            UpdateProdutos.Update();
        }

        protected void btn_filtrar_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in Repeater1.Items)
            {
                Repeater Repeater2 = (Repeater)item.FindControl("Repeater2");

                foreach (RepeaterItem item2 in Repeater2.Items)
                {
                    CheckBox cb_filtr = (CheckBox)item2.FindControl("cb_filtr");

                    if (cb_filtr.Checked)
                    {
                        string valor = cb_filtr.Attributes["name"];
                        filtros.Add(valor);
                        ddl_ordenar.Items.Add(valor);
                    }
                }
            }
            DataBin();
        }


        [WebMethod]
        public static void AdicionarAoCarrinho(int produtoId)
        {
            globalClass.carrinho.Add(produtoId);
                
            if (globalClass.cod_user != null)
            {
                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    mycon.Open();
                    using (SqlCommand mycmd = new SqlCommand("adicionar_carrinho", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.AddWithValue("@cod_produto", produtoId);
                        mycmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);

                        mycmd.ExecuteReader();
                    }
                }
            }
        }


        public class Caracteristica
        {
            public string NomeCaracteristica { get; set; }
            public List<string> Valores { get; set; } = new List<string>();
        }


        public class Produto
        {
            public int cod { get; set; }
            public string nome { get; set; }
            public int rating { get; set; }
            public decimal price { get; set; }
            public string categoria { get; set; }
            public string caracteristicas { get; set; }
            public byte[] img { get; set; }
        }
    }
}
