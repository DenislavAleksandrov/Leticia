using Leticia.Classes;
using Leticia.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Leticia.Controllers
{
    public class CornerSofaController : Controller
    {
        public ActionResult CornerSofa()
        {
            Language lang = new Language(ControllerFunctions.CornerSofa);

            HomeModel model = lang.Init();

            return View(model);
        }

    }
}
