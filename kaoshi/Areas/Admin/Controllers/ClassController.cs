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
   public class ClassController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         var cls = db.es_class.ToList();
         return View(cls);
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_class es_class = db.es_class.Find(id);
         if (es_class == null)
         {
            return HttpNotFound();
         }
         var stus = db.es_student.Where(s => s.class_id == es_class.no);
         ViewData["cno"] = es_class.no;
         ViewData["cname"] = es_class.name;

         return View(stus);
      }

      public ActionResult Create()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "no,name")] es_class es_class)
      {
         if (ModelState.IsValid)
         {
            db.es_class.Add(es_class);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         return View(es_class);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_class es_class = db.es_class.Find(id);
         if (es_class == null)
         {
            return HttpNotFound();
         }
         return View(es_class);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "no,name")] es_class es_class)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_class).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(es_class);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_class es_class = db.es_class.Find(id);
         if (es_class == null)
         {
            return HttpNotFound();
         }
         return View(es_class);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_class es_class = db.es_class.Find(id);
         db.es_class.Remove(es_class);
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
