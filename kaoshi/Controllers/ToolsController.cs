using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace kaoshi.Controllers
{
   public class Tools : Controller
   {
      // GET: Tools
      public ActionResult Index()
      {
         return View();
      }

      /// <summary>
      /// MD5加密
      /// </summary>
      /// <param name="pwd"></param>
      /// <returns></returns>
      public static string MD5(string pwd)
      {
         MD5 md5 = new MD5CryptoServiceProvider();
         byte[] palindata = Encoding.Default.GetBytes(pwd);//将要加密的字符串转换为字节数组
         byte[] encryptdata = md5.ComputeHash(palindata);//将字符串加密后也转换为字符数组
         return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串
      }
   }
}