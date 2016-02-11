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
        DbTransaksjonerProsjekt db = new DbTransaksjonerProsjekt();
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
           
            string b = (string)Session["Brukernavn"];
            db.RegistrerProsjekt(p, b);
            return View();
        }
        
        public ActionResult VisProsjekter()
        {
            string b = (string)Session["Brukernavn"];
            
            List<ProsjektVis> pro = db.HentProsjekter(b);
            return View(pro);
        }
        [ChildActionOnly]
        public ActionResult prosjektMeny()//Kategori meny
        {
            string b = (string)Session["Brukernavn"];
        
            List<ProsjektVis> pro = db.HentProsjekter(b);

            return PartialView(pro);
        }
    }
}