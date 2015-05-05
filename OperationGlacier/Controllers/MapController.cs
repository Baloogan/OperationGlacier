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
            var model = new Models.MapModel();
            model.side = side;
            if (date == "latest")
                date = "411207"; //lol
            model.date_str = date;

            ApplicationUser user = null;
            if (Request.IsAuthenticated)
            {
                user = UserManager.FindById(User.Identity.GetUserId());
                if (user.SideRestriction != "Both" && user.SideRestriction != side)
                {
                    return new RedirectResult("~/");
                }
            }
            var my_date = WitpUtility.from_date_str(model.date_str);
            IQueryable<Comment> comments = ApplicationDbContext.Comments.Where(c => c.date_in_game == my_date).OrderBy(c => c.date_in_world);
            if (Request.IsAuthenticated)
            {
                if (user.SideRestriction != "Both")
                {
                    comments = comments.Where(c => c.side_restriction == user.SideRestriction);
                }
            }
            if (model.side == "Japan")
            {
                comments = comments.Where(c => c.side_restriction == "Japan");
            }
            else if (model.side == "Allies")
            {
                comments = comments.Where(c => c.side_restriction == "Allies");
            }

            //var g = comments;

            model.comments = comments
                .ToList()//Linq to Elements -> Linq to something
                .Select(c => new CommentModel(c))
                .GroupBy(c => c.x * 10000 + c.y)
                .Select(c=> c.Select(d=>d).ToList())
                .ToList();
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ApplicationDbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}