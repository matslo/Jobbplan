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
      
        public void Get (int id)
        {
         //   db.HentProsjekter(id);
        }
        public void Post(Prosjekt prosjektInn, string b)
        {
            db.RegistrerProsjekt(prosjektInn, b); 
        }
    }
}
