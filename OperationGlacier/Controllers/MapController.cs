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

        public ActionResult Index(string side, string date, int? x, int? y, string game_name)
        {
            
                game_name = GameState.get_game_name(game_name);
            
            ViewBag.game_name = game_name;
            var model = new Models.MapModel();
            model.side = side;
            if (date == "latest")
                date = GameState.LatestTurn(game_name);
            model.date_str = date;

            if (x == null || y == null)
            {
                if (model.side == "Allies" || model.side == "Both")
                {
                    x = 180;//Pearl Harbor
                    y = 107;
                }
                if (model.side == "Japan")
                {
                    x = 114;//Tokyo
                    y = 60;
                }
            }
            model.center_x = (int)x;
            model.center_y = (int)y;
            model.center_zoom = 6;
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
            IQueryable<Comment> comments = ApplicationDbContext.Comments.Where(c => c.date_in_game == my_date && c.game_name == game_name).OrderBy(c => c.date_in_world);
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