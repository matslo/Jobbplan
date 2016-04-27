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
   // [Authorize]
    public class ProsjektDeltakelseApiController : ApiController
    {
        private IProsjektLogikk _ProsjektBLL;

        public ProsjektDeltakelseApiController()
        {
            _ProsjektBLL = new ProsjektBLL();
        }
       
        public virtual HttpResponseMessage Post(ProsjektrequestMelding pid )
        {
            string userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                bool ok = _ProsjektBLL.RegistrerProsjektdeltakelse(pid, userName);
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
                Content = new StringContent("Kunne ikke sende melding")
            };
          
        }
        public HttpResponseMessage Delete(int id)
        {
            string username = User.Identity.Name;
            bool ok = _ProsjektBLL.SlettBrukerFraProsjekt(username, id);
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
                Content = new StringContent("Kunne ikke slette bruker")
            };
        }
    }
}
