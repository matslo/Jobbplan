using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Model;
using System.Web.Security;
using Jobbplan.BLL;


namespace Jobbplan.Controllers
{
    public class AuthenticateController : ApiController
    {
        private IBrukerLogikk _BrukerBLL;

        public AuthenticateController()
        {
            _BrukerBLL = new BrukerBLL();
        }
        public AuthenticateController(IBrukerLogikk moqs)
        {
            _BrukerBLL = moqs;
        }
        public HttpResponseMessage Post(LogInn personInn)
        {
            if (ModelState.IsValid)
            {
                bool ok = _BrukerBLL.BrukerIdb(personInn);
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
