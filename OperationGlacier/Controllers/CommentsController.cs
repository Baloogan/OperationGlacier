﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OperationGlacier.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OperationGlacier.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public CommentsController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }

        // GET: Comments
        public async Task<ActionResult> Index()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.UserName != "Baloogan")
            {
                return RedirectToAction("Index", "Home");
            }

            return View(await db.Comments.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
     //   public ActionResult Create()
      //  {
      //      return View();
      //  }
        /*
         * 
            + "date_string=" + current_unit.date_string
            + "&unit_timeline_id=" + current_unit.timeline_id
            + "&unit_location=" + current_unit.location
            + "&x=" + current_unit.x
            + "&y=" + current_unit.y + "'>Add Comment</a>";
         * 
         * */
        public ActionResult Create(string date_string, string unit_timeline_id, string unit_location, int x, int y, string unit_name)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            Comment model = new Comment()
            {
                date_in_world = DateTime.Now,
                date_in_game = WitpUtility.from_date_str(date_string),
                unit_timeline_id = unit_timeline_id,
                x=x,
                y=y,
                unit_location = unit_location,
                unit_name = unit_name
            };
            
            return View(model);
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CommentID,Username,date_in_game,date_in_world,unit_timeline_id,unit_side_str,message,ReplyToCommentID,side_restriction,x,y,unit_name,unit_location,unit_report_first_line")] Comment comment)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = UserManager.FindById(User.Identity.GetUserId());
            comment.Username = user.UserName;
            comment.side_restriction = user.SideRestriction;
            comment.date_in_world = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                await db.SaveChangesAsync();
                return Redirect("/OperationGlacier/Unit?tid=" + comment.unit_timeline_id);
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.UserName != "Baloogan")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CommentID,Username,date_in_game,date_in_world,unit_timeline_id,unit_side_str,message,ReplyToCommentID,side_restriction,x,y,unit_name,unit_location,unit_report_first_line")] Comment comment)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.UserName != "Baloogan")
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(comment).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.UserName != "Baloogan")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.UserName != "Baloogan")
            {
                return RedirectToAction("Index", "Home");
            }

            Comment comment = await db.Comments.FindAsync(id);
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
