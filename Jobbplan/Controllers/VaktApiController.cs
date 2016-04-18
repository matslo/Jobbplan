using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Model;
using Jobbplan.BLL;

namespace Jobbplan.Controllers
{
    public class VaktApiController : ApiController
    {
        private IVaktLogikk _VaktBLL;

        public VaktApiController()
        {
            _VaktBLL = new VaktBLL();
        }

       
        //Get api/KalenderApi
        public List<Vaktkalender> Get()
        {
            string brukernavn = User.Identity.Name;
            return _VaktBLL.hentAlleVakterForBruker(brukernavn);
        }
        public List<Vaktkalender> Get(int id)
        {
            string brukernavn = User.Identity.Name;
            return _VaktBLL.hentAlleVakter(id, brukernavn);
        }
        // POST api/KalenderApi
        public HttpResponseMessage Post(Vaktskjema vaktInn)
        {
            string userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                bool ok = _VaktBLL.RegistrerVakt(vaktInn, userName);
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
        public HttpResponseMessage Put(int id)
        {
            string brukernavn = User.Identity.Name;

            bool ok = _VaktBLL.taLedigVakt(id, brukernavn);
            if (ok)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Created,
                };
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Kunne ikke ta ledig vakt")
            };
        }
        public HttpResponseMessage Delete(int id)
        {
            string userName = User.Identity.Name;
           
            bool ok = _VaktBLL.SlettVakt(id, userName);

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
