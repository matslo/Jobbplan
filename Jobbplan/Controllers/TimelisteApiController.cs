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
        public List<Timeliste> Get ()
        {
            string username = User.Identity.Name;
            return db.HentVakter(username);
        }

    }

}
