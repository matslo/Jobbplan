using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobbplan.Models;
using System.Web.Security;

namespace Jobbplan.Controllers
{   
    public class BrukerController : Controller
    {
        
       /* public BrukerController(DbTransaksjonerBrukerStub stub)
        {
            var dbStub = stub;
        }*/
        // GET: Kunde
        public ActionResult Index()   
        {
            return View();
        } 
        [AllowAnonymous]
        public ActionResult RegistrerAPI()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Registrer()
        {
            return View();
        }
        public ActionResult Loggut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
        
        [HttpPost]
        public ActionResult Registrer(Registrer innBruker)
        {
            if (ModelState.IsValid)
            {
                var db = new DbTransaksjonerBruker();
                bool insertOk = db.RegistrerBruker(innBruker);
                if (insertOk)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }
        [Authorize]
        public ActionResult LoggetInn()
        {
            string userName = HttpContext.User.Identity.Name;
            ViewData["brukerpå"] = userName;
            return View();
        }
        public ActionResult Testliste()
        {
            
            return View();
        }

        [ChildActionOnly]
        public ActionResult brukerListe()//Liste med ansatte
        {
            return PartialView();
        }

    }
}