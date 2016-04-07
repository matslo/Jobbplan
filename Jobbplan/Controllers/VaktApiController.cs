using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;
namespace Jobbplan.Controllers
{
    public class VaktApiController : ApiController
    {
        DbTransaksjonerVakt db = new DbTransaksjonerVakt();
        //Get api/KalenderApi
        public List<Vaktkalender> Get(int id)
        {
            string brukernavn = User.Identity.Name;
            return db.hentAlleVakter(id, brukernavn);
        }
        // POST api/KalenderApi
        public HttpResponseMessage Post(Vaktskjema vaktInn)
        {
            string userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                bool ok = db.RegistrerVakt(vaktInn, userName);
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
                Content = new StringContent("Kunne ikke opprette vakt")
            };
        }
   
        public void Put(int id)
        {
            string brukernavn = User.Identity.Name;
            db.taLedigVakt(id, brukernavn);
        }
        public HttpResponseMessage Delete(int id)
        {
            string userName = User.Identity.Name;
           
            bool ok = db.SlettVakt(id, userName);

            if (ok)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                };
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Kunne ikke slette vakt")
            };
        }
    }
}
