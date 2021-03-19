using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using click_gas.Models;

namespace click_gas.Controllers
{
    //[Authorize(Roles="user")]
    public class ClienteController : Controller
    {
        GasContext db = new GasContext();

        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Solicitar()
        {
            return View("Solicitar");
        }

        public ActionResult Contactos()
        {
            return View("Contactos");
        }

        public ActionResult VerPerfil()
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
            var clientes = (from m in db.Clientes where (m.email == idLogin) select m);
            if (clientes.ToList<Cliente>().Count > 0)
            {
                Cliente cliente = clientes.ToList<Cliente>().ElementAt<Cliente>(0);
                ViewBag.Nome = cliente.nome;
                ViewBag.Email = idLogin;
                ViewBag.Contribuinte = cliente.contribuinte;
                ViewBag.Contacto = cliente.contacto;
                ViewBag.DataNasc = cliente.dataNasc.ToString("dd/MM/yyyy");
                ViewBag.Morada = cliente.rua;
                ViewBag.Numero = cliente.numero;
                ViewBag.CodPostal = cliente.codPostal;
                ViewBag.Freguesia = cliente.freguesia;
                ViewBag.Concelho = cliente.concelho;
                return View();
            }

            return View("Index", "Home");
        }

        public ActionResult EditarPerfil()
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
            var clientes = (from m in db.Clientes where (m.email == idLogin) select m);
            if (clientes.ToList<Cliente>().Count > 0)
            {
                Cliente cliente = clientes.ToList<Cliente>().ElementAt<Cliente>(0);
                ViewBag.Nome = cliente.nome;
                ViewBag.Email = idLogin;
                ViewBag.Contribuinte = cliente.contribuinte;
                ViewBag.Contacto = cliente.contacto;
                ViewBag.DataNasc = cliente.dataNasc.ToString("dd/MM/yyyy");
                ViewBag.Morada = cliente.rua;
                ViewBag.Numero = cliente.numero;
                ViewBag.CodPostal = cliente.codPostal;
                ViewBag.Freguesia = cliente.freguesia;
                ViewBag.Concelho = cliente.concelho;
                return View();
            }

