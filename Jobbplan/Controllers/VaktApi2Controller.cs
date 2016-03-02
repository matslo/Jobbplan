using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    public class VaktApi2Controller : ApiController
    {
        DbTransaksjonerVakt db = new DbTransaksjonerVakt();
        //Get api/KalenderApi
      
        public void Put(Vaktskjema endrevakt)
        {
           db.EndreVakt(endrevakt);
        }
    }
}
