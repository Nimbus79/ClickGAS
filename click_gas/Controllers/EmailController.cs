using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using click_gas.Models;

namespace click_gas.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ConfirmarRegisto(Cliente cliente)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(cliente.email));  // e-mail do cliente
            message.From = new MailAddress("clickgas@outlook.com");  // e-mail da ClickGas
            message.Subject = "Registo bem sucedido";
            message.Body = "Caro(a) " + cliente.nome + ",<br> O seu registo foi bem sucedido, pode efetuar login.<br>Cumprimentos,<br>ClickGAS";
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "clickgas@outlook.com",  // e-mail
                    Password = "grupo24-17-18"  // password
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<ActionResult> ContactarEmpresa(Email email)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress("clickgas@outlook.com"));
            message.From = new MailAddress("clickgas@outlook.com");  // e-mail da ClickGas
            message.Subject = "Contacto de: " + email.emailCliente;
            message.Body = "De: " + email.emailCliente + "<br>" + email.mensagem;
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "clickgas@outlook.com",  // e-mail
                    Password = "grupo24-17-18"  // password
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);

                HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    return RedirectToAction("Contactos", "Cliente");
                }
                else
                {
                    return RedirectToAction("Contactos", "Home");
                }
            }
        }

        public async Task<ActionResult> ValidarPedido(Fatura fatura)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(fatura.emailCliente));
            message.From = new MailAddress("clickgas@outlook.com");  // e-mail da ClickGas
            message.Subject = "Fatura de Serviços";

            StringBuilder email = new StringBuilder();
            email.Append("<h1><center>FATURA DE SERVIÇO</center></h1>");
            email.Append("<p><b>Nº Referência</b> ").Append(fatura.referencia).Append("</p>"); ;
            email.Append("<p><b>Data Emissão:</b> ").Append(DateTime.Now.ToString("dd/MM/yyyy")).Append("</p>"); ;
            email.Append("<p><b>Nº Contribuinte:</b> ").Append(fatura.contribuinte).Append("</p>");
            email.Append("<br><br><p><center><b>SERVIÇOS PRESTADOS</b></center></p><table align=\"center\" border=\"1\">");
            email.Append("<tr><th>Serviço</th><th>Custo</th></tr>");

            List<ServicoFatura> servicos = (List<ServicoFatura>) TempData["Servicos"];

            foreach(ServicoFatura s in servicos)
            {
                email.Append("<tr><td>").Append(s.descricao).Append("</td><td>").Append(Math.Round(s.custo, 2, MidpointRounding.AwayFromZero)).Append("€</td></tr>");
            }

            email.Append("<tr><td>Mão de Obra</td><td> ").Append(Math.Round(fatura.maoDeObra, 2, MidpointRounding.AwayFromZero)).Append("€</td></tr>");
            email.Append("</table><br><br>");
            email.Append("<p><b>TOTAL:</b> ").Append(Math.Round(fatura.custoTotal, 2, MidpointRounding.AwayFromZero)).Append("€</p>");

            message.Body = email.ToString();
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "clickgas@outlook.com",  // e-mail
                    Password = "grupo24-17-18"  // password
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return RedirectToAction("pendentes", "Funcionario");
            }
        }

        public async Task<ActionResult> ConfirmarPedido(Email email)
        {
            var messageC = new MailMessage();
            messageC.To.Add(new MailAddress(email.emailCliente));
            messageC.From = new MailAddress("clickgas@outlook.com");  // e-mail da ClickGas
            messageC.Subject = "Novo serviço pendente";
            messageC.Body = "Caro Cliente,<br>Possui um novo serviço pendente a <b>" + email.data + "</b> às <b>" + email.hora + "</b> horas.<br> Cumprimentos,<br>ClickGAS";
            messageC.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "clickgas@outlook.com",  // e-mail
                    Password = "grupo24-17-18"  // password
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(messageC);
            }

            var messageF = new MailMessage();
            messageF.To.Add(new MailAddress(email.emailFuncionario));  // e-mail do cliente
            messageF.From = new MailAddress("clickgas@outlook.com");  // e-mail da ClickGas
            messageF.Subject = "Novo serviço pendente";
            messageF.Body = "Caro Funcionário,<br> Possui um novo serviço pendente a <b>" + email.data + "</b> às <b>" + email.hora + "</b> horas.<br> Cumprimentos,<br>ClickGAS";
            messageF.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "clickgas@outlook.com",  // e-mail
                    Password = "grupo24-17-18"  // password
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(messageF);
            }

            return RedirectToAction("Pendentes", "Cliente");
        }
    }
}