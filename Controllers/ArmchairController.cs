using Leticia.Classes;
using Leticia.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Leticia.Controllers
{
    public class ArmchairController : Controller
    {

        public ActionResult Armchair()
        {
            Language lang = new Language(ControllerFunctions.Armchair);

            HomeModel model = lang.Init();

            return View(model);
        }

    }
}
