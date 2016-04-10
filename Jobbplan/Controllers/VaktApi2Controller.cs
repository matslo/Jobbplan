using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    public class VaktApi2Controller : ApiController
    {
        DbTransaksjonerVakt db = new DbTransaksjonerVakt();
        //Get api/KalenderApi
        public List<Vaktkalender> Get(int id)
        {
            string brukernavn = User.Identity.Name;
            return db.hentAlleVakterBruker(id, brukernavn);
        }
      
        public HttpResponseMessage Put(Vaktskjema endrevakt)
        {
            string userName = User.Identity.Name;
      
            if (ModelState.IsValid)
            {
                bool ok = db.EndreVakt(endrevakt, userName);
                if (ok)
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.Created,
                    };
                }
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Kunne ikke endre vakt")
            };

        }
    }
}
