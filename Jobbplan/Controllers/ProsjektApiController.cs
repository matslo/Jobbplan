using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Jobbplan.Model;
using Jobbplan.BLL;

namespace Jobbplan.Controllers
{
    public class ProsjektApiController : ApiController 
    {
        private IProsjektLogikk _ProsjektBLL;

        public ProsjektApiController()
        {
            _ProsjektBLL = new ProsjektBLL();
        }    
        public List<ProsjektVis> Get()
        {
           string userName = User.Identity.Name;
           return _ProsjektBLL.HentProsjekter(userName);
        }
        public virtual HttpResponseMessage Post(Prosjekt prosjektInn)
        {
            string userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
               bool ok = _ProsjektBLL.RegistrerProsjekt(prosjektInn, userName);
                if (ok)
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                    };
                }
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Kunne ikke sette inn databasen")
            }; 
        }
        public void Delete(int id)
        {
            string userName = User.Identity.Name;
            _ProsjektBLL.SlettProsjekt(userName,id);       
        }
        public HttpResponseMessage Put(Prosjekt EndreProsjekt)
        {
            string userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                bool ok = _ProsjektBLL.EndreProsjekt(EndreProsjekt, userName);
                if (ok)
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                    };
                }
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Kunne ikke sette inn databasen")
            };
        }
    }
}
