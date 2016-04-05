using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    [Authorize]
    public class ProsjektController : Controller
    {
        DbTransaksjonerProsjekt db = new DbTransaksjonerProsjekt();
        // GET: Prosjekt
        public ActionResult Index()
        {
            return View();
        }       
        public ActionResult LeggTilBruker()
        {
            return View();
        }

        public ActionResult Meldingsboks()//Kategori meny
        {
           return View();
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