using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Services;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace TechTudo
{
    public partial class homePage : System.Web.UI.Page
    {
        public static string conString = ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString;

        int pageSize = 8;
        int currentPage = 1;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData(1); 
            }

            if (ViewState["CurrentPage"] != null)
            {
                currentPage = Convert.ToInt32(ViewState["CurrentPage"]);
            }
        }

        // ...

        private void BindData(int currentPage)
        {
            List<Produto> allProducts = GetProductsFromDatabase(); 

            // Realiza a paginação nos dados
            var pagedProducts = allProducts.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            rpt_cardDetails.DataSource = pagedProducts;
            rpt_cardDetails.DataBind();
        }

        // ...


        protected void pagination(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "next")
            {
                int totalRecords = GetTotalRecords();
                int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                if (currentPage < totalPages)
                {
                    currentPage++;
                    BindData(currentPage);
                    ViewState["CurrentPage"] = currentPage;
                    UpdateDestaques.Update();
                }
            }
            if (e.CommandName == "prev")
            {
                if (currentPage > 1)
                {
                    currentPage--;
                    BindData(currentPage);
                    ViewState["CurrentPage"] = currentPage;
                    UpdateDestaques.Update();
                }
            }
        }


        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                BindData(currentPage);
                ViewState["CurrentPage"] = currentPage;
                UpdateDestaques.Update();
            }
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            int totalRecords = GetTotalRecords(); 
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (currentPage < totalPages)
            {
                currentPage++;
                BindData(currentPage);
                ViewState["CurrentPage"] = currentPage;
                UpdateDestaques.Update();
            }
        }


        private List<Produto> GetProductsFromDatabase()
        {
            List<Produto> lst_produtoInfo = new List<Produto>();

            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                using (SqlCommand mycmd = new SqlCommand("destaques_produtos", mycon))
                {
                    mycmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = mycmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var produto = new Produto();

                            produto.cod = dr.GetInt32(0);
                            produto.nome = dr.GetString(1);
                            produto.rating = dr.GetInt32(2);
                            produto.price = dr.GetDecimal(3);
                            produto.categoria = dr.GetString(4);
                            if (!dr.IsDBNull(5))
                            {
                                produto.img = dr.GetSqlBytes(5).Value;
                            }

                            lst_produtoInfo.Add(produto);
                        }
                    }
                }
            }

            return lst_produtoInfo;
        }

        private int GetTotalRecords()
        {
            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                using (SqlCommand mycmd = new SqlCommand("SELECT COUNT(*) FROM Produtos", mycon))
                {
                    // ExecuteScalar retorna a contagem total de registros
                    return (int)mycmd.ExecuteScalar();
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

        [WebMethod]
        public static void Subscrever(string email)
        {
            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                using (SqlCommand mycmd = new SqlCommand("subscrever_User", mycon))
                {
                    mycmd.CommandType = CommandType.StoredProcedure;
                    mycmd.Parameters.AddWithValue("@email", email);

                    mycmd.ExecuteReader();
                }
            }
        }


        public class Produto
        {
            public int cod { get; set; }
            public string nome { get; set; }
            public int rating { get; set; }
            public decimal price { get; set; }
            public string categoria { get; set; }
            public byte[] img { get; set; }
        }
    }
}