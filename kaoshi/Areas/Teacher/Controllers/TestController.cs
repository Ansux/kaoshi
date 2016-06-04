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
   public class TestController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         var es_test = db.es_test.Include(e => e.es_subject).Include(e => e.es_test_type);
         return View(es_test.ToList());
      }

      /// <summary>
      /// 根据条件获取题库 (科目，类型)
      /// </summary>
      /// <param name="sid">科目ID</param>
      /// <param name="tid">试题类型ID</param>
      /// <returns></returns>
      public JsonResult List(int sid = -1, int tid = -1)
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
                           subject = t.subject,
                           title = t.title,
                           result = t.result,
                           analyze = t.analyze,
                           isShow = true,
                           editFlag = false,
                           addOptionFlag = false,
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

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_test es_test = db.es_test.Find(id);
         if (es_test == null)
         {
            return HttpNotFound();
         }
         return View(es_test);
      }

      /// <summary>
      /// 获取 科目和类型下拉列表
      /// </summary>
      /// <returns></returns>
      public JsonResult getDropDownList()
      {
         var subjects = new SelectList(db.es_subject, "id", "name");
         var types = new SelectList(db.es_test_type, "id", "name");
         var json = new
         {
            subjects = subjects,
            types = types
         };
         return Json(json, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// 提交试题
      /// </summary>
      /// <param name="es_test"></param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult Create([Bind(Include = "id,title,type,result,subject")] es_test es_test)
      {
         if (ModelState.IsValid)
         {
            db.es_test.Add(es_test);
            db.SaveChanges();
            var test = db.es_test.Find(es_test.id);
            var json = new
            {
               id = es_test.id,
               subject = es_test.subject,
               type = es_test.type,
               title = es_test.title,
               result = es_test.result,
               flag = false,
               options = new { }
            };
            return Json(json);
         }

         ViewBag.subject = new SelectList(db.es_subject, "id", "name", es_test.subject);
         ViewBag.type = new SelectList(db.es_test_type, "id", "name", es_test.type);
         return Json(false);
      }

      /// <summary>
      /// 编辑试题信息
      /// </summary>
      /// <param name="es_test"></param>
      /// <returns></returns>
      [HttpPost]
      public ActionResult EditTest([Bind(Include = "id,title,type,result,analyze,subject")] es_test es_test)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_test).State = EntityState.Modified;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
         }
         return Json(false, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// 删除试题
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult Delete(int id)
      {
         es_test es_test = db.es_test.Find(id);
         var options = db.es_test_option.Where(o => o.test == id);
         foreach (var option in options)
         {
            db.es_test_option.Remove(option);
         }
         db.SaveChanges();

         db.es_test.Remove(es_test);
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
      /// 创建试题选项
      /// </summary>
      /// <param name="option"></param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult CreateOption([Bind(Include = "id,abcd,content,test")] es_test_option option)
      {
         if (ModelState.IsValid)
         {
            db.es_test_option.Add(option);
            db.SaveChanges();
            var o = db.es_test_option.Find(option.id);
            return Json(o, JsonRequestBehavior.AllowGet);
         }

         return Json(false, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// 编辑试题选项
      /// </summary>
      /// <param name="option"></param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult EditOption(es_test_option option)
      {
         if (ModelState.IsValid)
         {
            db.Entry(option).State = EntityState.Modified;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
         }
         return Json(false, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// 删除试题选项
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult DeleteOption(int id)
      {
         es_test_option option = db.es_test_option.Find(id);
         db.es_test_option.Remove(option);
         db.SaveChanges();
         return Json(true, JsonRequestBehavior.AllowGet);
      }
   }
}
