using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using kaoshi.Models;

namespace kaoshi.Areas.Teacher.Controllers
{
   [Filters.TeacherLoginAuthorize]
   public class TestOptionController : Controller
   {
      private WebContext db = new WebContext();

      // GET: Teacher/TestOption
      public ActionResult Index()
      {
         var es_test_option = db.es_test_option.Include(e => e.es_test);
         return View(es_test_option.ToList());
      }

      // GET: Teacher/TestOption/Details/5
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_test_option es_test_option = db.es_test_option.Find(id);
         if (es_test_option == null)
         {
            return HttpNotFound();
         }
         return View(es_test_option);
      }

      // GET: Teacher/TestOption/Create
      public ActionResult Create()
      {
         ViewBag.test = new SelectList(db.es_test, "id", "title");
         return View();
      }

      // POST: Teacher/TestOption/Create
      // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
      // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,abcd,content,test")] es_test_option es_test_option)
      {
         if (ModelState.IsValid)
         {
            db.es_test_option.Add(es_test_option);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.test = new SelectList(db.es_test, "id", "title", es_test_option.test);
         return View(es_test_option);
      }

      // GET: Teacher/TestOption/Edit/5
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_test_option es_test_option = db.es_test_option.Find(id);
         if (es_test_option == null)
         {
            return HttpNotFound();
         }
         ViewBag.test = new SelectList(db.es_test, "id", "title", es_test_option.test);
         return View(es_test_option);
      }

      // POST: Teacher/TestOption/Edit/5
      // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
      // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,abcd,content,test")] es_test_option es_test_option)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_test_option).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.test = new SelectList(db.es_test, "id", "title", es_test_option.test);
         return View(es_test_option);
      }

      // GET: Teacher/TestOption/Delete/5
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_test_option es_test_option = db.es_test_option.Find(id);
         if (es_test_option == null)
         {
            return HttpNotFound();
         }
         return View(es_test_option);
      }

      // POST: Teacher/TestOption/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_test_option es_test_option = db.es_test_option.Find(id);
         db.es_test_option.Remove(es_test_option);
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
