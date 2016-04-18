using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jobbplan.BLL;
using Jobbplan.Model;

namespace Jobbplan.Controllers
{
    public class ProsjektreqApiController : ApiController
    {
        private IProsjektLogikk _ProsjektBLL;

        public ProsjektreqApiController()
        {
            _ProsjektBLL = new ProsjektBLL();
        }
       
        public List<ProsjektrequestMelding> Get()
        {
            string UserName = User.Identity.Name;
            return _ProsjektBLL.VisRequester(UserName);
        }
        public List<ProsjektrequestMelding> Get(int id)
        {
            string UserName = User.Identity.Name;
            return _ProsjektBLL.VisRequesterForProsjekt(id,UserName);
        }

        public HttpResponseMessage Post(ProsjektrequestSkjema reqInn)
        {
            string UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                bool ok = _ProsjektBLL.LeggTilBrukerRequest(reqInn, UserName);
                if (ok)
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.Created,
                    };
                }
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Kunne ikke sende melding")
            };
           
        }
         
        public List<ProsjektrequestMelding> Delete(int id)
        {
            string UserName = User.Identity.Name;
            _ProsjektBLL.SlettRequest(id,UserName);
            return _ProsjektBLL.VisRequester(UserName);
        }
    }
}
