using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kaoshi.Models
{
   public class Functions
   {
      static WebContext db = new WebContext();

      /// <summary>
      /// 通过组卷中的tests，拆分所包含的题目ids，并返回题目列表
      /// </summary>
      /// <param name="arrStr"></param>
      /// <returns></returns>
      public static List<es_test> GetTests(string arrStr)
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
      /// 获取题目选项
      /// </summary>
      /// <param name="test"></param>
      /// <returns></returns>
      public static List<es_test_option> GetOptions(int test)
      {
         var options = db.es_test_option.Where(o => o.test == test).ToList();
         return options;
      }
   }
}