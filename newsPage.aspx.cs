using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace TechTudo
{
    public partial class newsPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Criação de uma nova instância do XmlDocument
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                // Carregar o XML do feed RSS
                xmlDoc.Load("https://www.noticiasaominuto.com/rss/tech");

                // Define o documento XML no controle Xml1
                Xml1.Document = xmlDoc;
            }
            catch (Exception ex)
            {
                // Lidar com exceções (ex.: problemas de conexão, formato de XML incorreto, etc.)
                // Exiba a mensagem de erro ou faça o que for adequado para o seu aplicativo
                Response.Write("Erro: " + ex.Message);
            }
        }
    }
}