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
   [Filters.AdminLoginAuthorize]
   public class SubjectsController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         return View(db.es_subject.ToList());
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_subject es_subject = db.es_subject.Find(id);
         if (es_subject == null)
         {
            return HttpNotFound();
         }
         return View(es_subject);
      }

      public ActionResult Create()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,name")] es_subject es_subject)
      {
         if (ModelState.IsValid)
         {
            db.es_subject.Add(es_subject);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         return View(es_subject);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_subject es_subject = db.es_subject.Find(id);
         if (es_subject == null)
         {
            return HttpNotFound();
         }
         return View(es_subject);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,name")] es_subject es_subject)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_subject).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(es_subject);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_subject es_subject = db.es_subject.Find(id);
         if (es_subject == null)
         {
            return HttpNotFound();
         }
         return View(es_subject);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_subject es_subject = db.es_subject.Find(id);
         db.es_subject.Remove(es_subject);
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
