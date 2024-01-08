using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Web.Helpers;
using static TechTudo.admin_Orders;
using System.IO;
using System.Net.Mail;
using System.Net;
using static TechTudo.userPage;

namespace TechTudo
{
    public partial class adminPage : System.Web.UI.Page
    {
        string conString = ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (globalClass.cod_user == null || globalClass.type_user != "Admin")
                Response.Redirect("sighIn.aspx");
            else
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("admin_dashboard", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lbl_valorPendente.Text = string.Format("{0:C}", Convert.ToDecimal(reader["ValorPendente"]));
                                lbl_valorCompletos.Text = string.Format("{0:C}", Convert.ToDecimal(reader["ValorCompletos"]));
                                lbl_pedidos.Text = reader["QuantidadePedidos"].ToString();
                                lbl_produtos.Text = reader["QuantidadeProdutosAtivos"].ToString();
                                lbl_users.Text = reader["QuantidadeUsers"].ToString();
                                lbl_revendedores.Text = reader["QuantidadeRevendedores"].ToString();
                                lbl_admins.Text = reader["QuantidadeAdmins"].ToString();
                                lbl_contas.Text = reader["QuantidadeContas"].ToString();
                            }
                        }
                    }
                }
            }
        }

        protected void btn_criarUser_Click(object sender, EventArgs e)
        {
            SqlConnection mycon = new SqlConnection(conString);

            SqlCommand mycmd = new SqlCommand();
            mycmd.CommandType = CommandType.StoredProcedure;
            mycmd.CommandText = "criar_user";

            mycmd.Connection = mycon;

            mycmd.Parameters.AddWithValue("@email", tb_email.Text);
            mycmd.Parameters.AddWithValue("@password", EncryptString(tb_pass.Text));
            mycmd.Parameters.AddWithValue("@type", ddl_typeUser.SelectedValue);
            mycmd.Parameters.AddWithValue("@nome", tb_nome.Text);

            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;

            mycmd.Parameters.Add(valor);

            mycon.Open();
            mycmd.ExecuteNonQuery();

            int resposta = Convert.ToInt32(mycmd.Parameters["@retorno"].Value);

            mycon.Close();

            if (resposta == 1)
            {
                emailExistente.IsValid = true;

                string destinatario = tb_email.Text;
                string assunto = $"A sua conta {ddl_typeUser.SelectedItem} foi criada com sucesso";
                string corpoEmail = $"Abaixo segue-se as suas informaçoes de login: \n Email: {tb_email.Text} \n PassWord: {tb_pass.Text}";

                if (string.IsNullOrEmpty(destinatario))
                {
                    return;
                }

                string smtpUtilizador = ConfigurationManager.AppSettings["SMTP_USER"];
                string smtpPassword = ConfigurationManager.AppSettings["SMTP_PASS"];

                using (SmtpClient servidor = new SmtpClient())
                {
                    servidor.Host = ConfigurationManager.AppSettings["SMTP_HOST"];
                    servidor.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"]);

                    using (MailMessage mailMessage = new MailMessage(smtpUtilizador, destinatario))
                    {
                        mailMessage.Subject = assunto;
                        mailMessage.Body = corpoEmail;


                        servidor.Credentials = new NetworkCredential(smtpUtilizador, smtpPassword);
                        servidor.EnableSsl = true;

                        servidor.Send(mailMessage);
                    }
                }


                Response.Redirect("adminPage.aspx");
            }
            else if (resposta == 0)
            {
                emailExistente.IsValid = false;
            }
        }


        protected void btn_gerarPass_Click(object sender, EventArgs e)
        {
            int length = 12;
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_-";

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[length];
                rng.GetBytes(bytes);

                char[] chars = new char[length];
                int validCharCount = validChars.Length;

                for (int i = 0; i < length; i++)
                {
                    chars[i] = validChars[bytes[i] % validCharCount];
                }

                tb_pass.Text = new string(chars);
            }

           
        }


        protected void btn_cupao_Click(object sender, EventArgs e)
        {
            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                using (SqlCommand mycmd = new SqlCommand("novoCupao", mycon))
                {
                    mycmd.CommandType = CommandType.StoredProcedure;
                    mycmd.Parameters.AddWithValue("@codigo", tb_codigo.Text);
                    mycmd.Parameters.AddWithValue("@desconto", int.Parse(tb_desconto.Text));
                    mycmd.Parameters.AddWithValue("@cod_categoria", ddl_productType.SelectedValue);
                    mycmd.Parameters.AddWithValue("@data_comeco", DateTime.Parse(tb_dataComeco.Text));
                    mycmd.Parameters.AddWithValue("@data_final", DateTime.Parse(tb_dataTermino.Text));


                    using (SqlDataReader dr = mycmd.ExecuteReader())
                    {
                        string destinatario = "";

                        string SiteURL = ConfigurationManager.AppSettings["SiteURL"];

                        string smtpUtilizador = ConfigurationManager.AppSettings["SMTP_USER"];
                        string smtpPassword = ConfigurationManager.AppSettings["SMTP_PASS"];


                        while (dr.Read())
                        {
                            destinatario = dr.GetString(0);

                            if (string.IsNullOrEmpty(destinatario))
                            {
                                return;
                            }

                            using (SmtpClient servidor = new SmtpClient())
                            {
                                servidor.Host = ConfigurationManager.AppSettings["SMTP_HOST"];
                                servidor.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"]);

                                using (MailMessage mailMessage = new MailMessage(smtpUtilizador, destinatario))
                                {
                                    mailMessage.Subject = "Cupão de Desconto";

                                    // Ler o conteúdo do arquivo HTML
                                    string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emailTemplate.html");

                                    // Ler o conteúdo do arquivo HTML
                                    string htmlBody = File.ReadAllText(templatePath);


                                    Dictionary<string, string> variaveis = new Dictionary<string, string>
                                    {
                                        { "{CODE}", tb_codigo.Text },
                                        { "{DESCONTO}", tb_desconto.Text },
                                        { "{TYPE_PRODUCT}", ddl_productType.SelectedItem.Text },
                                        { "{DATA_COMECO}", tb_dataComeco.Text },
                                        { "{DATA_TERMINO}", tb_dataTermino.Text },
                                        { "{URL_LOJA}", SiteURL + "homePage.aspx" },
                                        { "{URL_BLOG}", SiteURL + "blogPage.aspx" },
                                    };

                                    // Aplicar substituições no template
                                    foreach (var variavel in variaveis)
                                    {
                                        htmlBody = htmlBody.Replace(variavel.Key, variavel.Value);
                                    }



                                    mailMessage.Body = htmlBody;
                                    mailMessage.IsBodyHtml = true;

                                    servidor.Credentials = new NetworkCredential(smtpUtilizador, smtpPassword);
                                    servidor.EnableSsl = true;

                                    servidor.Send(mailMessage);
                                    Response.Redirect("adminPage.aspx");
                                }
                            }
                        }
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