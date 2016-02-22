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

        public void Post(ProsjektrequestSkjema reqInn)
        {
            string UserName = User.Identity.Name;
            db.LeggTilBrukerRequest(reqInn,UserName);
        }
        public List<ProsjektrequestMelding> Delete(int id)
        {
            string UserName = User.Identity.Name;
            db.SlettRequest(id,UserName);
            return db.VisRequester(UserName);
        }
    }
}
