using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Net;
using iText.Layout;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics.Contracts;
using Org.BouncyCastle.Ocsp;

namespace TechTudo
{
    public partial class cartPage : System.Web.UI.Page
    {
        public static string conString = ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString;

        decimal totalDesconto = 0;
        string typeDesconto = "";

        public void UpdateCarrinho()
        {
            List<Produto> lst_produtoInfo = new List<Produto>();

            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                using (SqlCommand mycmd = new SqlCommand("procurar_carrinho", mycon))
                {
                    mycmd.CommandType = CommandType.StoredProcedure;
                    mycmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);

                    using (SqlDataReader dr = mycmd.ExecuteReader())
                    {
                        decimal itemsPreco = 0;
                        int itemsTotal = 0;
                        globalClass.valor_produtos = 0;

                        var produto = new Produto();

                        while (dr.Read())
                        {
                            produto = new Produto();

                            produto.cod = dr.GetInt32(0);
                            produto.nome = dr.GetString(1);
                            produto.price = dr.GetDecimal(2);
                            produto.quantidade = dr.GetInt32(3);


                            if((globalClass.desconto > 0 && typeDesconto == dr.GetString(4)) || globalClass.type_user == "Revendedor")
                            {
                                globalClass.valor_produtos += ((produto.price - (produto.price * (globalClass.desconto / 100))) * produto.quantidade);
                                itemsPreco += produto.price * produto.quantidade;
                                totalDesconto += (produto.price * (globalClass.desconto / 100));
                            }
                            else
                            {
                                globalClass.valor_produtos += (produto.price * produto.quantidade);
                                itemsPreco += produto.price * produto.quantidade;
                            }
                                

                            itemsTotal += produto.quantidade;
                            produto.categoria = dr.GetString(4);
                            if (!dr.IsDBNull(5))
                            {
                                produto.img = dr.GetSqlBytes(5).Value;
                            }

                            lst_produtoInfo.Add(produto);
                        }

                        lbl_itemsTotal.Text = itemsPreco.ToString();
                        lbl_preco.Text = itemsPreco.ToString();
                        rpt_products.DataSource = lst_produtoInfo;
                        rpt_products.DataBind();
                        lst_produtoInfo.Clear();
                    }
                }

