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
        public ActionResult Create([Bind(Include = "id,login_id,login_pwd,real_name,sex,email,create_at,update_at")] es_teacher es_teacher)
        {
            if (ModelState.IsValid)
            {
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
        public ActionResult Edit([Bind(Include = "id,login_id,login_pwd,real_name,sex,email,create_at,update_at")] es_teacher es_teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(es_teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(es_teacher);
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
