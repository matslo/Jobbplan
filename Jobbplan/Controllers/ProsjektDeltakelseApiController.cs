using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
   // [Authorize]
    public class ProsjektDeltakelseApiController : ApiController
    {
        DbTransaksjonerProsjekt db = new DbTransaksjonerProsjekt();
        public virtual HttpResponseMessage Post(ProsjektrequestMelding pid )
        {
            string userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                bool ok = db.RegistrerProsjektdeltakelse(pid, userName);
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
                Content = new StringContent("Kunne ikke sende melding")
            };
          
        }
        public void Delete(int id)
        {
            string username = User.Identity.Name;
            db.SlettBrukerFraProsjekt(username, id);
        }
    }
}
