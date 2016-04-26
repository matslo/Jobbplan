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
    public class VaktApi3Controller : ApiController
    {
        private IVaktLogikk _VaktBLL;

        public VaktApi3Controller()
        {
            _VaktBLL = new VaktBLL();
        }
        public VaktApi3Controller(IVaktLogikk moq)
        {
            _VaktBLL = moq;
        }
        //Get api/KalenderApi
        public List<VaktRequestMelding> Get()
        {
            string brukernavn = User.Identity.Name;
            return _VaktBLL.visVaktRequester(brukernavn);
        }
        public List<Vaktkalender> Get(int id)
        {
            string brukernavn = User.Identity.Name;
            return _VaktBLL.hentAlleLedigeVakter(id, brukernavn);
        }
        public HttpResponseMessage Post(int id)
        {
            string brukernavn = User.Identity.Name;
           
            if (ModelState.IsValid)
            {
                bool ok = _VaktBLL.taLedigVakt(id, brukernavn);
                if (ok)
                {
                    var response = Request.CreateResponse(HttpStatusCode.Created, id);
                    string uri = Url.Link("DefaultApi", new { id = id });
                    response.Headers.Location = new Uri(uri);
                    return response;
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            /*return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Kunne ikke ta ledig vakt")
            };*/
        }

        public HttpResponseMessage Put(int id)
        {
            bool ok = _VaktBLL.requestOk(id);
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
                Content = new StringContent("Feil")
            };

        }

        public HttpResponseMessage Delete (int id)
        {
            bool ok = _VaktBLL.SlettVaktRequest(id);
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
                Content = new StringContent("Feil")
            };
        }
    }
}
