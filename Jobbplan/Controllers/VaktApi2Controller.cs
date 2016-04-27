using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.BLL;
using Jobbplan.Model;

namespace Jobbplan.Controllers
{
    public class VaktApi2Controller : ApiController
    {
        private IVaktLogikk _VaktBLL;

        public VaktApi2Controller()
        {
            _VaktBLL = new VaktBLL();
        }
        public VaktApi2Controller(IVaktLogikk moqs)
        {
            _VaktBLL = moqs;
        }

        //Get api/KalenderApi
        public List<Vaktkalender> Get(int id)
        {
            string brukernavn = User.Identity.Name;
            return _VaktBLL.hentAlleVakterBruker(id, brukernavn);
        }
      
        public HttpResponseMessage Put(Vaktskjema endrevakt)
        {
            string userName = User.Identity.Name;
      
            if (ModelState.IsValid)
            {
                bool ok = _VaktBLL.EndreVakt(endrevakt, userName);
                if (ok)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, endrevakt);
                }
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Kunne ikke endre vakt")
                };
            }
          
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

        }
    }
}
