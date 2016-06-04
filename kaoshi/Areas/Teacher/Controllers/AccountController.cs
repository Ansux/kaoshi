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

namespace kaoshi.Areas.Teacher.Controllers
{
   public class AccountController : Controller
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

      [Filters.TeacherLoginAuthorize]
      public ActionResult Edit()
      {
         int tid = int.Parse(Session["Tid"].ToString());
         es_teacher es_teacher = db.es_teacher.Find(tid);
         if (es_teacher == null)
         {
            return HttpNotFound();
         }
         return View(es_teacher);
      }

      [Filters.TeacherLoginAuthorize]
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit(int id,string login_pwd, string real_name, byte sex, string email)
      {
         int tid = int.Parse(Session["Tid"].ToString());
         var teacher = db.es_teacher.Find(tid);
         try
         {
            teacher.real_name = real_name;
            teacher.sex = sex;
            teacher.email = email;
            if (login_pwd != null)
            {
               teacher.login_pwd = login_pwd;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         catch
         {
            return View(teacher);
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

      public ActionResult Signin()
      {
         var t = new es_teacher();
         t.login_id = "t1";
         t.login_pwd = "123";

         return View(t);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Signin([Bind(Include = "login_id,login_pwd")] es_teacher teacher)
      {
         if (ModelState.IsValid)
         {
            var pwd = Tools.MD5(teacher.login_pwd);
            var tobj = db.es_teacher.Where(t => t.login_id == teacher.login_id && t.login_pwd == pwd).FirstOrDefault();
            if (tobj != null)
            {
               Session["Tid"] = tobj.id;
               Session["TLoginId"] = tobj.login_id;
               Session["Tname"] = tobj.real_name;
            }

            var ReturnUrl = Request.QueryString["ReturnUrl"];
            if (ReturnUrl != null)
            {
               return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index");
         }

         return View(teacher);
      }

      public ActionResult Signout()
      {
         Session.Remove("Tid");
         Session.Remove("Tname");
         return RedirectToAction("Signin");
      }

   }
}
