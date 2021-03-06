﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

        protected UserManager<ApplicationUser> UserManager { get; set; }

        public UnitController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }

        public ActionResult Index(string tid, string game_name)
        {

            if (Request.IsAuthenticated)
            {
                ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                if (tid.Contains("Allies"))
                    if (user.SideRestriction == "Japan")
                        return Index(tid.Replace("Allies", "Japan"), game_name);
                if (tid.Contains("Japan"))
                    if (user.SideRestriction == "Allies")
                        return Index(tid.Replace("Japan", "Allies"), game_name);
            }

            game_name = GameState.get_game_name(game_name);

            ViewBag.game_name = game_name;
            UnitModel model = new UnitModel();
            model.timeline_id = tid;
            model.name = GameState.get_name_from_timeline_id(game_name, tid);
            var z = db.Comments
                .Where(c => c.unit_timeline_id == tid)
                .OrderBy(c => c.date_in_world);
            var zz = z.ToList();//actually important.

            if (Request.IsAuthenticated)
            {
                ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                zz = zz.Where(c => c.side_restriction == user.SideRestriction).ToList();
            }

            var zzz = zz.Select(c => new CommentModel(c)).ToList();

            model.comments = zzz;
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