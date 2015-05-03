using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationGlacier.Controllers
{
    public class MapController : Controller
    {
        public ActionResult Index(string side, string date)
        {
            ViewBag.Side = side;
            ViewBag.Date = date;
            if (side != "Allies" && System.Web.HttpContext.Current.User.Identity.Name == "baloogan@gmail.com")
                return View("Index", "Home");
            if (side != "Japan" && System.Web.HttpContext.Current.User.Identity.Name == "historicalgamer@gmail.com")
                return View("Index", "Home");
            return View();
        }


    }
}