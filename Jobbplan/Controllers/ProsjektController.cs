using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    public class ProsjektController : Controller
    {
        // GET: Prosjekt
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult LeggTil()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LeggTil(Prosjekt p)
        {
            DbTransaksjonerProsjekt db = new DbTransaksjonerProsjekt();
            string b = (string)Session["Brukernavn"];
            db.RegistrerProsjekt(p, b);
            return View();
        }
        
    }
}