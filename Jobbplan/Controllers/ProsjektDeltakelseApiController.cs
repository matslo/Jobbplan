using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    public class ProsjektDeltakelseApiController : ApiController
    {
        DbTransaksjonerProsjekt db = new DbTransaksjonerProsjekt();
        public void Post(Prosjektrequest prosjektInn)
        {
            string username = User.Identity.Name;
            db.RegistrerProsjektdeltakelse(username);
       //     db.RegistrerProsjektdeltakelse(prosjektInn);
        }
    }
}