                using (SqlCommand cmd = new SqlCommand("preco_portes", mycon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);

                    // Adicionar parâmetros de saída
                    SqlParameter outputParamPeso = new SqlParameter("@soma_peso", SqlDbType.Decimal);
                    outputParamPeso.Direction = ParameterDirection.Output;
                    outputParamPeso.Precision = 18;
                    outputParamPeso.Scale = 2;
                    cmd.Parameters.Add(outputParamPeso);

                    SqlParameter outputParamPortes = new SqlParameter("@valor_portes", SqlDbType.Decimal);
                    outputParamPortes.Direction = ParameterDirection.Output;
                    outputParamPortes.Precision = 18;
                    outputParamPortes.Scale = 2;
                    cmd.Parameters.Add(outputParamPortes);

                    cmd.ExecuteNonQuery();

                    //globalClass.peso_compra = (decimal)outputParamPeso.Value;
                    UpdateValores();
                    globalClass.portes_compra = (decimal)outputParamPortes.Value;
                    
                    lbl_portes.Text = ((decimal)outputParamPortes.Value).ToString();
                }
            }
            UpdatePrices.Update();
        }

        public void UpdateValores()
        {
            if(globalClass.desconto >  0)
                lbl_desconto.Text = $"{totalDesconto.ToString("0.00")}€ ({((int)(globalClass.desconto)).ToString()}%)";

            globalClass.valorFinal = globalClass.valor_produtos + globalClass.portes_compra;


            lbl_valorTotal.Text = globalClass.valorFinal.ToString("0.00");
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (globalClass.type_user == "Revendedor")
                globalClass.desconto = 20;

            if (globalClass.cod_user == null)
            {
                Response.Redirect("sighIn.aspx");
            }
            else
            {
                UpdateCarrinho();

                if (!globalClass.nome_user.Contains("User"))
                {
                    tb_nome.Text = globalClass.nome_user;
                }
                if (globalClass.telemovel != null)
                {
                    tb_contacto.Text = globalClass.telemovel;
                }
                if (globalClass.nif != null)
                {
                    tb_nif.Text = globalClass.nif;
                }

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
            UpdateValores();
        }


        protected void editProduto(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int cod_produto = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    mycon.Open();
                    using (SqlCommand mycmd = new SqlCommand("remover_carrinho", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.AddWithValue("@cod_produto", cod_produto);
                        mycmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);

                        mycmd.ExecuteNonQuery();
                    }
                }
            }
            else if(e.CommandName == "sub" || e.CommandName == "plus")
            {
                int cod_produto = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    mycon.Open();
                    using (SqlCommand mycmd = new SqlCommand("edit_carrinho", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.AddWithValue("@cod_produto", cod_produto);
                        mycmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);
                        mycmd.Parameters.AddWithValue("@func", e.CommandName);

                        mycmd.ExecuteNonQuery();
                    }
                }


            }

            UpdateCarrinho();
            UpdateCart.Update();
        }



        protected void btn_checkout_Click(object sender, EventArgs e)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString);

            byte[] pdfData = GerarPDFPedido(int.Parse(globalClass.cod_user), myConn);

            EnviarPDFPorEmail(pdfData, int.Parse(globalClass.cod_user));

            Response.Redirect("userPage.aspx");
        }


        private byte[] GerarPDFPedido(int idPedido, SqlConnection myConn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (iTextSharp.text.Document document = new iTextSharp.text.Document())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);


                    document.AddTitle("Detalhes do Pedido");
                    document.AddAuthor("TechTudo");

                    document.Open();


                    PdfPTable headerTable = new PdfPTable(1);
                    headerTable.WidthPercentage = 100;
                    PdfPCell headerCell = new PdfPCell(new Phrase("TechTudo", new Font(Font.FontFamily.HELVETICA, 24, Font.BOLD)));
                    headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerCell.Border = PdfPCell.NO_BORDER;
                    headerTable.AddCell(headerCell);
                    document.Add(headerTable);


                    document.Add(new Paragraph("\n"));


                    PdfPTable table = new PdfPTable(2);
                    table.WidthPercentage = 100;
                    table.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;


                    PdfPCell cell = new PdfPCell(new Phrase("Detalhes do Pedido", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD)));
                    cell.Colspan = 2;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("ID do Pedido: " + idPedido, new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL)));
                    cell.Colspan = 2;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);


                    document.Add(new Paragraph("\n"));

                    document.Add(new Paragraph("\n"));

                    using (SqlConnection mycon = new SqlConnection(conString))
                    {
                        mycon.Open();
                        using (SqlCommand mycmd = new SqlCommand("procurar_carrinho", mycon))
                        {
                            mycmd.CommandType = CommandType.StoredProcedure;
                            mycmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);

                            using (SqlDataReader dr = mycmd.ExecuteReader())
                            {
                                int itemsTotal = 0;

                                var produto = new Produto();


                                cell = new PdfPCell(new Phrase("Cliente:", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
                                cell.Colspan = 2;
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                table.AddCell(cell);

                                table.AddCell("Nome:");
                                table.AddCell(tb_nome.Text);

                                table.AddCell("Email:");
                                table.AddCell(globalClass.email);

                                table.AddCell("Telemovel:");
                                table.AddCell(tb_contacto.Text);

                                table.AddCell("NIF:");
                                table.AddCell(tb_nif.Text);

                                table.AddCell(new PdfPCell(new Phrase(" ")) { Colspan = 2, Border = PdfPCell.NO_BORDER });


                                cell = new PdfPCell(new Phrase("Destino:", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
                                cell.Colspan = 2;
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                table.AddCell(cell);

                                table.AddCell("Morada:");
                                table.AddCell($"{tb_morada.Text} - {tb_porta.Text}");

                                table.AddCell("Cidade:");
                                table.AddCell(tb_cidade.Text);

                                table.AddCell("Código postal:");
                                table.AddCell(tb_postal.Text);

                                table.AddCell(new PdfPCell(new Phrase(" ")) { Colspan = 2, Border = PdfPCell.NO_BORDER });

                                while (dr.Read())
                                {
                                    itemsTotal++;

                                    cell = new PdfPCell(new Phrase("Produto:", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
                                    cell.Colspan = 2;
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.NO_BORDER;
                                    table.AddCell(cell);

                                    table.AddCell("Categoria:");
                                    table.AddCell(dr.GetString(4).ToString());

                                    table.AddCell("Produto:");
                                    table.AddCell(dr.GetString(1).ToString());

                                    table.AddCell("Preco Produto:");
                                    table.AddCell(dr.GetDecimal(2).ToString());

                                    table.AddCell("Quantidade:");
                                    table.AddCell(dr.GetInt32(3).ToString());

                                    document.Add(new Paragraph(" "));
                                }

                                cell = new PdfPCell(new Phrase($"Preço Total: {globalClass.valor_produtos.ToString()}€", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
                                cell.Colspan = 2;
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell.Border = PdfPCell.NO_BORDER;
                                table.AddCell(cell);
                            }
                        }
                    }

                    document.Add(table);
                    document.Close();
                    writer.Close();
                }

                return ms.ToArray();
            }
        }

        private void EnviarPDFPorEmail(byte[] pdfData, int idPedido)
        {
            string destinatario = globalClass.email;
            string assunto = "Pedido #" + idPedido.ToString();
            string corpoEmail = "Por favor, encontre em anexo o PDF do seu pedido.";

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

                    MemoryStream ms = new MemoryStream(pdfData);
                    mailMessage.Attachments.Add(new Attachment(ms, "Pedido" + idPedido + ".pdf"));

                    servidor.Credentials = new NetworkCredential(smtpUtilizador, smtpPassword);
                    servidor.EnableSsl = true;

                    servidor.Send(mailMessage);
                }
            }

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
                    cmd.Parameters.AddWithValue("@contacto", tb_contacto.Text);
                    cmd.Parameters.AddWithValue("@nif", tb_nif.Text);

                    cmd.ExecuteNonQuery();
                }
            }


            using (SqlConnection conn = new SqlConnection(conString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("criar_pedido", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cod_user", globalClass.cod_user);

                    if(rb_mastercard.Checked)
                        cmd.Parameters.AddWithValue("@metodo", "Masterard");
                    else if (rb_payapl.Checked)
                        cmd.Parameters.AddWithValue("@metodo", "PayPal");
                    else if (rb_visa.Checked)
                        cmd.Parameters.AddWithValue("@metodo", "Visa");

                    cmd.ExecuteNonQuery();
                }
            }
        }



        protected void btn_aplicar_Click(object sender, EventArgs e)
        {
            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                using (SqlCommand mycmd = new SqlCommand("verificar_cupon", mycon))
                {
                    mycmd.CommandType = CommandType.StoredProcedure;
                    mycmd.Parameters.AddWithValue("@code", tb_code.Text);

                    SqlParameter valor = new SqlParameter
                    {
                        ParameterName = "@retorno_desconto",
                        Direction = ParameterDirection.Output,
                        SqlDbType = SqlDbType.Int
                    };

                    SqlParameter valor2 = new SqlParameter
                    {
                        ParameterName = "@retorno_categoria",
                        Direction = ParameterDirection.Output,
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50
                    };

                    mycmd.Parameters.Add(valor);

                    mycmd.Parameters.Add(valor2);

                    mycmd.ExecuteNonQuery();


                    int retorno_desconto = Convert.ToInt32(mycmd.Parameters["@retorno_desconto"].Value);

                    string retorno_categoria = mycmd.Parameters["@retorno_categoria"].Value.ToString();

                    if (retorno_desconto != 0)
                    {
                        cv_code.IsValid = true;

                        globalClass.desconto = retorno_desconto;
                        typeDesconto = retorno_categoria;

                        UpdateCarrinho();
                    }
                    else if(retorno_desconto == 0)
                    {
                        globalClass.desconto = retorno_desconto;
                        typeDesconto = "";
                        cv_code.IsValid = false;
                    }
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
            public int quantidade { get; set; }
        }

    }
}