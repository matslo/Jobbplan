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
       
        public ActionResult Testliste()
        {
            
            return View();
        }

        [ChildActionOnly]
        public ActionResult LoggetInn()// meny
        {
            if (HttpContext.User.Identity.Name != null)
            {
                string brukernavn = HttpContext.User.Identity.Name;
             
                ViewData["brukerpå"] = brukernavn;
                ViewBag.Innlogget = true;
                
            }
            else
            {
                ViewBag.Innlogget = false;
            }

            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult brukerListe()//Liste med ansatte
        {
            return PartialView();
        }

    }
}