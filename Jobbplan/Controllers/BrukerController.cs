using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobbplan.Models;
using System.Web.Security;

namespace Jobbplan.Controllers
{
    [AllowAnonymous]
    public class BrukerController : Controller
    {   
        // GET: Kunde
        public ActionResult MinProfil()
        {
            return View();
        }
        public ActionResult RegistrerAPI()
        {
            return View();
        }    
        public void Loggut()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
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
    }
}