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
   public class AccountController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         var es_manager = db.es_manager.Include(e => e.es_role);
         return View(es_manager.ToList());
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_manager es_manager = db.es_manager.Find(id);
         if (es_manager == null)
         {
            return HttpNotFound();
         }
         return View(es_manager);
      }

      public ActionResult Create()
      {
         ViewBag.role = new SelectList(db.es_role, "id", "name");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,login_id,login_pwd,real_name,sex,email,role")] es_manager es_manager)
      {
         if (ModelState.IsValid)
         {
            es_manager.create_at = DateTime.Now;
            db.es_manager.Add(es_manager);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.role = new SelectList(db.es_role, "id", "name", es_manager.role);
         return View(es_manager);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_manager es_manager = db.es_manager.Find(id);
         if (es_manager == null)
         {
            return HttpNotFound();
         }
         ViewBag.role = new SelectList(db.es_role, "id", "name", es_manager.role);
         return View(es_manager);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,login_id,login_pwd,real_name,sex,email,role")] es_manager es_manager)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_manager).State = EntityState.Modified;
            es_manager.update_at = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.role = new SelectList(db.es_role, "id", "name", es_manager.role);
         return View(es_manager);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_manager es_manager = db.es_manager.Find(id);
         if (es_manager == null)
         {
            return HttpNotFound();
         }
         return View(es_manager);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_manager es_manager = db.es_manager.Find(id);
         db.es_manager.Remove(es_manager);
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

      public ActionResult Signin()
      {
         if (Session["Mid"] != null)
         {
            return RedirectToAction("Index");
         }

         var a = new es_manager();
         a.login_id = "sa";
         a.login_pwd = "123";

         return View(a);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Signin([Bind(Include = "login_id,login_pwd")] es_manager manager)
      {
         if (ModelState.IsValid)
         {
            var pwd = Tools.MD5(manager.login_pwd);
            var mobj = db.es_manager.Where(m => m.login_id == manager.login_id && m.login_pwd == pwd).FirstOrDefault();
            if (mobj != null)
            {
               Session["Mid"] = mobj.id;
               Session["MLoginId"] = mobj.login_id;
               Session["Mname"] = mobj.real_name;
            }

            var ReturnUrl = Request.QueryString["ReturnUrl"];
            if (ReturnUrl != null)
            {
               return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index");
         }

         return View(manager);
      }

      public ActionResult Signout()
      {
         Session.Remove("Mid");
         Session.Remove("Mname");
         return RedirectToAction("Signin");
      }
   }
}
