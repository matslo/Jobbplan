using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Jobbplan.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home 
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Informasjonskapsler()
        {
            return View();
        }

        public ActionResult OmJobbplan()
        {
            return View();
        }
    }
}