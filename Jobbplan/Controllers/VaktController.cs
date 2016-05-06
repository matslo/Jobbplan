using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jobbplan.Controllers
{
    [Authorize]
    public class VaktController : Controller
    {
        // GET: Vakt
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MinVaktplan()
        {
            return View();
        }
        public ActionResult VaktPlanTest()
        {
            return View();
        }
    }
}