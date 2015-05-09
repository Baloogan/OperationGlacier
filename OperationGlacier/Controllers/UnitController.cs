using OperationGlacier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationGlacier.Controllers
{
    public class UnitController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index(string tid, string game_name)
        {

            game_name = GameState.get_game_name(game_name);

            ViewBag.game_name = game_name;
            UnitModel model = new UnitModel();
            model.timeline_id = tid;
            model.name = GameState.get_name_from_timeline_id(game_name, tid);
            model.comments = db.Comments
                .Where(c => c.unit_timeline_id == tid)
                .OrderBy(c => c.date_in_world)
                .ToList()//actually important.
                .Select(c => new CommentModel(c))
                .ToList();

            return View(model);
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