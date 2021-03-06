﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.Model;
using Jobbplan.BLL;

namespace Jobbplan.Controllers
{
    [Authorize]
    public class TimelisteApiController : ApiController
    {
        private IBrukerLogikk _BrukerBLL;

        public TimelisteApiController()
        {
            _BrukerBLL = new BrukerBLL();
        }
       
        public List<Timeliste> Get (int id)
        {
            string username = User.Identity.Name;
            return _BrukerBLL.HentVakter(username, id);
        }

    }

}
