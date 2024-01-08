using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.Services;

namespace TechTudo
{
    public partial class sighIn : System.Web.UI.Page
    {
        public static string conString = ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ValidatePassword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Regex maiusculas = new Regex("[A-Z]");
            Regex minusculas = new Regex("[a-z]");
            Regex numeros = new Regex("[0-9]");
            Regex especial = new Regex("[^a-zA-Z0-9]");
            Regex pelica = new Regex("'");


            if (tb_pass_Regist.Text.Length < 6)
                args.IsValid = false;
            else if (maiusculas.Matches(tb_pass_Regist.Text).Count < 1)
                args.IsValid = false;
            else if (minusculas.Matches(tb_pass_Regist.Text).Count < 1)
                args.IsValid = false;
            else if (numeros.Matches(tb_pass_Regist.Text).Count < 1)
                args.IsValid = false;
            else if (especial.Matches(tb_pass_Regist.Text).Count < 1)
                args.IsValid = false;
            else if (pelica.Matches(tb_pass_Regist.Text).Count < 0)
                args.IsValid = false;
            else
                args.IsValid = true;
        }

        public void login_user(string email, string pass, bool login)
        {
            if (email != "" && pass != "")
            {
                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    int resposta = 0;
                    mycon.Open();
                    using (SqlCommand mycmd = new SqlCommand("login_user", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.AddWithValue("@email", email);
                        mycmd.Parameters.AddWithValue("@password", EncryptString(pass));

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

                                globalClass.email = tb_email_Login.Text;
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

                    if(login)
                    {
                        if (resposta == 1)
                        {
                            Response.Redirect("adminPage.aspx");
                        }
                        else if (resposta == 2)
                        {
                            Response.Redirect("homePage.aspx");
                        }
                        else if (resposta == 0)
                        {
                            lbl_loginError.Text = "Email e/ou Palavra-Passe errados";
                        }
                        else if (resposta == -1)
                        {
                            lbl_loginError.Text = "Conta Inativa";
                        }
                    }
                }
            }
        }


        protected void btn_createAccount_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (Page.IsValid)
            {
                SqlConnection mycon = new SqlConnection(ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString);

                SqlCommand mycmd = new SqlCommand();
                mycmd.CommandType = CommandType.StoredProcedure;
                mycmd.CommandText = "criar_user";

                mycmd.Connection = mycon;

                mycmd.Parameters.AddWithValue("@email", tb_email_Regist.Text);
                mycmd.Parameters.AddWithValue("@password", EncryptString(tb_pass_Regist.Text));
                mycmd.Parameters.AddWithValue("@type", "User");
                mycmd.Parameters.AddWithValue("@nome", "User");

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
                    login_user(tb_email_Regist.Text, tb_pass_Regist.Text, false);

                    lbl_aut.Text = "Utilizador inserido com sucesso!! \n Porfavor ativar conta pelo email!!";

                    MailMessage mail = new MailMessage();
                    SmtpClient servidor = new SmtpClient();

                    string smtpUtilizador = ConfigurationManager.AppSettings["SMTP_USER"];
                    string smtpPassword = ConfigurationManager.AppSettings["SMTP_PASS"];

                    servidor.Host = ConfigurationManager.AppSettings["SMTP_HOST"];
                    servidor.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"]);

                    string siteURL = ConfigurationManager.AppSettings["SiteURL"];

                    mail.From = new MailAddress(smtpUtilizador);

                    mail.To.Add(new MailAddress(tb_email_Regist.Text));
                    mail.Subject = "Registo de utilizador - Ativação de conta";
                    mail.IsBodyHtml = true;

                    mail.Body = $"O seu utilizador foi criado com sucesso, para ativar clique <a href='{siteURL}/active_account.aspx?utilizador={EncryptString(tb_email_Regist.Text)}'> aqui</a>";

                    servidor.Credentials = new NetworkCredential(smtpUtilizador, smtpPassword);
                    servidor.EnableSsl = true;

                    servidor.Send(mail);

                }
                else if (resposta == 0)
                {
                    emailExistente.IsValid = false;
                }
            }
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            login_user(tb_email_Login.Text, tb_pass_Login.Text, true);
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