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

        public List<Profil> Get()
        {
            string brukernavn = User.Identity.Name;
            return db.HentBruker(brukernavn);
        }
        public HttpResponseMessage Put(Profil EndreBrukerInfo)
        {
            string userName = User.Identity.Name;            
            if (ModelState.IsValid)
            {
                bool ok = db.EndreBrukerInfo(EndreBrukerInfo, userName);
                if (ok)
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                    };
                }
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Kunne ikke sette inn databasen")
            };
        }
    }
}
