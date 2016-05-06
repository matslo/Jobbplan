using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.BLL;
using Jobbplan.Model;

namespace Jobbplan.Controllers
{
    [Authorize]
    public class SjefController : ApiController
    {
        private IBrukerLogikk _BrukerBLL;

        public SjefController()
        {
            _BrukerBLL = new BrukerBLL();
        }
        public SjefController(IBrukerLogikk moqs)
        {
            _BrukerBLL = moqs;
        }
       
        public HttpResponseMessage Post(Sjef innBruker)
        {
            string userName = User.Identity.Name;
          
                bool ok = _BrukerBLL.GiBrukerAdminTilgang(innBruker, userName);
                if (ok)
                {
                var response = Request.CreateResponse(HttpStatusCode.Created, innBruker);
                string uri = Url.Link("DefaultApi", null);
                response.Headers.Location = new Uri(uri);
                return response;
                }
            
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Feil")
            };
        }
        public HttpResponseMessage Put(Sjef innBruker)
        {
            string userName = User.Identity.Name;

            bool ok = _BrukerBLL.FjernAdminTilgang(innBruker, userName);
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
        public HttpResponseMessage Delete(int id)
        {
            var dbtp = new ProsjektBLL();
            string userName = User.Identity.Name;

            bool ok = dbtp.SlettRequestSomAdmin(userName,id);
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
