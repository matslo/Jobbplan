using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    public class ProsjektApiController : ApiController 
    {   
        DbTransaksjonerProsjekt db = new DbTransaksjonerProsjekt();    
        public List<ProsjektVis> Get()
        {
           string userName = User.Identity.Name;
           return db.HentProsjekter(userName);
        }
        public HttpResponseMessage Post(Prosjekt prosjektInn)
        {
            string userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
               bool ok = db.RegistrerProsjekt(prosjektInn, userName);
                if (ok)
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                    };
                }
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Kunne ikke sette inn databasen")
            }; 
        }
        public void Delete(int id)
        {
            string userName = User.Identity.Name;
            db.SlettProsjekt(userName,id);       
        }
        public void Put(Prosjekt EndreProsjekt)
        {
            db.EndreProsjekt(EndreProsjekt);
        }
    }
}
