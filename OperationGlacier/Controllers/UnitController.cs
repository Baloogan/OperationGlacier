using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationGlacier.Controllers
{
    public class UnitController : Controller
    {
        public ActionResult Index(string tid)
        {
            ViewBag.tid = tid;
            return View();
        }


    }
}