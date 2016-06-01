using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using kaoshi.Models;

namespace kaoshi.Areas.Admin.Controllers
{
   public class NoticeController : Controller
   {
      private WebContext db = new WebContext();

      // GET: Admin/Notice
      public ActionResult Index()
      {
         return View(db.es_notice.ToList());
      }

      // GET: Admin/Notice/Details/5
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_notice es_notice = db.es_notice.Find(id);
         if (es_notice == null)
         {
            return HttpNotFound();
         }
         return View(es_notice);
      }

      // GET: Admin/Notice/Create
      public ActionResult Create()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,title,content")] es_notice es_notice)
      {
         if (ModelState.IsValid)
         {
            es_notice.create_at = DateTime.Now;
            db.es_notice.Add(es_notice);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         return View(es_notice);
      }

      // GET: Admin/Notice/Edit/5
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_notice es_notice = db.es_notice.Find(id);
         if (es_notice == null)
         {
            return HttpNotFound();
         }
         return View(es_notice);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,title,content")] es_notice es_notice)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_notice).State = EntityState.Modified;
            db.Entry(es_notice).Property(e => e.create_at).IsModified = false;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(es_notice);
      }

      // GET: Admin/Notice/Delete/5
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_notice es_notice = db.es_notice.Find(id);
         if (es_notice == null)
         {
            return HttpNotFound();
         }
         return View(es_notice);
      }

      // POST: Admin/Notice/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_notice es_notice = db.es_notice.Find(id);
         db.es_notice.Remove(es_notice);
         db.SaveChanges();
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
