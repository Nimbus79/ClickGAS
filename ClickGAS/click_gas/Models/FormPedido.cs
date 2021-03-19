using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace click_gas.Models
{
    public class FormPedido
    {
        public string propano45 { get; set; }

        public string propano11 { get; set; }

        public string propano5 { get; set; }

        public string carburante11 { get; set; }

        public string butano13 { get; set; }

        public string butano6 { get; set; }

        public int n45 { get; set; }

        public int n11 { get; set; }

        public int n5 { get; set; }

        public int nc11 { get; set; }

        public int n13 { get; set; }

        public int n6 { get; set; }

        public string mangueiras { get; set; }

        public string redutores { get; set; }

        public string valvulas { get; set; }

        public string tubos { get; set; }

        public string sistema { get; set; }

        public string ligacoes { get; set; }

        public string observacoes { get; set; }

        public DateTime data { get; set; }

        public string hora { get; set; }
    }
}