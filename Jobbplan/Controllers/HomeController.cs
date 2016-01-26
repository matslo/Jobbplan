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
        public ActionResult Index(Bruker.LogInn innBruker)
        {
           
            if (BrukerIdb(innBruker))
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
        private static bool BrukerIdb(Bruker.LogInn innBruker)
        {   //Sjekker om bruker er i db
            using (var db = new Dbkontekst())
            {
                byte[] passordDb = lagHash(innBruker.Passord);
                Bruker.dbBruker funnetBruker = db.Brukere.FirstOrDefault
                    (b => b.Passord == passordDb && b.Email == innBruker.Brukernavn);
                if (funnetBruker == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        private static byte[] lagHash(string innPassord)
        {
            //Hash passord
            byte[] innData, utData;
            var algoritme = System.Security.Cryptography.SHA256.Create();
            if (innPassord != null)
            {
                innData = System.Text.Encoding.ASCII.GetBytes(innPassord);
                utData = algoritme.ComputeHash(innData);
                return utData;

            }
            return null;
        }


    }
}