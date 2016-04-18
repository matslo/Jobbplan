using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Jobbplan.Model;
using Jobbplan.BLL;

namespace Jobbplan.Controllers
{
    public class BrukerApiController : ApiController
    {
        private IBrukerLogikk _BrukerBLL;

        public BrukerApiController()
        {
            _BrukerBLL = new BrukerBLL();
        }
        
        public int Get()
        {
            string Username = User.Identity.Name;
            return _BrukerBLL.AntallMeldinger(Username);
        }
        // GET api/BrukerApi/4
        public List<BrukerListe> Get (int id)
       {
            string brukernavn = User.Identity.Name;
            return _BrukerBLL.HentBrukere(id,brukernavn);
          
        }
        // POST api/BrukerApi
        public HttpResponseMessage Post(Registrer personInn)
        {

            if (ModelState.IsValid)
            {
                bool ok = _BrukerBLL.RegistrerBruker(personInn);   
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
                Content = new StringContent("Kunne ikke sette inn databasen")
            };
       }
        public void Delete (int id)
        {
            //SLETT BRUKER
        }       
    }
}
