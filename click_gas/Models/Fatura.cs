using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace click_gas.Models
{
    public class Fatura
    {
        public Fatura()
        {

        }

        public Fatura(string eC, int r, string d, int c, double cT, double mO)
        {
            emailCliente = eC;
            referencia = r;
            dataEmissao = d;
            contribuinte = c;
            custoTotal = cT;
            maoDeObra = mO;
        }

        public string emailCliente { get; set; }
        public int referencia { get; set; }
        public string dataEmissao { get; set; }
        public int contribuinte { get; set; }
        public double custoTotal { get; set; }
        public double maoDeObra { get; set; }
    }
}