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
        public ActionResult Index(string tid)
        {
            UnitModel model = new UnitModel();
            model.timeline_id = tid;
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