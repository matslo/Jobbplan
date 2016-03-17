using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;
namespace Jobbplan.Controllers
{
    public class SjefController : ApiController
    {
        DbTransaksjonerBruker db = new DbTransaksjonerBruker();
        public HttpResponseMessage Post(Sjef personInn)
        {
            bool ok = db.GiBrukerAdminTilgang(personInn);
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
                Content = new StringContent("Kunne ikke sette inn kunden i DB")
            };
        }
        public HttpResponseMessage Delete(int id)
        {
            DbTransaksjonerProsjekt dbTp = new DbTransaksjonerProsjekt();
            string UserName = User.Identity.Name;
            bool ok = dbTp.SlettRequestSomAdmin(UserName, id);
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
                Content = new StringContent("Kunne ikke slette request")
            };
        }
    }
}
