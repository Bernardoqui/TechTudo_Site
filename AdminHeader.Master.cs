using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TechTudo
{
    public partial class AdminHeader : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (globalClass.cod_user != null)
            {
                lbl_nome.Text = globalClass.nome_user.ToString();
                lbl_email.Text = globalClass.email.ToString();
            }
        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {
            globalClass.LimparDados();

            Response.Redirect("homePage.aspx");
        }
    }
}