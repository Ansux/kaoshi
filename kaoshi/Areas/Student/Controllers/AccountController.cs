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

namespace kaoshi.Areas.Student.Controllers
{
   [Filters.StuLoginAuthorize]
   public class AccountController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         var exams = db.es_exam.Where(e => e.end_time > DateTime.Now).ToList();

         var notices = db.es_notice.Take(6);
         ViewData["notices"] = notices.ToList();

         return View(exams);
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_student es_student = db.es_student.Find(id);
         if (es_student == null)
         {
            return HttpNotFound();
         }
         return View(es_student);
      }

      public ActionResult Notices()
      {
         var notice = db.es_notice.ToList();
         return View(notice);
      }

      public ActionResult Notice(int? id)
      {
         return View(db.es_notice.Find(id));
      }

      [AllowAnonymous]
      public ActionResult Create()
      {
         ViewBag.class_id = new SelectList(db.es_class, "no", "name");
         return View();
      }

      [AllowAnonymous]
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "sno,pwd,real_name,sex,email,class_id")] es_student stu)
      {
         if (ModelState.IsValid)
         {
            stu.create_at = DateTime.Now;
            stu.pwd = Tools.MD5(stu.pwd);
            db.es_student.Add(stu);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         return View(stu);
      }

      public ActionResult Edit()
      {
         var id = int.Parse(Session["Sno"].ToString());
         es_student es_student = db.es_student.Find(id);
         if (es_student == null)
         {
            return HttpNotFound();
         }
         return View(es_student);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit(string real_name, string email, byte sex)
      {
         var id = int.Parse(Session["Sno"].ToString());
         var stu = db.es_student.Find(id);
         try
         {
            db.Entry(stu).State = EntityState.Unchanged;
            db.Entry(stu).Property(e => e.real_name).IsModified = true;
            db.Entry(stu).Property(e => e.email).IsModified = true;
            db.Entry(stu).Property(e => e.sex).IsModified = true;
            db.Entry(stu).Property(e => e.update_at).IsModified = true;
            stu.update_at = DateTime.Now;
            stu.real_name = real_name;
            stu.email = email;
            stu.sex = sex;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         catch (Exception e)
         {
            return View(stu);

         }
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_student es_student = db.es_student.Find(id);
         if (es_student == null)
         {
            return HttpNotFound();
         }
         return View(es_student);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_student es_student = db.es_student.Find(id);
         db.es_student.Remove(es_student);
         db.SaveChanges();
         return RedirectToAction("Index");
      }

      [AllowAnonymous]
      public ActionResult Signin()
      {
         if (Session["Sno"] != null)
         {
            return RedirectToAction("Index");
         }

         var stu = new es_student();
         stu.sno = 30101;
         stu.pwd = "123";
         return View(stu);
      }

      [AllowAnonymous]
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Signin([Bind(Include = "sno,pwd")] es_student stu)
      {
         if (ModelState.IsValid)
         {
            var pwd = Tools.MD5(stu.pwd);
            var student = db.es_student.Where(s => s.sno == stu.sno && s.pwd == pwd).FirstOrDefault();
            if (student != null)
            {
               Session["Sno"] = student.sno;
               Session["Sname"] = student.real_name;
            }

            var ReturnUrl = Request.QueryString["ReturnUrl"];
            if (ReturnUrl != null)
            {
               return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index");
         }

         return View(stu);
      }

      [AllowAnonymous]
      public ActionResult Signout()
      {
         Session.Remove("Sno");
         Session.Remove("Sname");
         return RedirectToAction("Signin");
      }
   }
}
