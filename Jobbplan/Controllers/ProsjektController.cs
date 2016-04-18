using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobbplan.BLL;
using Jobbplan.Model;

namespace Jobbplan.Controllers
{
    [Authorize]
    public class ProsjektController : Controller
    {
        private IProsjektLogikk _ProsjektBLL;

        public ProsjektController()
        {
            _ProsjektBLL = new ProsjektBLL();
        }
        
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
        
            List<ProsjektVis> pro = _ProsjektBLL.HentProsjekter(userName);

            return PartialView(pro);
        }
    }
}