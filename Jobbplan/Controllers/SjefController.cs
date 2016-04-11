using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
        public HttpResponseMessage Post(Sjef innBruker)
        {
            string userName = User.Identity.Name;
          
                bool ok = db.GiBrukerAdminTilgang(innBruker, userName);
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
        public HttpResponseMessage Put(Sjef innBruker)
        {
            string userName = User.Identity.Name;

            bool ok = db.FjernAdminTilgang(innBruker, userName);
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
            var dbtp = new DbTransaksjonerProsjekt();
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
