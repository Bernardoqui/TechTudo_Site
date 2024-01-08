using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;
using System.Configuration;
using System.Web.WebPages;
using Org.BouncyCastle.Ocsp;
using System.Text.RegularExpressions;
using System.Web.Services;

namespace TechTudo
{
    public partial class productPage : System.Web.UI.Page
    {
        public static string conString = ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString;

        public string cod_produto = "";

        public static decimal valor = 0;


        public class ProdutoRelacionado
        {
            public int CodProduto { get; set; }
            public string NomeProduto { get; set; }
            public decimal Preco { get; set; }
            public byte[] ImgPrincipal { get; set; }

            public ProdutoRelacionado(int codProduto, string nomeProduto, decimal preco, byte[] imgPrincipal)
            {
                CodProduto = codProduto;
                NomeProduto = nomeProduto;
                Preco = preco;
                ImgPrincipal = imgPrincipal;
            }

        }

        Dictionary<string, string> dic_categorias = new Dictionary<string, string>();


        protected void Page_Load(object sender, EventArgs e)
        {
            cod_produto = Request.QueryString["product"];


            if (!IsPostBack)
            {
                if (!cod_produto.IsEmpty())
                {
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        // Abra a conexão
                        connection.Open();

                        string numeroString = cod_produto.Trim('\'');
                        int cod = Convert.ToInt32(numeroString);

                        using (SqlCommand command = new SqlCommand("produtoPage", connection))
                        {

                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@cod_produto", cod);

                            // Crie um leitor de dados para obter os resultados da stored procedure
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Verifique se existem linhas retornadas
                                if (reader.HasRows)
                                {
                                    // Leia os resultados da primeira consulta
                                    while (reader.Read())
                                    {
                                        // Acesse os dados do leitor usando os índices ou os nomes das colunas
                                        byte[] imgBytes = (byte[])reader["img_principal"];
                                        string nomeProduto = reader["nome_produto"].ToString();
                                        decimal preco = Convert.ToDecimal(reader["price"]);
                                        string caracteristicas = reader["caracteristicas"].ToString();

                                        img_principal.ImageUrl = $"data:image/jpeg;base64, {Convert.ToBase64String(imgBytes)}";
                                        lbl_title.Text = nomeProduto;
                                        valor = preco;
                                        lbl_cod.Text = cod_produto;

                                        if (!string.IsNullOrEmpty(caracteristicas))
                                        {
                                            MatchCollection matches = Regex.Matches(caracteristicas, "%%(.*?): (.*?)%%");

                                            string chave = "";
                                            string valo = "";

                                            foreach (Match match in matches)
                                            {
                                                chave = match.Groups[1].Value;
                                                valo = match.Groups[2].Value;

                                                dic_categorias.Add(chave, valo);
                                            }

                                            rpt_caract.DataSource = dic_categorias;
                                            rpt_caract.DataBind();
                                        }
                                    }


                                    // Avance para o próximo conjunto de resultados
                                    reader.NextResult();


                                    List<byte[]> imgs = new List<byte[]>();

                                    while (reader.Read())
                                    {
                                        imgs.Add((byte[])reader["img"]);
                                    }

                                    rpt_imgs.DataSource = imgs;
                                    rpt_imgs.DataBind();

                                    // Avance para o próximo conjunto de resultados
                                    reader.NextResult();


                                    List<ProdutoRelacionado> relacionados = new List<ProdutoRelacionado>();

                                    // Leitura dos resultados da terceira consulta
                                    while (reader.Read())
                                    {
                                        int codProduto = Convert.ToInt32(reader["CodProduto"]);
                                        string nomeProdutoTop5 = reader["NomeProduto"].ToString();
                                        decimal precoTop5 = Convert.ToDecimal(reader["Preco"]);
                                        byte[] imgPrincipalTop5 = (byte[])reader["ImgPrincipal"];

                                        relacionados.Add(new ProdutoRelacionado(codProduto, nomeProdutoTop5, precoTop5, imgPrincipalTop5));
                                    }

                                    rpt_relacionados.DataSource = relacionados;
                                    rpt_relacionados.DataBind();
                                }
                            }
                        }
                    }
                }
            }

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



        protected void funcs(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "compra")
            {
                string numeroString = cod_produto.Trim('\'');

                globalClass.carrinho.Add(int.Parse(numeroString));

                if (globalClass.cod_user != null)
                {
                    using (SqlConnection mycon = new SqlConnection(conString))
                    {
                        mycon.Open();
                        using (SqlCommand mycmd = new SqlCommand("adicionar_carrinho", mycon))
                        {
                            mycmd.CommandType = CommandType.StoredProcedure;
                            mycmd.Parameters.AddWithValue("@cod_produto", int.Parse(numeroString));
                            mycmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);

                            mycmd.ExecuteReader();
                        }
                    }
                }
            }
            else if (e.CommandName == "favorito")
            {
                string numeroString = cod_produto.Trim('\'');


                if (globalClass.cod_user != null)
                {
                    using (SqlConnection mycon = new SqlConnection(conString))
                    {
                        mycon.Open();
                        using (SqlCommand mycmd = new SqlCommand("new_favorito", mycon))
                        {
                            mycmd.CommandType = CommandType.StoredProcedure;
                            mycmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);
                            mycmd.Parameters.AddWithValue("@cod_produto", int.Parse(numeroString));

                            mycmd.ExecuteReader();
                        }
                    }
                }
            }
        }
    }
}