using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using click_gas.Models;

namespace click_gas.Controllers
{
    public class FuncionarioController : Controller
    {
        GasContext db = new GasContext();
        // GET: Funcionario
        public ActionResult pendentes()
        {
            string idLogin = "";
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                if (!string.IsNullOrEmpty(authCookie.Value))
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    idLogin = authTicket.Name.ToString();
                }
            }
            var pends = (from m in db.Pedidoes where (m.emailFuncionario == idLogin && m.estado == false) select m);
            return View(pends);
        }

        [HttpPost]
        public ActionResult consultar(int idpedido, string emailCliente)
        {
            var pedidos = (from m in db.Pedidoes where (m.id == idpedido) select m);
            Pedido pedido = pedidos.ToList<Pedido>().ElementAt<Pedido>(0);
            var clientes = (from m in db.Clientes where (m.email == emailCliente) select m);
            Cliente cliente = clientes.ToList<Cliente>().ElementAt<Cliente>(0);
            ServicosPedido servped = new ServicosPedido();
            servped.nomeCliente = cliente.nome;
            servped.contactoCliente = cliente.contacto;
            servped.rua = cliente.rua;
            servped.numero = cliente.numero;
            servped.freguesia = cliente.freguesia;
            servped.concelho = cliente.concelho;
            servped.codPostal = cliente.codPostal;
            servped.data = pedido.data;
            servped.hora = pedido.hora;
            servped.observacoes = pedido.observacoes;
            var pends = (from m in db.Pedido_tem_Servico where (m.idPedido == idpedido) select m);
            foreach (Pedido_tem_Servico p in pends)
            {
                var servs = (from m in db.Servicoes where (m.id == p.idServico) select m);
                Servico serv = servs.ToList<Servico>().ElementAt<Servico>(0);
                string desc = serv.tipo + " " + serv.descricao;
                ServicoSel s = new ServicoSel(desc, p.quantidade);
                servped.servicos.Add(s);
            }

            ViewBag.Pedido = idpedido;
            return View(servped);
        }

        [HttpPost]
        public ActionResult Validar(int idPedido)
        {
            ViewBag.Pedido = idPedido;
            return View("Validar");
        }

        [HttpPost]
        public ActionResult ValidarPedido([Bind(Include = "propano45,propano11,propano5,carburante11,butano13,butano6,n45,n11,n5,nc11,n13,n6,data,hora,observacoes,mangueiras,redutores,valvulas,tubos,sistema,ligacoes")] FormPedido formPedido, int idPedido, string duracao, int nm, int nr, int nv, int nt, int ns, int nl)
        {
            IEnumerable<Servico> servicos;
            List<ServicoFatura> servs = new List<ServicoFatura>();
            double custo = 0;

            IEnumerable<Pedido> pedidos = (from m in db.Pedidoes where m.id == idPedido select m);
            Pedido pedido = pedidos.ToList<Pedido>().ElementAt<Pedido>(0);

            pedido.observacoes = formPedido.observacoes;

            IEnumerable<Cliente> clientes = (from m in db.Clientes where m.email == pedido.emailCliente select m);
            Cliente cliente = clientes.ToList<Cliente>().ElementAt<Cliente>(0);

            if (formPedido.propano45 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 1 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    custo += (servico.preco * formPedido.n45);
                    servs.Add(new ServicoFatura(servico.descricao, servico.preco * formPedido.n45));
                }
            }
            if (formPedido.propano11 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 2 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    custo += (servico.preco * formPedido.n11);
                    servs.Add(new ServicoFatura(servico.descricao, servico.preco * formPedido.n11));
                }
            }
            if (formPedido.propano5 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 3 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    custo += (servico.preco * formPedido.n5);
                    servs.Add(new ServicoFatura(servico.descricao, servico.preco * formPedido.n5));
                }
            }
            if (formPedido.carburante11 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 4 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    custo += (servico.preco * formPedido.nc11);
                    servs.Add(new ServicoFatura(servico.descricao, servico.preco * formPedido.nc11));
                }
            }
            if (formPedido.butano13 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 5 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    custo += (servico.preco * formPedido.n13);
                    servs.Add(new ServicoFatura(servico.descricao, servico.preco * formPedido.n13));
                }
            }
            if (formPedido.butano6 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 6 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    duracao += (servico.duracaoMedia * formPedido.n6);
                    custo += (servico.preco * formPedido.n6);
                    servs.Add(new ServicoFatura(servico.descricao, servico.preco * formPedido.n6));
                }
            }
            if (formPedido.mangueiras == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 7 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    custo += servico.preco * nm;
                    servs.Add(new ServicoFatura(servico.descricao, servico.preco * nm));
                }
            }
            if (formPedido.redutores == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 8 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    custo += servico.preco * nr;
                    servs.Add(new ServicoFatura(servico.descricao, servico.preco * nr));
                }
            }
            if (formPedido.valvulas == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 9 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    custo += servico.preco * nv;
                    servs.Add(new ServicoFatura(servico.descricao, servico.preco * nv));
                }
            }
            if (formPedido.tubos == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 10 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    custo += servico.preco * nt;
                    servs.Add(new ServicoFatura(servico.descricao, servico.preco * nt));
                }
            }
            if (formPedido.sistema == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 11 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    custo += servico.preco * ns;
                    servs.Add(new ServicoFatura(servico.descricao, servico.preco * ns));
                }
            }
            if (formPedido.ligacoes == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 12 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    custo += servico.preco * nl;
                    servs.Add(new ServicoFatura(servico.descricao, servico.preco * nl));
                }
            }

            IEnumerable<Funcionario> funcionarios = (from m in db.Funcionarios where m.email == pedido.emailFuncionario select m);
            Funcionario func = funcionarios.ToList<Funcionario>().ElementAt<Funcionario>(0);

            Time time = new Time(duracao);
            float d = time.GetTimeFloat();
            double custoTotal = custo + (d * func.salario);
            
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new GasContext())
                    {
                        var result = db.Pedidoes.SingleOrDefault(p => p.id == idPedido);
                        if (result != null)
                        {
                            result.duracaoReal = d;
                            result.custoReal = custoTotal;
                            result.estado = true;
                            db.SaveChanges();
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            Fatura fatura = new Fatura(pedido.emailCliente, pedido.id, pedido.data.ToString("dd/MM/yyyy"), cliente.contribuinte, custoTotal, d * func.salario);
            string msg = "Caro Cliente,\nInformamos que a fatura relativa ao servico prestado a " + pedido.data.ToString("dd/MM/yyyy") + " possui a referencia nr "+ pedido.id +".\n Cumprimentos,\nClickGAS";
            string contacto = "351" + cliente.contacto;
            new SmsController().Send(contacto, msg);
            TempData["Servicos"] = servs;
            return RedirectToAction("ValidarPedido", "Email", fatura);
        }
    }
}