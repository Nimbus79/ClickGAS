using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace click_gas.Models
{
    public class ServicoFatura
    {
        public ServicoFatura(string desc, double cus)
        {
            descricao = desc;
            custo = cus;
        }

        public string descricao { get; set; }
        public double custo { get; set; }
    }
}