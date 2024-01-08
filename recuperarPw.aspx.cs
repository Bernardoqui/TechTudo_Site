using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace TechTudo
{
    public partial class recuperarPw : System.Web.UI.Page
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

        }

        protected void btn_active_Click(object sender, EventArgs e)
        {
            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                using (SqlCommand mycmd = new SqlCommand("recuperarPW", mycon))
                {
                    mycmd.CommandType = CommandType.StoredProcedure;
                    mycmd.Parameters.AddWithValue("@email", tb_email.Text);
                    mycmd.Parameters.AddWithValue("@pass", " ");
                    mycmd.Parameters.AddWithValue("@func", 1);

                    SqlParameter valor = new SqlParameter
                    {
                        ParameterName = "@retorno",
                        Direction = ParameterDirection.Output,
                        SqlDbType = SqlDbType.Int
                    };

                    mycmd.Parameters.Add(valor);

                    mycmd.ExecuteNonQuery();

                    int resposta = Convert.ToInt32(mycmd.Parameters["@retorno"].Value);

                    lbl_Error.Text = resposta.ToString();

                    if (resposta == 1)
                    {
                        lbl_Error.Text = "Link de recuperação enviado para o seu email!!";

                        MailMessage mail = new MailMessage();
                        SmtpClient servidor = new SmtpClient();

                        string smtpUtilizador = ConfigurationManager.AppSettings["SMTP_USER"];
                        string smtpPassword = ConfigurationManager.AppSettings["SMTP_PASS"];

                        servidor.Host = ConfigurationManager.AppSettings["SMTP_HOST"];
                        servidor.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"]);

                        string siteURL = ConfigurationManager.AppSettings["SiteURL"];

                        mail.From = new MailAddress(smtpUtilizador);

                        mail.To.Add(new MailAddress(tb_email.Text));
                        mail.Subject = "Ação de utilizador - Recuperação de conta";
                        mail.IsBodyHtml = true;

                        mail.Body = $"Para recuperar a sua conta clique <a href='{siteURL}/recuperarPW.aspx?email={EncryptString(tb_email.Text)}'> aqui</a>";

                        servidor.Credentials = new NetworkCredential(smtpUtilizador, smtpPassword);
                        servidor.EnableSsl = true;

                        servidor.Send(mail);

                        string css = "<style>.wrapper i { background-color: green; }</style>";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "DynamicStyle", css);
                    }
                }
            }
        }

        protected void btn_guardar_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (Page.IsValid)
            {
                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    int resposta = 0;
                    mycon.Open();
                    using (SqlCommand mycmd = new SqlCommand("recuperarPW", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.AddWithValue("@email", DecryptString(Request.QueryString["email"]));
                        mycmd.Parameters.AddWithValue("@pass", EncryptString(tb_newPass.Text));
                        mycmd.Parameters.AddWithValue("@func", 2);

                        SqlParameter valor = new SqlParameter
                        {
                            ParameterName = "@retorno",
                            Direction = ParameterDirection.Output,
                            SqlDbType = SqlDbType.Int
                        };

                        mycmd.Parameters.Add(valor);

                        mycmd.ExecuteNonQuery();

                        resposta = Convert.ToInt32(mycmd.Parameters["@retorno"].Value);

                        string css = "<style>.wrapper i { background-color: green; }</style>";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "DynamicStyle", css);

                        Response.Redirect("sighIn.aspx");

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