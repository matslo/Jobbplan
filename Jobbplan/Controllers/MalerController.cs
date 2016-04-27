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
    public class MalerController : ApiController
    {
        private IVaktLogikk _VaktBLL;

        public MalerController()
        {
            _VaktBLL = new VaktBLL();
        }
        public MalerController(IVaktLogikk moqs)
        {
            _VaktBLL = moqs;
        }

        public List<VisMaler> Get(int id)
        {
            string brukernavn = User.Identity.Name;
            return _VaktBLL.hentAlleMaler(id, brukernavn);
        }
        // POST api/KalenderApi
        public HttpResponseMessage Post(MalerSkjema mal)
        {
            string userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                bool ok = _VaktBLL.RegistrerMal(mal, userName);
                if (ok)
                {
                    var response = Request.CreateResponse(HttpStatusCode.Created, mal);
                    string uri = Url.Link("DefaultApi",null);
                    response.Headers.Location = new Uri(uri);
                    return response;
                }
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Kunne ikke sette inn databasen")
                };
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
    }
}
