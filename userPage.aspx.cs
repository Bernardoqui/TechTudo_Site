using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.WebPages;

namespace TechTudo
{
    public partial class userPage : System.Web.UI.Page
    {
        public static string conString = ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString;


        protected void ValidatePassword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Regex maiusculas = new Regex("[A-Z]");
            Regex minusculas = new Regex("[a-z]");
            Regex numeros = new Regex("[0-9]");
            Regex especial = new Regex("[^a-zA-Z0-9]");
            Regex pelica = new Regex("'");


            if (tb_newPass.Text.Length < 6)
                args.IsValid = false;
            else if (maiusculas.Matches(tb_newPass.Text).Count < 1)
                args.IsValid = false;
            else if (minusculas.Matches(tb_newPass.Text).Count < 1)
                args.IsValid = false;
            else if (numeros.Matches(tb_newPass.Text).Count < 1)
                args.IsValid = false;
            else if (especial.Matches(tb_newPass.Text).Count < 1)
                args.IsValid = false;
            else if (pelica.Matches(tb_newPass.Text).Count < 0)
                args.IsValid = false;
            else
                args.IsValid = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (globalClass.cod_user.IsEmpty())
                Response.Redirect("homePage.aspx");
            else
            {
                BindData();


                if(tb_email.Text.Length <= 0)
                {
                    tb_email.Text = globalClass.email;
                    tb_nome.Text = globalClass.nome_user;
                    tb_contacto.Text = globalClass.telemovel;
                    tb_nif.Text = globalClass.nif;

                    using (SqlConnection mycon = new SqlConnection(conString))
                    {
                        mycon.Open();
                        using (SqlCommand mycmd = new SqlCommand("procurar_morada", mycon))
                        {
                            mycmd.CommandType = CommandType.StoredProcedure;
                            mycmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);

                            using (SqlDataReader reader = mycmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    globalClass.morada = reader["morada"].ToString();
                                    globalClass.porta_andar = reader["porta_andar"].ToString();
                                    globalClass.cidade = reader["cidade"].ToString();
                                    globalClass.cod_postal = reader["codigo_postal"].ToString();

                                    tb_morada.Text = reader["morada"].ToString();
                                    tb_porta.Text = reader["porta_andar"].ToString();
                                    tb_cidade.Text = reader["cidade"].ToString();
                                    tb_postal.Text = reader["codigo_postal"].ToString();
                                }
                            }
                        }
                    }
                }
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
            }
        }


        static List<Pedido> GetPedidosComItensFromDatabase()
        {
            List<Pedido> pedidos = new List<Pedido>();

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetPedidosById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@cod_user", globalClass.cod_user));

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
                                Morada = reader["Morada"].ToString(),
                                Metodo = reader["Metodo"].ToString(),
                                QuantidadeTotal = (int)reader["QuantidadeTotal"],
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



        protected void btn_guardar_Click1(object sender, EventArgs e)
        {
            Page.Validate();

            if (Page.IsValid)
            {
                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    mycon.Open();
                    using (SqlCommand mycmd = new SqlCommand("alterarPW", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);
                        mycmd.Parameters.AddWithValue("@velhaPass", EncryptString(tb_oldPass.Text));
                        mycmd.Parameters.AddWithValue("@novaPass", EncryptString(tb_newPass.Text));

                        SqlParameter retorno = new SqlParameter
                        {
                            ParameterName = "@retorno",
                            Direction = ParameterDirection.Output,
                            SqlDbType = SqlDbType.Int
                        };

                        mycmd.Parameters.Add(retorno);
                        mycmd.ExecuteNonQuery();

                        int resposta = Convert.ToInt32(mycmd.Parameters["@retorno"].Value);


                        if (resposta == 1)
                        {
                            string script = "showBanner('.banner.success')";
                            ClientScript.RegisterStartupScript(this.GetType(), "ServerMessage", script, true);

                            tb_oldPass.Text = "";
                            tb_newPass.Text = "";
                            tb_newPass2.Text = "";

                        }
                        else if (resposta == 0)
                        {
                            cv_existPass.IsValid = false;
                        }
                    }
                }
            }
        }


        protected void btn_guardarInfo_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("salvarInfo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);
                    cmd.Parameters.AddWithValue("@nome", tb_nome.Text);
                    cmd.Parameters.AddWithValue("@morada", tb_morada.Text);
                    cmd.Parameters.AddWithValue("@porta", tb_porta.Text);
                    cmd.Parameters.AddWithValue("@cidade", tb_cidade.Text);
                    cmd.Parameters.AddWithValue("@postal", tb_postal.Text);
                    cmd.Parameters.AddWithValue("@contacto", int.Parse(tb_contacto.Text));
                    cmd.Parameters.AddWithValue("@nif", int.Parse(tb_nif.Text));

                    globalClass.nome_user = tb_nome.Text;
                    globalClass.telemovel = tb_contacto.Text;
                    globalClass.nif = tb_nif.Text;



                    cmd.ExecuteNonQuery();

                    string script = "showBanner('.banner.success')";
                    ClientScript.RegisterStartupScript(this.GetType(), "ServerMessage", script, true);
                }
            }
        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {
            globalClass.LimparDados();

            Response.Redirect("homePage.aspx");
        }


        public class Pedido
        {
            public int IdPedido { get; set; }
            public DateTime DataPedido { get; set; }
            public string EstadoPedido { get; set; }
            public decimal PrecoFinal { get; set; }
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





        public static string EncryptString(string Message)
        {
            string Passphrase = "techtudo_code";
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();



            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }



            // Step 6. Return the encrypted string as a base64 encoded string
            string enc = Convert.ToBase64String(Results);
            enc = enc.Replace("+", "KKK");
            enc = enc.Replace("/", "JJJ");
            enc = enc.Replace("\\", "III");
            return enc;
        }

        public static string DecryptString(string Message)
        {
            string Passphrase = "techtudo_code";
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();



            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below



            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));



            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();



            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;



            // Step 4. Convert the input string to a byte[]



            Message = Message.Replace("KKK", "+");
            Message = Message.Replace("JJJ", "/");
            Message = Message.Replace("III", "\\");




            byte[] DataToDecrypt = Convert.FromBase64String(Message);



            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }



            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);



        }

    }
}