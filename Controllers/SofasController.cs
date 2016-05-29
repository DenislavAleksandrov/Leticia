using Leticia.Classes;
using Leticia.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Leticia.Controllers
{
    public class SofasController : Controller
    {
        public ActionResult Sofas()
        {
            Language language = new Language(ControllerFunctions.Sofas);

            HomeModel model = language.Init();

            return View(model);
        }

    }
}
