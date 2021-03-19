using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace click_gas.Models
{
    public class ServicosPedido
    {
        public ServicosPedido()
        {
            servicos = new HashSet<ServicoSel>();
        }

        public int idPedido { get; set; }

        public string nomeCliente { get; set; }

        public int contactoCliente { get; set; }

        public string rua { get; set; }

        public int numero { get; set; }

        public string freguesia { get; set; }

        public string concelho { get; set; }

        public string codPostal { get; set; }

        public DateTime data { get; set; }

        public double hora { get; set; }

        public string observacoes { get; set; }

        public ICollection<ServicoSel> servicos { get; set; }
    }
}