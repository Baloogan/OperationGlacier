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
            return View();
        }


    }
}