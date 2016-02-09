using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;
namespace Jobbplan.Controllers
{
    public class VaktApiController : ApiController
    {
        DbTransaksjonerVakt db = new DbTransaksjonerVakt();

        // POST api/KalenderApi
        public void Post(Vakt vaktInn)
        {
            db.RegistrerVakt(vaktInn);

        }
    }
}