            return View("Index", "Home");
        }

        [HttpPost]
        public ActionResult AtualizarDados(string email, DateTime dataNasc, int contribuinte, string nome, int contacto, string morada, int numero, string codPostal, string freguesia, string concelho)
        {
            var clientes = (from m in db.Clientes where (m.email == email) select m);

            try
            {
                using (var db = new GasContext())
                {
                    var result = db.Clientes.SingleOrDefault(c => c.email == email);
                    if (result != null)
                    {
                        result.nome = nome;
                        result.contacto = contacto;
                        result.rua = morada;
                        result.numero = numero;
                        result.codPostal = codPostal;
                        result.freguesia = freguesia;
                        result.concelho = concelho;
                        db.SaveChanges();
                    }
                }
            } catch(DbUpdateException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            ViewBag.Nome = nome;
            ViewBag.Email = email;
            ViewBag.Contribuinte = contribuinte;
            ViewBag.Contacto = contacto;
            ViewBag.DataNasc = dataNasc.ToString("dd/MM/yyyy");
            ViewBag.Morada = morada;
            ViewBag.Numero = numero;
            ViewBag.CodPostal = codPostal;
            ViewBag.Freguesia = freguesia;
            ViewBag.Concelho = concelho;

            return View("VerPerfil");
        }

        public ActionResult Catalogo()
        {
            return View("Catalogo");
        }

        private string getEmailCliente()
        {
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                if (!string.IsNullOrEmpty(authCookie.Value))
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    return authTicket.Name.ToString();
                }
            }

            return "";
        }

        private Funcionario escolheFuncionario()
        {
            var funcionarios = (from m in db.Funcionarios select m);
            int b = funcionarios.Count();
            Random r = new Random();
            int e = (int)r.Next(1, b);
            var funcionarios2 = (from m in db.Funcionarios where m.id == e select m);
            Funcionario func = funcionarios2.ToList<Funcionario>().ElementAt<Funcionario>(0);

            return func;
        }

        [HttpPost]
        public ActionResult AdicionarPedido([Bind(Include = "propano45,propano11,propano5,carburante11,butano13,butano6,n45,n11,n5,nc11,n13,n6,data,hora,observacoes,mangueiras,redutores,valvulas,tubos,sistema,ligacoes")] FormPedido formPedido)
        {
            IEnumerable<Servico> servicos;
            List<ServicoSel> servicosSelecionados = new List<ServicoSel>();
            double duracao = 0;
            double custo = 0;

            var pedidos = (from m in db.Pedidoes select m);
            var ids = new int[pedidos.Count() + 1];
            int k = 0;
            foreach(Pedido p in pedidos)
            {
                ids[k] = p.id;
                k++;
            }
            int idPedido = ids.Max() + 1;

            Pedido pedido = new Pedido();
            pedido.id = idPedido;
            pedido.estado = false;
            pedido.observacoes = formPedido.observacoes;

            pedido.emailCliente = getEmailCliente();

            pedido.data = formPedido.data;
            Time t = new Time(formPedido.hora);
            pedido.hora = t.GetTimeFloat();

            if (formPedido.propano45 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 1 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    duracao += (servico.duracaoMedia * formPedido.n45);
                    custo += (servico.preco * formPedido.n45);
                }
            }
            if (formPedido.propano11 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 2 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    duracao += (servico.duracaoMedia * formPedido.n11);
                    custo += (servico.preco * formPedido.n11);
                }
            }
            if (formPedido.propano5 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 3 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    duracao += (servico.duracaoMedia * formPedido.n5);
                    custo += (servico.preco * formPedido.n5);
                }
            }
            if (formPedido.carburante11 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 4 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    duracao += (servico.duracaoMedia * formPedido.nc11);
                    custo += (servico.preco * formPedido.nc11);
                }
            }
            if (formPedido.butano13 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 5 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    duracao += (servico.duracaoMedia * formPedido.n13);
                    custo += (servico.preco * formPedido.n13);
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
                }
            }
            if (formPedido.mangueiras == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 7 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    duracao += servico.duracaoMedia;
                    custo += servico.preco;
                }
            }
            if (formPedido.redutores == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 8 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    duracao += servico.duracaoMedia;
                    custo += servico.preco;
                }
            }
            if (formPedido.valvulas == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 9 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    duracao += servico.duracaoMedia;
                    custo += servico.preco;
                }
            }
            if (formPedido.tubos == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 10 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    duracao += servico.duracaoMedia;
                    custo += servico.preco;
                }
            }
            if (formPedido.sistema == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 11 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    duracao += servico.duracaoMedia;
                    custo += servico.preco;
                }
            }
            if (formPedido.ligacoes == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 12 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    duracao += servico.duracaoMedia;
                    custo += servico.preco;
                }
            }


            Funcionario func = escolheFuncionario();
            pedido.duracaoEstimada = duracao;
            pedido.custoEstimado = custo + (duracao * func.salario);


            pedido.emailFuncionario = func.email;

            if (ModelState.IsValid)
            {
                db.Pedidoes.Add(pedido);
                db.SaveChanges();
            }

            if (formPedido.propano45 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 1 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Pedido_tem_Servico pts = new Pedido_tem_Servico();
                    pts.idPedido = idPedido;
                    pts.idServico = 1;
                    pts.quantidade = formPedido.n45;
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    ServicoSel s = new ServicoSel(servico.descricao, formPedido.n45);
                    servicosSelecionados.Add(s);

                    if (ModelState.IsValid)
                    {
                        db.Pedido_tem_Servico.Add(pts);
                        db.SaveChanges();
                    }
                }
            }
            if (formPedido.propano11 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 2 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Pedido_tem_Servico pts = new Pedido_tem_Servico();
                    pts.idPedido = idPedido;
                    pts.idServico = 2;
                    pts.quantidade = formPedido.n11;
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    ServicoSel s = new ServicoSel(servico.descricao, formPedido.n11);
                    servicosSelecionados.Add(s);
                    if (ModelState.IsValid)
                    {
                        db.Pedido_tem_Servico.Add(pts);
                        db.SaveChanges();
                    }
                }
            }
            if (formPedido.propano5 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 3 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Pedido_tem_Servico pts = new Pedido_tem_Servico();
                    pts.idPedido = idPedido;
                    pts.idServico = 3;
                    pts.quantidade = formPedido.n5;
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    ServicoSel s = new ServicoSel(servico.descricao, formPedido.n5);
                    servicosSelecionados.Add(s);
                    if (ModelState.IsValid)
                    {
                        db.Pedido_tem_Servico.Add(pts);
                        db.SaveChanges();
                    }
                }
            }
            if (formPedido.carburante11 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 4 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Pedido_tem_Servico pts = new Pedido_tem_Servico();
                    pts.idPedido = idPedido;
                    pts.idServico = 4;
                    pts.quantidade = formPedido.nc11;
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    ServicoSel s = new ServicoSel(servico.descricao, formPedido.nc11);
                    servicosSelecionados.Add(s);
                    if (ModelState.IsValid)
                    {
                        db.Pedido_tem_Servico.Add(pts);
                        db.SaveChanges();
                    }
                }
            }
            if (formPedido.butano13 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 5 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Pedido_tem_Servico pts = new Pedido_tem_Servico();
                    pts.idPedido = idPedido;
                    pts.idServico = 5;
                    pts.quantidade = formPedido.n13;
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    ServicoSel s = new ServicoSel(servico.descricao, formPedido.n13);
                    servicosSelecionados.Add(s);
                    if (ModelState.IsValid)
                    {
                        db.Pedido_tem_Servico.Add(pts);
                        db.SaveChanges();
                    }
                }
            }
            if (formPedido.butano6 == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 6 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Pedido_tem_Servico pts = new Pedido_tem_Servico();
                    pts.idPedido = idPedido;
                    pts.idServico = 6;
                    pts.quantidade = formPedido.n6;
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    ServicoSel s = new ServicoSel(servico.descricao, formPedido.n6);
                    servicosSelecionados.Add(s);
                    if (ModelState.IsValid)
                    {
                        db.Pedido_tem_Servico.Add(pts);
                        db.SaveChanges();
                    }
                }
            }
            if (formPedido.mangueiras == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 7 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Pedido_tem_Servico pts = new Pedido_tem_Servico();
                    pts.idPedido = idPedido;
                    pts.idServico = 7;
                    pts.quantidade = 1;
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    ServicoSel s = new ServicoSel(servico.descricao, 1);
                    servicosSelecionados.Add(s);
                    if (ModelState.IsValid)
                    {
                        db.Pedido_tem_Servico.Add(pts);
                        db.SaveChanges();
                    }
                }
            }
            if (formPedido.redutores == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 8 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Pedido_tem_Servico pts = new Pedido_tem_Servico();
                    pts.idPedido = idPedido;
                    pts.idServico = 8;
                    pts.quantidade = 1;
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    ServicoSel s = new ServicoSel(servico.descricao, 1);
                    servicosSelecionados.Add(s);
                    if (ModelState.IsValid)
                    {
                        db.Pedido_tem_Servico.Add(pts);
                        db.SaveChanges();
                    }
                }
            }
            if (formPedido.valvulas == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 9 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Pedido_tem_Servico pts = new Pedido_tem_Servico();
                    pts.idPedido = idPedido;
                    pts.idServico = 9;
                    pts.quantidade = 1;
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    ServicoSel s = new ServicoSel(servico.descricao, 1);
                    servicosSelecionados.Add(s);
                    if (ModelState.IsValid)
                    {
                        db.Pedido_tem_Servico.Add(pts);
                        db.SaveChanges();
                    }
                }
            }
            if (formPedido.tubos == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 10 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Pedido_tem_Servico pts = new Pedido_tem_Servico();
                    pts.idPedido = idPedido;
                    pts.idServico = 10;
                    pts.quantidade = 1;
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    ServicoSel s = new ServicoSel(servico.descricao, 1);
                    servicosSelecionados.Add(s);
                    if (ModelState.IsValid)
                    {
                        db.Pedido_tem_Servico.Add(pts);
                        db.SaveChanges();
                    }
                }
            }
            if (formPedido.sistema == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 11 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Pedido_tem_Servico pts = new Pedido_tem_Servico();
                    pts.idPedido = idPedido;
                    pts.idServico = 11;
                    pts.quantidade = 1;
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    ServicoSel s = new ServicoSel(servico.descricao, 1);
                    servicosSelecionados.Add(s);
                    if (ModelState.IsValid)
                    {
                        db.Pedido_tem_Servico.Add(pts);
                        db.SaveChanges();
                    }
                }
            }
            if (formPedido.ligacoes == "on")
            {
                servicos = (from m in db.Servicoes where m.id == 12 select m);
                if (servicos.ToList<Servico>().Count > 0)
                {
                    Pedido_tem_Servico pts = new Pedido_tem_Servico();
                    pts.idPedido = idPedido;
                    pts.idServico = 12;
                    pts.quantidade = 1;
                    Servico servico = servicos.ToList<Servico>().ElementAt<Servico>(0);
                    ServicoSel s = new ServicoSel(servico.descricao, 1);
                    servicosSelecionados.Add(s);
                    if (ModelState.IsValid)
                    {
                        db.Pedido_tem_Servico.Add(pts);
                        db.SaveChanges();
                    }
                }
            }

            Time d = new Time(pedido.duracaoEstimada);
            ViewBag.Duracao = d.GetTimeString();
            ViewBag.Custo = pedido.custoEstimado;
            ViewBag.Funcionario = pedido.emailFuncionario;
            ViewBag.Data = pedido.data.ToString("dd/MM/yyyy");
            Time hr = new Time(pedido.hora);
            ViewBag.Hora = hr.GetTimeString();
            ViewBag.Observacoes = pedido.observacoes;
            ViewBag.Servicos = servicosSelecionados;
            ViewBag.Cliente = pedido.emailCliente;
            ViewBag.Pedido = pedido.id;

            return View("Confirmar");
        }

        public ActionResult ConfirmaCancelamento(int idPedido, string data, string hora)
        {
            ViewBag.Hora = hora;
            ViewBag.Data = data;
            ViewBag.Pedido = idPedido;

            return View();
        }

        [HttpPost]
        public ActionResult Cancelar(int idPedido, string sub)
        {
            if (sub == "Continuar")
            {
                try
                {
                    using (var db = new GasContext())
                    {
                        IEnumerable<Pedido> pedidos = (from m in db.Pedidoes where m.id == idPedido select m);
                        Pedido pedido = pedidos.ToList<Pedido>().ElementAt<Pedido>(0);
                        db.Pedidoes.Remove(pedido);

                        IEnumerable<Pedido_tem_Servico> pts = (from m in db.Pedido_tem_Servico where m.idPedido == idPedido select m);
                        foreach (Pedido_tem_Servico p in pts)
                        {
                            db.Pedido_tem_Servico.Remove(p);
                        }

                        db.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                return RedirectToAction("Pendentes", "Cliente");
            }

            else
            {
                return RedirectToAction("Pendentes", "Cliente");
            }
        }

        [HttpPost]
        public ActionResult Confirmar(string res, string emailCliente, string emailFuncionario, int idPedido, string data, string hora)
        {
            if (res == "Confirmar")
            {
                IEnumerable<Cliente> clientes = (from m in db.Clientes where m.email == emailCliente select m);
                Cliente cliente = clientes.ToList<Cliente>().ElementAt<Cliente>(0);
                string msg = "Caro Cliente,\nInformamos que possui um novo servico pendente a " + data + " as " + hora + " horas.\n Cumprimentos,\nClickGAS";
                string contacto = "351" + cliente.contacto;
                new SmsController().Send(contacto , msg);
                Email email = new Email(emailCliente, emailFuncionario, hora, data);
                return RedirectToAction("ConfirmarPedido", "Email", email);
            }

            else
            {
                IEnumerable<Pedido> pedidos = (from m in db.Pedidoes where m.id == idPedido select m);
                Pedido pedido = pedidos.ToList<Pedido>().ElementAt<Pedido>(0);
                db.Pedidoes.Remove(pedido);

                IEnumerable<Pedido_tem_Servico> pts = (from m in db.Pedido_tem_Servico where m.idPedido == idPedido select m);
                foreach(Pedido_tem_Servico p in pts)
                {
                    db.Pedido_tem_Servico.Remove(p);
                }

                db.SaveChanges();

                return View("Solicitar");
            }
        }

        public ActionResult Pendentes()
        {
            string email = "";
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                if (!string.IsNullOrEmpty(authCookie.Value))
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    email = authTicket.Name.ToString();
                }
            }
            var pends = (from m in db.Pedidoes where (m.emailCliente == email && m.estado == false) select m);

            var pendC = new HashSet<ServicosPedido>();

            foreach (Pedido p in pends)
            {
                ServicosPedido servped = new ServicosPedido();
                servped.idPedido = p.id;
                servped.data = p.data;
                servped.hora = p.hora;
                servped.observacoes = p.observacoes;
                var pedts = (from m in db.Pedido_tem_Servico where (m.idPedido == p.id) select m);
                foreach (Pedido_tem_Servico ps in pedts)
                {
                    var servs = (from m in db.Servicoes where (m.id == ps.idServico) select m);
                    Servico serv = servs.ToList<Servico>().ElementAt<Servico>(0);
                    string desc = serv.tipo + " " + serv.descricao;
                    ServicoSel s = new ServicoSel(desc, ps.quantidade);
                    servped.servicos.Add(s);
                }
                pendC.Add(servped);
            }
            return View(pendC);
        }
    }
}