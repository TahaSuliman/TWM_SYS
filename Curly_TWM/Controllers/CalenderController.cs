using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Curly_TWM.Controllers
{
    public class CalenderController : Controller
    {
        // GET: Calender Admin
        public ActionResult Index()
        {
            return View();
        }


        // GET: Calender Initiative_Officer
        public ActionResult Initiative_Officer()
        {
            return View();
        }
    }
}