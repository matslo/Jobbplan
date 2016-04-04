using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;
using System.Web.Security;


namespace Jobbplan.Controllers
{
    public class AuthenticateController : ApiController
    {
        DbTransaksjonerBruker db = new DbTransaksjonerBruker();
        public HttpResponseMessage Post(LogInn personInn)
        {
            if (ModelState.IsValid)
            {
                bool ok = db.BrukerIdb(personInn);
                if (ok)
                {
                    FormsAuthentication.RedirectFromLoginPage(personInn.Brukernavn, false);
                    
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                    };             
                }
            }


            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Ikke gyldig brukernavn/ passord")
            };
        }
    }
}
