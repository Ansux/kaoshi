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
   public class ExamController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         var es_exam = db.es_exam;
         return View(es_exam.ToList());
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_exam es_exam = db.es_exam.Find(id);
         if (es_exam == null)
         {
            return HttpNotFound();
         }
         return View(es_exam);
      }

      public ActionResult ViewDetail(int sno)
      {
         var exam = db.es_stu_exam.SingleOrDefault(e => e.student == sno);
         return View(exam);
      }

      public ActionResult Create()
      {
         ViewBag.paper = new SelectList(db.es_paper.Where(p => p.status == 3), "id", "name");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,name,paper,begin_time")] es_exam es_exam)
      {
         if (ModelState.IsValid)
         {
            var test_time = db.es_paper.Find(es_exam.paper).test_time;
            es_exam.end_time = Convert.ToDateTime(es_exam.begin_time).AddMinutes(test_time);

            es_exam.status = 1;
            db.es_exam.Add(es_exam);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.paper = new SelectList(db.es_paper, "id", "name", es_exam.paper);
         return View(es_exam);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_exam es_exam = db.es_exam.Find(id);
         if (es_exam == null)
         {
            return HttpNotFound();
         }
         ViewBag.paper = new SelectList(db.es_paper, "id", "name", es_exam.paper);
         return View(es_exam);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,name,paper,begin_time")] es_exam es_exam)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_exam).State = EntityState.Modified;

            var test_time = db.es_paper.Find(es_exam.paper).test_time;
            es_exam.end_time = Convert.ToDateTime(es_exam.begin_time).AddMinutes(test_time);

            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.paper = new SelectList(db.es_paper, "id", "name", es_exam.paper);
         return View(es_exam);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_exam es_exam = db.es_exam.Find(id);
         if (es_exam == null)
         {
            return HttpNotFound();
         }
         return View(es_exam);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_exam es_exam = db.es_exam.Find(id);
         db.es_exam.Remove(es_exam);
         db.SaveChanges();
         return RedirectToAction("Index");
      }

      #region 统计功能
      public ActionResult Chart()
      {
         // 统计已考试结束的考试
         var exam = db.es_exam.Where(e => e.end_time < DateTime.Now).ToList();
         return View(exam);
      }

      // 成绩统计详情页
      public ActionResult ChartDetail(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_exam es_exam = db.es_exam.Find(id);
         if (es_exam == null)
         {
            return HttpNotFound();
         }
         ViewBag.stuNum = db.es_student.Count();
         ViewBag.stuExamNum = db.es_stu_exam.Where(e => e.exam == es_exam.id).Count();
         return View(es_exam);
      }

      // 获取所有考试成绩，用于考试成绩排名
      public JsonResult GetRanks(int? id)
      {
         var exams = db.es_stu_exam.Where(e => e.exam == id).ToList();
         var json = (from e in exams
                     select new
                     {
                        id = e.id,
                        sno = e.es_student.sno,
                        sname = e.es_student.real_name,
                        score = e.score
                     }).ToArray();

         return Json(json, JsonRequestBehavior.AllowGet);
      }

      // 试卷试题，用于统计考题失分情况
      public JsonResult GetTestChart(int? id)
      {
         var exam = db.es_exam.Find(id);
         var tests = db.es_stu_test.Where(e => e.es_stu_exam.exam == id).GroupBy(e => e.test)
            .Select(e => new { tid = e.Key, ok = e.Where(t => t.result == t.es_test.result).Count(), no = e.Where(t => t.result != t.es_test.result).Count() })
            .ToList();
         var composes = db.es_paper_compose.Where(e => e.paper == exam.paper).ToList();
         var json = (from c in composes
                     select new
                     {
                        id = c.id,
                        type = c.type,
                        typeName = c.es_test_type.name,
                        value = c.value,
                        number = c.number,
                        tests = (from t in Functions.GetTests(c.tests)
                                 select new
                                 {
                                    id = t.id,
                                    title = t.title,
                                    ok_count = (tests.Find(e => e.tid == t.id) == null) ? 0 : tests.Find(e => e.tid == t.id).ok,
                                    no_count = (tests.Find(e => e.tid == t.id) == null) ? 0 : tests.Find(e => e.tid == t.id).no,
                                    result = t.result,
                                    type = t.type,
                                    options = (from o in Functions.GetOptions(t.id)
                                               select new
                                               {
                                                  id = o.id,
                                                  abcd = o.abcd,
                                                  content = o.content
                                               }).ToArray()
                                 }).ToArray()
                     }).ToArray();

         return Json(json, JsonRequestBehavior.AllowGet);
      }

      public List<es_test> GetTests(string arrStr)
      {
         if (!string.IsNullOrEmpty(arrStr))
         {
            string[] arrString = arrStr.Split(',');
            int[] ids = Array.ConvertAll<string, int>(arrString, s => int.Parse(s));

            var tests = db.es_test.Where(t => ids.Contains(t.id)).ToList();

            return tests;
         }
         return new List<es_test>();
      }

      #endregion

      public JsonResult GetExams()
      {
         var exams = db.es_exam.Where(e => e.end_time < DateTime.Now).ToList();
         var json = new
         {
            exams = (from e in exams
                     select new
                     {
                        id = e.id,
                        paper = e.es_paper.name,
                        begin_time = e.begin_time.ToString(),
                        end_time = e.end_time.ToString()
                     }).ToArray()
         };
         return Json(json, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetStuExmaInfo(int eid)
      {
         var exams = db.es_stu_exam.Where(e => e.exam == eid).ToList();
         var json = new
         {
            exams = (from e in exams
                     select new
                     {
                        id = e.id,
                        student = e.student,
                        score = e.score
                     }).ToArray()
         };
         return Json(json, JsonRequestBehavior.AllowGet);
      }

      public ActionResult TestChart(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_exam exam = db.es_exam.Find(id);
         if (exam == null)
         {
            return HttpNotFound();
         }
         return View(exam);
      }

   }
}
