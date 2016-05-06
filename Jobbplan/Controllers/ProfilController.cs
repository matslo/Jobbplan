using System;
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
    public class ProfilController : ApiController
    {
        private IBrukerLogikk _BrukerBLL;

        public ProfilController()
        {
            _BrukerBLL = new BrukerBLL();
        }
        public ProfilController(IBrukerLogikk moqs)
        {
            _BrukerBLL = moqs;
        }

        public List<Profil> Get()
        {
            string brukernavn = User.Identity.Name;
            return _BrukerBLL.HentBruker(brukernavn);
        }
        public HttpResponseMessage Put(Profil EndreBrukerInfo)
        {
            string userName = User.Identity.Name;            
            if (ModelState.IsValid)
            {
                bool ok = _BrukerBLL.EndreBrukerInfo(EndreBrukerInfo, userName);
                if (ok)
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                    };
                }
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Kunne ikke sette inn i databasen")

                };
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
    }
}
