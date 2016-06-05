using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using kaoshi.Models;

namespace kaoshi.Areas.Student.Controllers
{
   [Filters.StuLoginAuthorize]
   public class ExamController : Controller
   {
      private WebContext db = new WebContext();

      /// <summary>
      /// 学生所有的考试信息，包括即将进行的考试和完成的考试
      /// </summary>
      /// <returns></returns>
      public ActionResult Index()
      {
         var es_exam = db.es_exam.Where(e => e.end_time > DateTime.Now).Include(e => e.es_paper);

         return View(es_exam.ToList());
      }

      /// <summary>
      /// 获取学生考试记录
      /// </summary>
      /// <returns></returns>
      public ActionResult Record()
      {
         var sno = (int)Session["Sno"];
         var exams = db.es_stu_exam.Where(e=>e.end_time!=null && e.student == sno).ToList();
         return View(exams);
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_stu_exam es_stu_exam = db.es_stu_exam.Find(id);
         if (es_stu_exam == null)
         {
            return HttpNotFound();
         }
         return View(es_stu_exam);
      }

      /// <summary>
      /// 学生考试页面
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public ActionResult Testing(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         var exam = db.es_exam.Find(id);
         es_paper es_paper = db.es_paper.Find(exam.paper);
         if (es_paper == null)
         {
            return HttpNotFound();
         }

         // 创建考试记录，用于存储考试过程
         var sno = (int)Session["Sno"];
         var stu_exam = db.es_stu_exam.Where(p => p.student == sno && p.exam == id).FirstOrDefault();
         if (stu_exam == null)
         {
            var examObj = new es_stu_exam();
            examObj.student = sno;
            examObj.exam = (int)id;
            examObj.begin_time = DateTime.Now;
            db.es_stu_exam.Add(examObj);
            db.SaveChanges();
            ViewData["eid"] = examObj.id;
         }
         else
         {
            ViewData["eid"] = stu_exam.id;
         }

         // 传入试卷ID
         ViewData["pid"] = es_paper.id;

         return View(exam);
      }

      #region 获取试卷试题
      /// <summary>
      /// 通过试卷ID获取整套试卷的题目列表
      /// </summary>
      /// <param name="pid"></param>
      /// <returns></returns>
      public JsonResult GetPaperTests(int? pid)
      {
         var composes = db.es_paper_compose.Where(p => p.paper == pid).ToList();
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
            return Json(json, JsonRequestBehavior.AllowGet);
         }
         return Json(false, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// 通过试题ID字符连接串，将试题ID转数组，并将试题列表返回
      /// </summary>
      /// <param name="arrStr"></param>
      /// <returns></returns>
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

      /// <summary>
      /// 通过试题ID，获取试题的选项
      /// </summary>
      /// <param name="test"></param>
      /// <returns></returns>
      public List<es_test_option> getOptions(int test)
      {
         var options = db.es_test_option.Where(o => o.test == test).ToList();
         return options;
      }
      #endregion

      /// <summary>
      /// 通过考试记录的ID，获取学生参加本场考试的所有试题答题记录
      /// </summary>
      /// <param name="eid"></param>
      /// <returns></returns>
      public JsonResult GetStuTest(int eid)
      {
         var tests = db.es_stu_test.Where(t => t.exam == eid).ToList();
         var json = new
         {
            tests = (from t in tests
                     select new
                     {
                        id = t.id,
                        test = t.test,
                        result = t.result
                     }).ToArray()
         };

         return Json(json, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// 保存学生答题的所选的选项
      /// </summary>
      /// <param name="eid"></param>
      /// <param name="tid"></param>
      /// <param name="option"></param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult SaveUserOption(int eid, int tid, string option)
      {
         var test = db.es_stu_test.Where(t => t.exam == eid && t.test == tid).FirstOrDefault();
         if (test == null)
         {
            var tobj = new es_stu_test();
            tobj.exam = eid;
            tobj.test = tid;
            tobj.result = option;
            db.es_stu_test.Add(tobj);
            db.SaveChanges();
         }
         else
         {
            db.Entry(test).State = EntityState.Modified;
            test.result = option;
            db.SaveChanges();
         }
         var json = new
         {
            eid = eid,
            tid = tid,
            option = option
         };

         return Json(json);
      }

      /// <summary>
      /// 交卷。并将考试成绩记录到数据库中
      /// </summary>
      /// <param name="eid"></param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult Submit(int eid)
      {
         var exam = db.es_stu_exam.Find(eid);
         if (exam == null)
         {
            return Json(false);
         }

         var stuTests = db.es_stu_test.Where(t => t.exam == eid);
         decimal score = 0;
         foreach (var t in stuTests)
         {
            if (t.es_test.result == t.result)
            {
               score += (decimal)t.es_stu_exam.es_exam.es_paper.es_paper_compose.FirstOrDefault().value;
            };
         };

         // 更新用户试卷
         db.Entry(exam).State = EntityState.Modified;
         exam.end_time = DateTime.Now;
         exam.score = score;
         db.SaveChanges();

         return Json(true);

      }

      /// <summary>
      /// 通过考试ID，获取考试的考试结果
      /// </summary>
      /// <param name="eid"></param>
      /// <returns></returns>
      public ActionResult Result(int? eid)
      {
         if (eid == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         var exam = db.es_stu_exam.Find(eid);
         es_paper paper = db.es_paper.Find(exam.es_exam.paper);
         if (paper == null)
         {
            return HttpNotFound();
         }

         ViewData["eid"] = exam.id;
         ViewData["pid"] = paper.id;

         return View(exam);
      }

      /// <summary>
      /// 考试须知
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public ActionResult Instructions(int? id)
      {
         ViewBag.id = id;
         return View();
      }

   }
}
