using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    public class BrukerApiController : ApiController
    {
       DbTransaksjonerBruker db = new DbTransaksjonerBruker();
       public List<BrukerListe> Get (int id)
       {
            return db.HentBrukere(id);
       }
        // POST api/BrukerApi
        public void Post(Registrer personInn)
        {
            
            db.RegistrerBruker(personInn);
            
        }
       
    }
}
