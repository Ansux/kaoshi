using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kaoshi.Filters
{
   public class AdminLoginAuthorize : ActionFilterAttribute
   {
      public override void OnActionExecuting(ActionExecutingContext filterContext)
      {
         if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
               || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
         {
            return;
         }

         if (filterContext.HttpContext.Session["Mid"] == null)
         {
            string redirectOnSuccess = filterContext.HttpContext.Request.RawUrl;
            string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
            string loginUrl = "~/admin/account/signin" + redirectUrl;
            filterContext.Result = new RedirectResult(loginUrl, true);
         }
      }
   }
}