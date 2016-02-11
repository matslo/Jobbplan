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
      /*
        public void Post(Prosjekt prosjektInn)
        {  
           db.RegistrerProsjekt(prosjektInn);
        }*/
    }
}
