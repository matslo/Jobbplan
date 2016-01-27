using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home 
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LogInn innBruker)
        {
            var db = new DbTransaksjonerBruker();
           
            if (db.BrukerIdb(innBruker))
            {
                Session["LoggetInn"] = true;
                Session["Brukernavn"] = innBruker.Brukernavn; // legger brukernavnet i session
                ViewBag.Innlogget = true;
                return RedirectToAction("LoggetInn","Bruker");
            }
            else
            {
                Session["AdminInne"] = false;
                Session["LoggetInn"] = false;
                ViewBag.Innlogget = false;
                return RedirectToAction("Registrer", "Bruker");
            }
        }
    }
}