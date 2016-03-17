using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;
using Jobbplan.Controllers;

namespace Jobbplan.Controllers
{
    public class ProfilController : ApiController
    {
        DbTransaksjonerBruker db = new DbTransaksjonerBruker();

        public List<dbBruker> Get()
        {
            string brukernavn = User.Identity.Name;
            return db.HentBruker(brukernavn);
        }
    }
}
