using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nexmo.Api;
using click_gas.Models;

namespace click_gas.Controllers
{
    public class SmsController : Controller
    {
        // GET: SMS
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Send()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public void Send(string to, string text)
        {
            var results = SMS.Send(new SMS.SMSRequest
            {
                from = Configuration.Instance.Settings["appsettings:NEXMO_FROM_NUMBER"],
                to = to,
                text = text
            });
        }
    }
}