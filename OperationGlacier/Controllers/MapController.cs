using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OperationGlacier.Models;
using Microsoft.AspNet.Identity.EntityFramework;
namespace OperationGlacier.Controllers
{
    public class MapController : Controller
    {
        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public MapController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        public ActionResult Index(string side, string date)
        {
            ViewBag.Side = side;
            if (date == "latest")
                date = "411207"; //lol
            ViewBag.Date = date;

            if (Request.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                if (user.SideRestriction != "Both" && user.SideRestriction != side)
                {
                    return new RedirectResult("~/");
                }
            }

            return View();
        }


    }
}