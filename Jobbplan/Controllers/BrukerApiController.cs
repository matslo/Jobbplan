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
       
        // POST api/BrukerApi
        public void Post(Registrer personInn)
        {
            db.RegistrerBruker(personInn);
           
        }
       
    }
}
