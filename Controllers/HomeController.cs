using Leticia.Classes;
using Leticia.Models.ProjectModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Leticia.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Language language = new Language(ControllerFunctions.Index);

            HomeModel model = language.Init();

            return View(model);
        }
       
        public ActionResult About()
        {
            Language language = new Language(ControllerFunctions.About);

            return View();
        }

        public ActionResult Contact()
        {
            Language language = new Language(ControllerFunctions.Contacts);

            HomeModel model = language.Init();

            ViewBag.Message = "Your contact page.";

            return View(model);
        }

        public ActionResult PrivaciPolicy()
        {
            Language language = new Language(ControllerFunctions.PrivacyPolicy);

            HomeModel model = language.Init();

            return View(model);
        }

        [HttpPost]
        public JsonResult Email()
        {
            try
            {
                string result = Request.Form[0];

                Leticia.Classes.Email.RootObject results = JsonConvert.DeserializeObject<Leticia.Classes.Email.RootObject>(result);

                string body = results.text + "\n" + results.telefon + "\n" + results.name + "\n" + results.email;

                MailMessage mail = new MailMessage(results.email, "leticia_mebel@abv.bg", "Zapitvane", body);

                SmtpClient smtp = new SmtpClient();

                smtp.Host = "smtp.gmail.com";

                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("denisvoll55@gmail.com", "light100%");
                smtp.EnableSsl = true;
                smtp.Send(mail);

                return this.Json("OK");
            }
            catch (Exception ex)
            {
                return this.Json("FALSE");
            }
          
        }
    }
}
