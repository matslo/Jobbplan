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
       [Authorize]
        public ActionResult LeggTil()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LeggTil(Prosjekt p)
        {

            string userName = HttpContext.User.Identity.Name;
            db.RegistrerProsjekt(p, userName);
            return View();
        }
        [Authorize]
        public ActionResult VisProsjekter()
        {

            string userName = HttpContext.User.Identity.Name;

            List<ProsjektVis> pro = db.HentProsjekter(userName);
            return View(pro);
        }
        [ChildActionOnly]
        public ActionResult prosjektMeny()//Kategori meny
        {
            string userName = HttpContext.User.Identity.Name;
        
            List<ProsjektVis> pro = db.HentProsjekter(userName);

            return PartialView(pro);
        }
    }
}