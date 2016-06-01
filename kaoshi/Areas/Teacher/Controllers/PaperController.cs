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
   // [Filters.TeacherLoginAuthorize]
   public class PaperController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         var es_paper = db.es_paper.Include(e => e.es_subject);
         return View(es_paper.ToList());
      }

      public ActionResult Statistics()
      {
         return View();
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_paper es_paper = db.es_paper.Find(id);
         if (es_paper == null)
         {
            return HttpNotFound();
         }
         return View(es_paper);
      }

      public ActionResult Create()
      {
         ViewBag.subject = new SelectList(db.es_subject, "id", "name");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,name,ab_paging,test_time,total_points,subject")] es_paper es_paper)
      {
         if (ModelState.IsValid)
         {
            es_paper.create_at = DateTime.Now;
            es_paper.status = 1;
            db.es_paper.Add(es_paper);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.subject = new SelectList(db.es_subject, "id", "name", es_paper.subject);
         return View(es_paper);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_paper es_paper = db.es_paper.Find(id);
         if (es_paper == null)
         {
            return HttpNotFound();
         }

         ViewBag.subject = new SelectList(db.es_subject, "id", "name", es_paper.subject);
         // 判断此试卷中是否已存在题目，若已存在则不允许修改试卷的科目
         var composes = db.es_paper_compose.Where(c => c.paper == es_paper.id);
         if (composes != null)
         {
            foreach (var c in composes.ToList())
            {
               if (c.tests != null)
               {
                  ViewData["flag"] = true;
               }
            }
         }

         return View(es_paper);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,name,ab_paging,test_time,total_points,subject")] es_paper es_paper)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_paper).State = EntityState.Unchanged;
            db.Entry(es_paper).Property(p => p.update_at).IsModified = true;
            db.Entry(es_paper).Property(p => p.name).IsModified = true;
            db.Entry(es_paper).Property(p => p.ab_paging).IsModified = true;
            db.Entry(es_paper).Property(p => p.test_time).IsModified = true;
            db.Entry(es_paper).Property(p => p.total_points).IsModified = true;

            if (es_paper.subject > 0)
            {
               db.Entry(es_paper).Property(p => p.subject).IsModified = true;
            }

            es_paper.update_at = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.subject = new SelectList(db.es_subject, "id", "name", es_paper.subject);
         return View(es_paper);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_paper es_paper = db.es_paper.Find(id);
         if (es_paper == null)
         {
            return HttpNotFound();
         }
         return View(es_paper);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_paper es_paper = db.es_paper.Find(id);
         db.es_paper.Remove(es_paper);
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

      /// <summary>
      /// 组卷
      /// </summary>
      /// <returns></returns>
      public ActionResult Compose(int id)
      {
         return RedirectToAction("Create", "PaperCompose", new { id = id });
      }

      /// <summary>
      /// 获取试卷组卷列表
      /// </summary>
      /// <param name="id">试卷ID</param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult GetComposes(int id)
      {
         var composes = db.es_paper_compose.Where(c => c.paper == id).ToList();
         if (composes != null)
         {
            var json = new
            {
               composes = (from c in composes
                           select new
                           {
                              id = c.id,
                              type = c.type,
                              typeName = c.es_test_type.name,
                              value = c.value,
                              number = c.number,
                              tests = (from t in getTests(c.tests)
                                       select new
                                       {
                                          id = t.id,
                                          title = t.title,
                                          result = t.result,
                                          type = t.type,
                                          options = (from o in getOptions(t.id)
                                                     select new
                                                     {
                                                        id = o.id,
                                                        abcd = o.abcd,
                                                        content = o.content
                                                     }).ToArray()
                                       }).ToArray()
                           }).ToArray()
            };
            return Json(json);
         }
         return Json(false);
      }

      public List<es_test> getTests(string arrStr)
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

      public List<es_test_option> getOptions(int test)
      {
         var options = db.es_test_option.Where(o => o.test == test).ToList();
         return options;
      }

      /// <summary>
      /// 进入选题中心
      /// </summary>
      /// <param name="pid">试卷ID</param>
      /// <returns></returns>
      public ActionResult SelectTopic(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_paper es_paper = db.es_paper.Find(id);
         if (es_paper == null)
         {
            return HttpNotFound();
         }
         return View(es_paper);
      }

      /// <summary>
      /// 随机选题
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public ActionResult MachineSelectTopic(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_paper paper = db.es_paper.Find(id);
         if (paper == null)
         {
            return HttpNotFound();
         }

         return View(paper);
      }

      [HttpPost]
      public JsonResult Confirm(int id)
      {
         var paper = db.es_paper.Find(id);
         db.Entry(paper).State = EntityState.Unchanged;
         db.Entry(paper).Property(p => p.status).IsModified = true;
         paper.status = 2;
         db.SaveChanges();
         return Json(true);
      }

      [HttpPost]
      public JsonResult Complete(int id)
      {
         var paper = db.es_paper.Find(id);
         db.Entry(paper).State = EntityState.Unchanged;
         db.Entry(paper).Property(p => p.status).IsModified = true;
         paper.status = 3;
         db.SaveChanges();
         return Json(true);
      }

      public JsonResult GetTypeTest(int subject, int tid)
      {
         var tests = db.es_test.Where(e => e.subject == subject && e.type == tid);
         var json = (from t in tests
                     select new
                     {
                        id = t.id,
                        title = t.title
                     }).ToArray();
         return Json(json, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult MachineSelect(int? id)
      {
         if (id == null)
         {
            return Json(false);
         }
         es_paper paper = db.es_paper.Find(id);
         if (paper == null)
         {
            return Json(false);
         }

         var composes = db.es_paper_compose.Where(e => e.paper == paper.id).ToList();

         foreach (var c in composes)
         {
            var tests = db.es_test.Where(e => e.subject == paper.subject && e.type == c.type).ToList();
            var tString = "";
            var tempArr = new List<int>();
            Random random = new Random();
            for (int i = 0; i < c.number; i++)
            {
               while (true)
               {
                  var tid = tests[random.Next(tests.Count)].id;
                  if (!tempArr.Contains(tid))
                  {
                     tempArr.Add(tid);
                     tString += tid + ",";
                     break;
                  }
               }
            }
            tString = tString.Substring(0, tString.Length - 1);
            db.Entry(c).State = EntityState.Unchanged;
            db.Entry(c).Property(e => e.tests).IsModified = true;
            c.tests = tString;
         }
         db.SaveChanges();

         return Json(GetComposesByPaper((int)id));
      }

      public object GetComposesByPaper(int pid)
      {
         return new
         {
            composes = (from c in db.es_paper_compose.Where(e => e.paper == pid).ToList()
                        select new
                        {
                           id = c.id,
                           type = c.type,
                           typeName = c.es_test_type.name,
                           value = c.value,
                           number = c.number,
                           tests = (from t in getTests(c.tests)
                                    select new
                                    {
                                       id = t.id,
                                       title = t.title,
                                       result = t.result,
                                       type = t.type,
                                       options = (from o in getOptions(t.id)
                                                  select new
                                                  {
                                                     id = o.id,
                                                     abcd = o.abcd,
                                                     content = o.content
                                                  }).ToArray()
                                    }).ToArray()
                        }).ToArray()
         };
      }
   }
}
