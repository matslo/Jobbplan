using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobbplan.Models;
using System.Web.Security;

namespace Jobbplan.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home 
        public ActionResult Index()
        {
            return View();
        }
        /*
        [HttpPost]
        public ActionResult Index(LogInn innBruker)
        {
            var db = new DbTransaksjonerBruker();

            if (Membership.ValidateUser(UsernameTextbox.Text, PasswordTextbox.Text))
                FormsAuthentication.RedirectFromLoginPage(UsernameTextbox.Text, NotPublicCheckBox.Checked);
            else
                Msg.Text = "Login failed. Please check your user name and password and try again.";
             if (db.BrukerIdb(innBruker))
             {
                FormsAuthentication.RedirectFromLoginPage(innBruker.Brukernavn,false);
                Session["LoggetInn"] = true;
                string userName = HttpContext.User.Identity.Name;
                ViewBag.Innlogget = true;
                return RedirectToAction("Index","Vakt");
             }
             else
             {
                 Session["LoggetInn"] = false;
                 ViewBag.Innlogget = false;
                 return View();
            }
        }*/
        public ActionResult Loggut()
        {
            Session["LoggetInn"] = false;
            //Session["Brukernavn"] = null;
            return RedirectToAction("Index", "Home");

        }
        
    }
}