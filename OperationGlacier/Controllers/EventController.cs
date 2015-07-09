using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OperationGlacier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationGlacier.Controllers
{
    public class EventController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        protected UserManager<ApplicationUser> UserManager { get; set; }

        public EventController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }

        public ActionResult Index(string hash)
        {

            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}