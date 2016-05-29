using Leticia.Classes;
using Leticia.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Leticia.Controllers
{
    public class ServicesController : Controller
    {
        public ActionResult Services()
        {
            Language lang = new Language(ControllerFunctions.Services);

            HomeModel model = lang.Init();

            return View(model);
        }

    }
}
