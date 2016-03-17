using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Models;

namespace Jobbplan.Controllers
{
    public class VaktApi3Controller : ApiController
    {
        DbTransaksjonerVakt db = new DbTransaksjonerVakt();
        //Get api/KalenderApi
        public List<VaktRequestMelding> Get()
        {
            string brukernavn = User.Identity.Name;
            return db.visVaktRequester(brukernavn);
        }
        public void Post(int id)
        {
            string brukernavn = User.Identity.Name;
            db.taLedigVakt(id, brukernavn);
        }
        public bool Put(int id)
        {
            return db.requestOk(id);
        }
        public bool Delete (int id)
        {
            return db.SlettVaktRequest(id);
        }
    }
}
