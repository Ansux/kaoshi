using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using kaoshi.Models;
using kaoshi.Controllers;

namespace kaoshi.Areas.Admin.Controllers
{
   [Filters.AdminLoginAuthorize]
   public class TeachersController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         return View(db.es_teacher.ToList());
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_teacher es_teacher = db.es_teacher.Find(id);
         if (es_teacher == null)
         {
            return HttpNotFound();
         }
         return View(es_teacher);
      }

      public ActionResult Create()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,login_id,login_pwd,real_name,sex,email")] es_teacher es_teacher)
      {
         if (ModelState.IsValid)
         {
            es_teacher.login_pwd = Tools.MD5(es_teacher.login_pwd);
            es_teacher.create_at = DateTime.Now;
            db.es_teacher.Add(es_teacher);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         return View(es_teacher);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_teacher es_teacher = db.es_teacher.Find(id);
         if (es_teacher == null)
         {
            return HttpNotFound();
         }
         return View(es_teacher);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit(int id, string login_pwd, string real_name, byte sex, string email)
      {
         var es_teacher = db.es_teacher.Find(id);
         try
         {
            es_teacher.sex = sex;
            es_teacher.real_name = real_name;
            es_teacher.email = email;
            es_teacher.update_at = DateTime.Now;

            if (es_teacher.login_pwd != null)
            {
               es_teacher.login_pwd = Tools.MD5(login_pwd);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         catch
         {
            return View(es_teacher);
         }
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_teacher es_teacher = db.es_teacher.Find(id);
         if (es_teacher == null)
         {
            return HttpNotFound();
         }
         return View(es_teacher);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_teacher es_teacher = db.es_teacher.Find(id);
         db.es_teacher.Remove(es_teacher);
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
