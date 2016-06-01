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
   public class RoleController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         return View(db.es_role.ToList());
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_role es_role = db.es_role.Find(id);
         if (es_role == null)
         {
            return HttpNotFound();
         }
         return View(es_role);
      }

      public ActionResult Create()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,name")] es_role es_role)
      {
         if (ModelState.IsValid)
         {
            db.es_role.Add(es_role);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         return View(es_role);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_role es_role = db.es_role.Find(id);
         if (es_role == null)
         {
            return HttpNotFound();
         }
         return View(es_role);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,name")] es_role es_role)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_role).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(es_role);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_role es_role = db.es_role.Find(id);
         if (es_role == null)
         {
            return HttpNotFound();
         }
         return View(es_role);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_role es_role = db.es_role.Find(id);
         db.es_role.Remove(es_role);
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
