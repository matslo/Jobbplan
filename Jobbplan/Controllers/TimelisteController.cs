using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jobbplan.Controllers
{
    [Authorize]
    public class TimelisteController : Controller
    {
        // GET: Timeliste
        public ActionResult Index()
        {
            return View();
        }
    }
}