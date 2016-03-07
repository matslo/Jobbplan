using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Jobbplan.Controllers
{
    public class BrukerApiController : ApiController
    {
       DbTransaksjonerBruker db = new DbTransaksjonerBruker();
        // GET api/BrukerApi/4
        public List<BrukerListe> Get (int id)
       {
            string brukernavn = User.Identity.Name;
            return db.HentBrukere(id,brukernavn);
       }
        // POST api/BrukerApi
        public HttpResponseMessage Post(Registrer personInn)
        {
            if (ModelState.IsValid)
            {
               
                bool ok = db.RegistrerBruker(personInn);   
                 if (ok)
                 {
                 return  new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,    
                    };

                 }
            }


            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Kunne ikke sette inn kunden i DB")
            };
       }
       
    }
}
