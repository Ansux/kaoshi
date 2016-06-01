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
    public class StudentsController : Controller
    {
        private WebContext db = new WebContext();

        // GET: Admin/Student
        public ActionResult Index()
        {
            var es_student = db.es_student.Include(e => e.es_class);
            return View(es_student.ToList());
        }

        // GET: Admin/Student/Details/5
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

        // GET: Admin/Student/Create
        public ActionResult Create()
        {
            ViewBag.class_id = new SelectList(db.es_class, "no", "name");
            return View();
        }

        // POST: Admin/Student/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sno,pwd,real_name,sex,email,create_at,update_at,class_id")] es_student es_student)
        {
            if (ModelState.IsValid)
            {
                db.es_student.Add(es_student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.class_id = new SelectList(db.es_class, "no", "name", es_student.class_id);
            return View(es_student);
        }

        // GET: Admin/Student/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.class_id = new SelectList(db.es_class, "no", "name", es_student.class_id);
            return View(es_student);
        }

        // POST: Admin/Student/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sno,pwd,real_name,sex,email,create_at,update_at,class_id")] es_student es_student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(es_student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.class_id = new SelectList(db.es_class, "no", "name", es_student.class_id);
            return View(es_student);
        }

        // GET: Admin/Student/Delete/5
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

        // POST: Admin/Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            es_student es_student = db.es_student.Find(id);
            db.es_student.Remove(es_student);
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
