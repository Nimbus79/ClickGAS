using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace click_gas.Models
{
    public class ServicoSel
    {
        public ServicoSel(string s, int q)
        {
            servico = s;
            quantidade = q;
        }

        public string servico { get; set; }
        public int quantidade { get; set; }
    }
}