using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    public class MalerController : ApiController
    {
        DbTransaksjonerVakt db = new DbTransaksjonerVakt();
        public List<VisMaler> Get(int id)
        {
            string brukernavn = User.Identity.Name;
            return db.hentAlleMaler(id, brukernavn);
        }
        // POST api/KalenderApi
        public HttpResponseMessage Post(MalerSkjema mal)
        {
            string userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                bool ok = db.RegistrerMal(mal, userName);
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
                Content = new StringContent("Kunne ikke opprette mal")
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
                Content = new StringContent("Kunne ikke slette mal")
            };
        }
    }
}
