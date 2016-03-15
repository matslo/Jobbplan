using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    
    public class TimelisteApiController : ApiController
    {
        DbTransaksjonerBruker db = new DbTransaksjonerBruker();
        public IHttpActionResult Get (int id)
        {
            return db.HentVakter(id);
        }

    }

}
