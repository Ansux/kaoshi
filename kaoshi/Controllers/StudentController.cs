using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kaoshi.Controllers
{
  public class StudentController : Controller
  {
    // GET: Teacher
    public ActionResult Index()
    {
      return Redirect("/student/account/signin");
    }
  }
}