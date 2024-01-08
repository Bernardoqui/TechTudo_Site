using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechTudo
{
    public class globalClass
    {
        public static List<int> carrinho = new List<int>();

        //USER INFO//

        public static bool ativo = false;

        public static string cod_user = null;

        public static string nome_user = null;

        public static string email = null;

        public static string type_user = null;

        public static string telemovel = null;

        public static string nif = null;


        /////////////

        //shipping info//

        public static string morada = null;

        public static string porta_andar = null;

        public static string cidade = null;

        public static string cod_postal = null;



        public static decimal peso_compra = 0;

        public static decimal portes_compra = 0;

        public static decimal desconto = 0;

        public static decimal valor_produtos = 0;

        public static decimal valorFinal = 0;



        public static void LimparDados()
        {
            carrinho.Clear();

            ativo = false;
            cod_user = null;
            nome_user = null;
            email = null;
            type_user = null;
            telemovel = null;
            nif = null;

            morada = null;
            porta_andar = null;
            cidade = null;
            cod_postal = null;

            peso_compra = 0;
            portes_compra = 0;
            desconto = 0;
            valor_produtos = 0;
            valorFinal = 0;
        }
    }
}