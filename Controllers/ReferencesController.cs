using Leticia.Classes;
using Leticia.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Leticia.Controllers
{
    public class ReferencesController : Controller
    {

        public ActionResult References()
        {
            Language language = new Language(ControllerFunctions.References);

            HomeModel model = language.Init();

            return View(model);
        }

    }
}
