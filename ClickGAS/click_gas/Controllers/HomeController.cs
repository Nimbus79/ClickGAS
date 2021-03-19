using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using click_gas.Models;

namespace click_gas.Controllers
{
    public class HomeController : Controller
    {
        GasContext db = new GasContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contactos()
        {
            return View("Contactos");
        }

        public ActionResult Catalogo()
        {
            return View("Catalogo");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult AdicionarCliente([Bind(Include = "contribuinte,nome,palavraPasse,email,contacto,dataNasc,rua,numero,codPostal,freguesia,concelho")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.role = "user";
                cliente.palavraPasse = MyHelpers.HashPassword(cliente.palavraPasse);
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("ConfirmarRegisto", "Email", cliente);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Contactar(string email, string mensagem)
        {
            Email e = new Email(email, mensagem);
            return RedirectToAction("ContactarEmpresa", "Email", e);
        }

        [HttpPost]
        public ActionResult Login(string email, string palavraPasse)
        {
            if (ModelState.IsValid)
            {
                var clientes = (from m in db.Clientes
                                where m.email == email
                                select m);
                if (clientes.ToList<Cliente>().Count > 0)
                {
                    Cliente cliente = clientes.ToList<Cliente>().ElementAt<Cliente>(0);
                    //FormsAuthentication.SetAuthCookie(cliente.email,false);

                    using (MD5 md5Hash = MD5.Create())
                    {
                        if (MyHelpers.VerifyMd5Hash(md5Hash, palavraPasse, cliente.palavraPasse))
                        {
                            HttpCookie cookie = MyHelpers.CreateAuthorizeTicket(cliente.email.ToString(), cliente.role);
                            Response.Cookies.Add(cookie);
                            return RedirectToAction("VerPerfil", "Cliente");
                        }
                        else
                        {
                            ModelState.AddModelError("password", "Password incorreta!");
                            return View("Index");
                        }
                    }

                }
                else
                {
                    var funcionarios = (from m in db.Funcionarios
                                        where m.email == email
                                        select m);
                    if (funcionarios.ToList<Funcionario>().Count > 0)
                    {
                        Funcionario func = funcionarios.ToList<Funcionario>().ElementAt<Funcionario>(0);
                        using (MD5 md5Hash = MD5.Create())
                        {
                            if (MyHelpers.VerifyMd5Hash(md5Hash, palavraPasse, func.palavraPasse))
                            {
                                HttpCookie cookie = MyHelpers.CreateAuthorizeTicket(func.email.ToString(), func.role);
                                Response.Cookies.Add(cookie);
                                return RedirectToAction("Pendentes", "Funcionario");
                            }
                            else
                            {
                                ModelState.AddModelError("password", "Password incorreta!");
                                return View("Index");
                            }
                        }
                    }
                    ModelState.AddModelError("", "Dados incorretos!");
                    return View("Index");

                }
            }
            else
            {
                ModelState.AddModelError("", "Pedido inválido!");
                return View("Index");

            }
        }
    }
}