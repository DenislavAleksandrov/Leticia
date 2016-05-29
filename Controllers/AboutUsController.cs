using Leticia.Classes;
using Leticia.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Leticia.Controllers
{
    public class AboutUsController : Controller
    {
        public ActionResult AboutUs()
        {
            Language language = new Language(ControllerFunctions.AboutUs);

            HomeModel model = language.Init();

            return View(model);
        }
    }
}
