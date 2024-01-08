using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.WebPages;

namespace TechTudo
{
    public partial class adminProducts : System.Web.UI.Page
    {
        string conString = ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString;

        public static IDictionary<string, string> dic_categorias = new Dictionary<string, string>();


        List<string> listaChaves = new List<string>();

        public static List<string> listKeys = new List<string>();


        public static List<byte[]> imageBytesList = new List<byte[]>();

        public static List<byte[]> imageBytesList_update = new List<byte[]>();

        public void request_Imgs(List<byte[]> BytesList, FileUpload fileUpload)
        {
            foreach (HttpPostedFile file in fileUpload.PostedFiles)
            {
                if (file.ContentLength > 0)
                {
                    using (BinaryReader reader = new BinaryReader(file.InputStream))
                    {
                        byte[] imageBytes = reader.ReadBytes(file.ContentLength);
                        BytesList.Add(imageBytes);
                    }
                }
            }
        }

        public void procurar_produto(int cod_produto)
        {
            imageBytesList_update.Clear();

            string caract = "";

            if (string.IsNullOrEmpty(cod_produto.ToString()))
            {
                lbl_error_search.Text = "O campo de ID do produto está vazio.";
                lbl_error_search.Style["color"] = "red";
            }
            else
            {
                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    mycon.Open();
                    using (SqlCommand mycmd = new SqlCommand("procurar_produto", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.Add(new SqlParameter("@cod", SqlDbType.Int));
                        mycmd.Parameters["@cod"].Value = cod_produto;

                        using (SqlDataReader dr = mycmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                tb_nome_update.Text = dr.GetString(0);
                                caract = dr.GetString(1);
                                tb_stock_update.Text = dr.GetInt32(2).ToString();
                                tb_price_update.Text = dr.GetDecimal(3).ToString();

                                if (!dr.IsDBNull(4))
                                {
                                    byte[] imgBytes = (byte[])dr[4];
                                    string base64String = Convert.ToBase64String(imgBytes);
                                    img_principal_update.ImageUrl = "data:image/png;base64," + base64String;
                                }
                                ddl_ativacao.SelectedValue = dr.GetBoolean(5).ToString();

                                if (dr.GetBoolean(5).ToString() == "False")
                                    ddl_ativacao.Style["background-color"] = "#c0392b;";
                                else if (dr.GetBoolean(5).ToString() == "True")
                                    ddl_ativacao.Style["background-color"] = "rgb(5, 164, 24);";
                            }

                            if (!dr.HasRows)
                            {
                                lbl_error_search.Text = "Produto não encontrado";
                                lbl_error_search.Style["fore-color"] = "red";
                            }
                        }
                    }

                    using (SqlCommand command = new SqlCommand("procurar_imagensProdutos", mycon))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@cod_produto", cod_produto);

                        using (SqlDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (!dr.IsDBNull(0))
                                {
                                    imageBytesList_update.Add(dr.GetSqlBytes(0).Value);
                                }
                            }
                        }
                    }
                    tb_idProduto.Text = cod_produto.ToString();
                }
            }


            List<KeyValuePair<string, string>> dataDictionary = new List<KeyValuePair<string, string>>();
            MatchCollection matches = Regex.Matches(caract, "%%(.*?): (.*?)%%");

            string chave = "";
            string valo = "";

            foreach (Match match in matches)
            {
                chave = match.Groups[1].Value;
                valo = match.Groups[2].Value;

                listKeys.Add(chave);
                dataDictionary.Add(new KeyValuePair<string, string>(chave, valo));
            }

            rpt_update.DataSource = dataDictionary;
            rpt_update.DataBind();

            rpt_imgs_update.DataSource = imageBytesList_update;
            rpt_imgs_update.DataBind();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (globalClass.cod_user == null || globalClass.type_user != "Admin")
                Response.Redirect("sighIn.aspx");
        }

        protected void ddl_categoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            dic_categorias.Clear();

            using (SqlConnection connection = new SqlConnection(conString))
            {
                using (SqlCommand command = new SqlCommand("formato_produtos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@cod", ddl_categoria.SelectedValue);

                    SqlParameter retorno = new SqlParameter
                    {
                        ParameterName = "@retorno",
                        Direction = ParameterDirection.Output,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = -1
                    };

                    command.Parameters.Add(retorno);

                    connection.Open();
                    command.ExecuteNonQuery();

                    string resposta = retorno.Value.ToString();

                    if (!string.IsNullOrEmpty(resposta))
                    {
                        MatchCollection matches = Regex.Matches(resposta, "%%(.*?): (.*?)%%");

                        string chave = "";
                        string valo = "";

                        foreach (Match match in matches)
                        {
                            chave = match.Groups[1].Value;
                            valo = match.Groups[2].Value;

                            dic_categorias.Add(chave, valo);
                            listaChaves.Add(chave);
                        }
                    }
                }
            }

            rpt_caracteristicas.DataSource = listaChaves;
            rpt_caracteristicas.DataBind();
        }

        protected void btn_adicionar_Click(object sender, EventArgs e)
        {
            string caracteristicas = "";

            for (int i = 0; i < rpt_caracteristicas.Items.Count; i++)
            {
                caracteristicas += $"%%{dic_categorias.Keys.ElementAt(i)}: ";

                TextBox tb_caract = rpt_caracteristicas.Items[i].FindControl("tb_caract") as TextBox;

                if (tb_caract != null)
                {
                    string valo = tb_caract.Text;
                    caracteristicas += $"{valo}%% ";
                }
                else
                    caracteristicas += $" %% ";
            }

            if (string.IsNullOrEmpty(tb_nome.Text) ||
                string.IsNullOrEmpty(tb_preco.Text) ||
                string.IsNullOrEmpty(tb_stock.Text) ||
                !Regex.IsMatch(tb_preco.Text, @"^\d+([,.]\d+)?$"))
            {
                btn_adicionar.Style["background"] = "linear-gradient(to right, #d50c0c 0%, #a40505 99%)";
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("adicionar_produtos", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@nome_produto", tb_nome.Text);
                        command.Parameters.AddWithValue("@cod_categoria", ddl_categoria.SelectedValue);
                        command.Parameters.AddWithValue("@caracteristicas", caracteristicas);
                        command.Parameters.AddWithValue("@stock", tb_stock.Text);
                        command.Parameters.AddWithValue("@price", tb_preco.Text.Replace(',', '.'));
                        if (img_principal.ImageUrl != null)
                        {
                            string base64Data = img_principal.ImageUrl.Split(',')[1];

                            // Converte a string base64 em bytes
                            byte[] imgBytes = Convert.FromBase64String(base64Data);

                            command.Parameters.AddWithValue("@imgBytes", imgBytes);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@imgBytes", null);
                        }


                        SqlParameter retorno = new SqlParameter
                        {
                            ParameterName = "@retorno",
                            Direction = ParameterDirection.Output,
                            SqlDbType = SqlDbType.Int
                        };
                        command.Parameters.Add(retorno);


                        command.ExecuteNonQuery();

                        int resposta = Convert.ToInt32(retorno.Value);

                        
                        if (resposta == 0)
                        {
                            btn_adicionar.Style["background"] = "linear-gradient(to right, #d50c0c 0%, #a40505 99%)";
                        }
                        else
                        {
                            procurar_produto(resposta);
                            btn_adicionar.Style["background"] = "linear-gradient(to right, #31d50c 0%, #05a418 99%)";
                        }
                    }

                    if (imageBytesList.Count > 0)
                    {
                        foreach (byte[] img in imageBytesList)
                        {
                            using (SqlCommand command = new SqlCommand("adicionar_imagensProdutos", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@ImageBytes", img);
                                command.Parameters.AddWithValue("@nome_produto", tb_nome.Text);

                                command.ExecuteNonQuery();
                            }
                        }
                    }

                }
            }

            listaChaves.Clear();
            dic_categorias.Clear();
            UpdateAdicionar.Update();

        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if(tb_idProduto.Text != "")
                procurar_produto(int.Parse(tb_idProduto.Text));
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            string caracteristicas = "";

            for (int i = 0; i < rpt_update.Items.Count; i++)
            {
                caracteristicas += $"%%{listKeys[i]}: {((TextBox)rpt_update.Items[i].FindControl("tb_caract_update")).Text}%% ";
            }


            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("atualizar_produto", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@cod_produto", tb_idProduto.Text);
                    command.Parameters.AddWithValue("@nome_produto", tb_nome_update.Text);
                    command.Parameters.AddWithValue("@caracteristicas", caracteristicas);
                    command.Parameters.AddWithValue("@stock", tb_stock_update.Text);
                    command.Parameters.AddWithValue("@price", tb_price_update.Text.Replace(',', '.'));

                    if (img_principal_update.ImageUrl != "")
                    {
                        string base64Data = img_principal_update.ImageUrl.Split(',')[1];

                        byte[] imgBytes = Convert.FromBase64String(base64Data);

                        command.Parameters.AddWithValue("@img_principal", imgBytes);
                    }
                    else
                    {
                        byte[] imageBytes = File.ReadAllBytes(Server.MapPath("/img/default.png"));

                        command.Parameters.AddWithValue("@img_principal", imageBytes);
                    }

                    command.Parameters.AddWithValue("@ativo", ddl_ativacao.SelectedValue);


                    command.ExecuteNonQuery();
                }

                if (imageBytesList_update.Count > 0)
                {
                    bool delete = true;
                    foreach (var imageBytes in imageBytesList_update)
                    {
                        using (SqlCommand command = new SqlCommand("atualizar_imagenProdutos", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ImageBytes", imageBytes);
                            command.Parameters.AddWithValue("@cod_produto", tb_idProduto.Text);
                            command.Parameters.AddWithValue("@delete", delete);

                            command.ExecuteNonQuery();
                        }
                        delete = false;
                    }
                }

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (fileUpload.HasFiles)
            {
                imageBytesList.Clear();
                request_Imgs(imageBytesList, fileUpload);

                repeaterFiles.DataSource = imageBytesList;
                repeaterFiles.DataBind();
            }
        }

        protected void btn_imgs_update_Click(object sender, EventArgs e)
        {
            if (fileUpload_update.HasFiles)
            {
                imageBytesList_update.Clear();
                request_Imgs(imageBytesList_update, fileUpload_update);

                rpt_imgs_update.DataSource = imageBytesList_update;
                rpt_imgs_update.DataBind();
            }
        }

        protected void btn_imgPrincipal_Click(object sender, EventArgs e)
        {
            if (fileUpload_principal.HasFiles)
            {
                img_principal.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(fileUpload_principal.FileBytes);
            }
        }

        protected void img_principal_Click(object sender, ImageClickEventArgs e)
        {
            img_principal.ImageUrl = null;
        }

        protected void btn_imgPrincipal_update_Click(object sender, EventArgs e)
        {

            if (fileUpload_principalUpdate.HasFiles)
            {
                img_principal_update.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(fileUpload_principalUpdate.FileBytes);
            }
        }

        protected void img_principal_update_Click(object sender, ImageClickEventArgs e)
        {
            img_principal_update.ImageUrl = null;
        }
    }
}