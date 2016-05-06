using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.BLL;
using Jobbplan.Model;

namespace Jobbplan.Controllers
{  [Authorize]
    public class ProsjektreqApiController : ApiController
    {
      
        private IProsjektLogikk _ProsjektBLL;

        public ProsjektreqApiController()
        {
            _ProsjektBLL = new ProsjektBLL();
        }
        public ProsjektreqApiController(IProsjektLogikk moqs)
        {
            _ProsjektBLL = moqs;
        }
        public List<ProsjektrequestMelding> Get()
        {
            string UserName = User.Identity.Name;
            return _ProsjektBLL.VisRequester(UserName);
        }
        public List<ProsjektrequestMelding> Get(int id)
        {
            string UserName = User.Identity.Name;
            return _ProsjektBLL.VisRequesterForProsjekt(id,UserName);
        }
        public HttpResponseMessage Post(ProsjektrequestSkjema reqInn)
        {
            string UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                bool ok = _ProsjektBLL.LeggTilBrukerRequest(reqInn, UserName);
                if (ok)
                {
                    var response = Request.CreateResponse(HttpStatusCode.Created, reqInn);
                    string uri = Url.Link("DefaultApi", null);
                    response.Headers.Location = new Uri(uri);
                    return response;
                }
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Kunne ikke sende melding")
                };
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
        public HttpResponseMessage Delete(int id)
        {
            string UserName = User.Identity.Name;
            bool ok = _ProsjektBLL.SlettRequest(id, UserName);
            if (ok)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Kunne ikke Slette bruker")
            };
        }
    }
}
