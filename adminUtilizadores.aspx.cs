using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Drawing;
using static iTextSharp.text.pdf.AcroFields;

namespace TechTudo
{
    public partial class adminUtilizadores : System.Web.UI.Page
    {
        string conString = ConfigurationManager.ConnectionStrings["TechTudoConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (globalClass.cod_user == null || globalClass.type_user != "Admin")
            {
                Response.Redirect("sighIn.aspx");
            }
            else
            {
                refreshUsers();
            }
        }


        public void refreshUsers()
        {
            List<user> lst_userInfo = new List<user>();

            using (SqlConnection mycon = new SqlConnection(conString))
            {
                using (SqlCommand mycmd = new SqlCommand("procurar_utilizadores", mycon))
                {
                    mycon.Open();

                    using (SqlDataReader dr = mycmd.ExecuteReader())
                    {
                        var user = new user();

                        while (dr.Read())
                        {
                            user = new user();

                            user.id = dr.GetInt32(0).ToString();
                            user.nome = dr.GetString(1);
                            user.email = dr.GetString(2);
                            user.type = dr.GetString(3);
                            user.data = dr.GetDateTime(4).ToString("dd/MM/yyyy");
                            user.ativo = dr.GetBoolean(5);

                            lst_userInfo.Add(user);
                        }
                    }
                }
            }

            rpt_users.DataSource = lst_userInfo;
            rpt_users.DataBind();
            lst_userInfo.Clear();
        }


        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (tb_idUser.Text != "")
            {
                List<user> lst_userInfo = new List<user>();

                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    using (SqlCommand mycmd = new SqlCommand("Find_users", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.AddWithValue("@parametroBusca", tb_idUser.Text);

                        mycon.Open();

                        using (SqlDataReader dr = mycmd.ExecuteReader())
                        {
                            var user = new user();

                            while (dr.Read())
                            {
                                user = new user();

                                user.id = dr.GetInt32(0).ToString();
                                user.nome = dr.GetString(1);
                                user.email = dr.GetString(2);
                                user.type = dr.GetString(3);
                                user.data = dr.GetDateTime(4).ToString("dd/MM/yyyy");
                                user.ativo = dr.GetBoolean(5);

                                lst_userInfo.Add(user);
                            }
                        }
                    }
                }

                rpt_users.DataSource = lst_userInfo;
                rpt_users.DataBind();
                lst_userInfo.Clear();
            }
            else
                Response.Redirect(Request.RawUrl);

        }


        protected void editUser(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "desativar")
            {
                int cod_user = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    mycon.Open();
                    using (SqlCommand mycmd = new SqlCommand("ativacaoUser", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.AddWithValue("@cod_user", cod_user);
                        mycmd.Parameters.AddWithValue("@func", 0);

                        mycmd.ExecuteNonQuery();
                    }
                }
                refreshUsers();
            }
            else if (e.CommandName == "ativar")
            {
                int cod_user = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    mycon.Open();
                    using (SqlCommand mycmd = new SqlCommand("ativacaoUser", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.AddWithValue("@cod_user", cod_user);
                        mycmd.Parameters.AddWithValue("@func", 1);

                        mycmd.ExecuteNonQuery();
                    }
                }
                refreshUsers();
            }
            else if(e.CommandName == "save")
            {
                int cod_user = Convert.ToInt32(e.CommandArgument);

                RepeaterItem item = (sender as Control).NamingContainer as RepeaterItem;

                using (SqlConnection mycon = new SqlConnection(conString))
                {
                    mycon.Open();
                    using (SqlCommand mycmd = new SqlCommand("editar_user", mycon))
                    {
                        mycmd.CommandType = CommandType.StoredProcedure;
                        mycmd.Parameters.AddWithValue("@cod_user", cod_user);
                        mycmd.Parameters.AddWithValue("@email", (item.FindControl("tb_email") as TextBox).Text);
                        mycmd.Parameters.AddWithValue("@type", (item.FindControl("tb_type") as TextBox).Text);
                        mycmd.Parameters.AddWithValue("@nome", (item.FindControl("tb_nome") as TextBox).Text);

                        mycmd.ExecuteNonQuery();
                    }
                }
                refreshUsers();
            }
        }



        public class user
        {
            public string id { get; set; }
            public string nome { get; set; }
            public string email { get; set; }
            public string type { get; set; }
            public string data { get; set; }
            public bool ativo { get; set; }
        }

    }
}