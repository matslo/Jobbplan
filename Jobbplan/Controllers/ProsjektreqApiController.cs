using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    public class ProsjektreqApiController : ApiController
    {
        DbTransaksjonerProsjekt db = new DbTransaksjonerProsjekt();
        public List<ProsjektrequestMelding> Get()
        {
            string UserName = User.Identity.Name;
            return db.VisRequester(UserName);
        }
        public List<ProsjektrequestMelding> Get(int id)
        {
            string UserName = User.Identity.Name;
            return db.VisRequesterForProsjekt(id,UserName);
        }

        public HttpResponseMessage Post(ProsjektrequestSkjema reqInn)
        {
            string UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                bool ok = db.LeggTilBrukerRequest(reqInn, UserName);
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
         
        public List<ProsjektrequestMelding> Delete(int id)
        {
            string UserName = User.Identity.Name;
            db.SlettRequest(id,UserName);
            return db.VisRequester(UserName);
        }
    }
}
