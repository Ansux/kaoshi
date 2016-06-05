using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using kaoshi.Models;
using kaoshi.Areas.Teacher.Models;

namespace kaoshi.Areas.Teacher.Controllers
{
   [Filters.TeacherLoginAuthorize]
   public class PaperComposeController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         var es_paper_compose = db.es_paper_compose.Include(e => e.es_paper).Include(e => e.es_test_type);
         return View(es_paper_compose.ToList());
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_paper_compose es_paper_compose = db.es_paper_compose.Find(id);
         if (es_paper_compose == null)
         {
            return HttpNotFound();
         }
         return View(es_paper_compose);
      }

      /// <summary>
      /// 获取试卷相关信息，包括试卷组卷
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public JsonResult GetPaperInfo(int id)
      {
         var paper = db.es_paper.Find(id);
         var composes = db.es_paper_compose.Where(c => c.paper == id).ToList();
         var json = new
         {
            id = paper.id,
            name = paper.name,
            ab_paging = paper.ab_paging,
            test_time = paper.test_time,
            status = paper.status,
            total_points = paper.total_points,
            composes = (from c in composes
                        select new
                        {
                           id = c.id,
                           tid = c.type,
                           tname = c.es_test_type.name,
                           value = c.value,
                           number = c.number,
                           tests = c.tests,
                           editFlag = false
                        }).ToArray()
         };
         return Json(json, JsonRequestBehavior.AllowGet);
      }

      public ActionResult Create(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         ViewBag.paper = id;
         return View();
      }

      /// <summary>
      /// 添加试卷组卷部分，包括(单选，多选，判断题)
      /// </summary>
      /// <param name="compose"></param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult Create([Bind(Include = "id,type,value,number,paper,tests")] es_paper_compose compose)
      {
         if (ModelState.IsValid)
         {
            db.es_paper_compose.Add(compose);
            db.SaveChanges();
            var c = db.es_paper_compose.Find(compose.id);
            var json = new
            {
               id = c.id,
               type = c.type,
               tname = db.es_test_type.Find(c.type).name,
               value = c.value,
               number = c.number,
               tests = c.tests
            };
            return Json(json, JsonRequestBehavior.AllowGet);
         }

         return Json(false, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// 获取试题类型 (单选，多选，判断)，用于跟试卷中原有的类型作比较，进行筛选
      /// </summary>
      /// <returns></returns>
      public JsonResult getTypeList()
      {
         var types = db.es_test_type.ToList();
         var json = new
         {
            types = (from t in types
                     select new
                     {
                        id = t.id,
                        name = t.name,
                        flag = false
                     }).ToArray()
         };
         return Json(json, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// 保存试卷组卷部分
      /// </summary>
      /// <param name="compose"></param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult Save([Bind(Include = "id,type,value,number,paper")] es_paper_compose compose)
      {
         if (ModelState.IsValid)
         {
            db.Entry(compose).State = EntityState.Modified;
            db.Entry(compose).Property(c => c.tests).IsModified = false;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
         }
         return Json(false, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// 删除试卷组卷部分
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult Delete(int id)
      {
         es_paper_compose es_paper_compose = db.es_paper_compose.Find(id);
         db.es_paper_compose.Remove(es_paper_compose);
         db.SaveChanges();
         return Json(true, JsonRequestBehavior.AllowGet);
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
      /// 将试题添加到试卷的组卷中。
      /// </summary>
      /// <param name="composeId"></param>
      /// <param name="testId"></param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult AddTest(int composeId, int testId)
      {
         var compose = db.es_paper_compose.Find(composeId);
         var tests = compose.tests;
         db.Entry(compose).State = EntityState.Unchanged;
         db.Entry(compose).Property(c => c.tests).IsModified = true;
         if (string.IsNullOrEmpty(tests))
         {
            compose.tests = testId.ToString();
         }
         else {
            compose.tests += "," + testId;
         }
         db.SaveChanges();
         return Json(true);
      }

      /// <summary>
      /// 将试题从组卷中移除
      /// </summary>
      /// <param name="composeId"></param>
      /// <param name="testId"></param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult RemoveTest(int composeId, int testId)
      {
         var compose = db.es_paper_compose.Find(composeId);
         var tests = compose.tests;
         db.Entry(compose).State = EntityState.Unchanged;
         db.Entry(compose).Property(c => c.tests).IsModified = true;

         var composes = tests.Split(',').ToList();

         for (var i = 0; i < composes.Count; i++)
         {
            if (composes[i] == testId.ToString())
            {
               composes.RemoveAt(i);
            }
         }

         compose.tests = string.Join(",", composes.ToArray());
         db.SaveChanges();

         return Json(true);
      }

      /// <summary>
      /// 根据条件获取题库 (科目，类型)
      /// </summary>
      /// <param name="sid">科目ID</param>
      /// <param name="tid">试题类型ID</param>
      /// <returns></returns>
      public JsonResult GetTestList(int sid = -1, int tid = -1)
      {
         var tests = db.es_test.ToList();
         if (sid > -1)
         {
            tests = tests.Where(t => t.subject == sid).ToList();
         }
         if (tid > -1)
         {
            tests = tests.Where(t => t.type == tid).ToList();
         }

         var options = db.es_test_option.ToList();

         if (tests != null)
         {
            var json = new
            {
               tests = (from t in tests
                        select new
                        {
                           id = t.id,
                           type = t.type,
                           title = t.title,
                           result = t.result,
                           options = (from o in options
                                      where o.test == t.id
                                      orderby o.abcd
                                      select new
                                      {
                                         id = o.id,
                                         abcd = o.abcd,
                                         content = o.content,
                                         test = o.test,
                                         editFlag = false
                                      }).ToArray()
                        }).ToArray()
            };
            return Json(json, JsonRequestBehavior.AllowGet);
         }
         return Json(false, JsonRequestBehavior.AllowGet);
      }
   }
}
