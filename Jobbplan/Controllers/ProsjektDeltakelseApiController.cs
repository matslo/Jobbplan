using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    [Authorize]
    public class ProsjektDeltakelseApiController : ApiController
    {
        DbTransaksjonerProsjekt db = new DbTransaksjonerProsjekt();
        public void Post(ProsjektrequestMelding pid )
        {
            string username = User.Identity.Name;
            db.RegistrerProsjektdeltakelse(pid, username);
        }
    }
}
