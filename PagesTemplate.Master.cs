using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.WebPages;

namespace TechTudo
{
    public partial class PagesTemplate : System.Web.UI.MasterPage
    {
        public static string conString = ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!globalClass.cod_user.IsEmpty())
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand("procurar_favoritos", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);

                        con.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rpt_favoritos.DataSource = rdr;
                            rpt_favoritos.DataBind();
                        }
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


        protected void btn_createAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("sighIn.aspx");
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            if (tb_email.Text != "" && tb_pass.Text != "")
            {
                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    int resposta = 0;
                    mycon.Open();
                    using (SqlCommand mycmd = new SqlCommand("login_user", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.AddWithValue("@email", tb_email.Text);
                        mycmd.Parameters.AddWithValue("@password", EncryptString(tb_pass.Text));

                        SqlParameter valor = new SqlParameter
                        {
                            ParameterName = "@retorno",
                            Direction = ParameterDirection.Output,
                            SqlDbType = SqlDbType.Int
                        };

                        mycmd.Parameters.Add(valor);

                        mycmd.ExecuteNonQuery();


                        resposta = Convert.ToInt32(mycmd.Parameters["@retorno"].Value);

                        using (SqlDataReader reader = mycmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                globalClass.cod_user = reader["id_user"].ToString();
                                globalClass.nome_user = reader["nome_user"].ToString();
                                globalClass.type_user = reader["type_user"].ToString();
                                globalClass.telemovel = reader["telemovel"].ToString();
                                globalClass.nif = reader["nif"].ToString();
                                globalClass.ativo = reader.GetBoolean(reader.GetOrdinal("ativo"));

                                globalClass.email = tb_email.Text;
                            }
                        }


                    }

                    foreach (int produtoId in globalClass.carrinho)
                    {
                        using (SqlCommand mycmd = new SqlCommand("adicionar_carrinho", mycon))
                        {
                            mycmd.CommandType = CommandType.StoredProcedure;
                            mycmd.Parameters.AddWithValue("@cod_produto", produtoId);
                            mycmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);

                            mycmd.ExecuteNonQuery();
                        }
                    }

                    if (resposta == 1)
                    {
                        Response.Redirect("adminPage.aspx");
                    }
                    if (resposta == 2)
                    {
                        Response.Redirect("homePage.aspx");
                    }
                }
            }
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

    }
}