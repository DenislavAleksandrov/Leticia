using Leticia.Classes;
using Leticia.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Leticia.Controllers
{
    public class PortfolioController : Controller
    {
        public ActionResult Portfolio()
        {
            Language language = new Language(ControllerFunctions.Portfolio);

            HomeModel model = language.Init();

            return View(model);
        }
    }
}
