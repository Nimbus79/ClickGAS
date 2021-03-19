using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace click_gas.Models
{
    public class Email
    {
        public Email() { }

        public Email(string eC, string msg)
        {
            emailCliente = eC;
            mensagem = msg;
        }

        public Email(string eC, string eF, string h, string d)
        {
            emailCliente = eC;
            emailFuncionario = eF;
            data = d;
            hora = h;
        }

        public string emailCliente { get; set; }
        public string emailFuncionario { get; set; }
        public string hora { get; set; }
        public string data { get; set; }
        public string mensagem { get; set; }
    }
}