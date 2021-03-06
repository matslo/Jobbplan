﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Jobbplan.Model;
using Jobbplan.BLL;
using System.Web.Script.Serialization;
using System.Text;

namespace Jobbplan.Controllers
{
    [Authorize]
    public class ProsjektApiController : ApiController 
    {
        private IProsjektLogikk _ProsjektBLL;

        public ProsjektApiController()
        {
            _ProsjektBLL = new ProsjektBLL();
        }
        public ProsjektApiController(IProsjektLogikk moq)
        {
            _ProsjektBLL = moq;
        }
        public HttpResponseMessage Get()
        {
           string userName = User.Identity.Name;
            
            var liste = _ProsjektBLL.HentProsjekter(userName);

            var Json = new JavaScriptSerializer();
            string JsonString = Json.Serialize(liste);
            if (liste.Count()>0)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(JsonString, Encoding.UTF8, "application/json"),
                    StatusCode = HttpStatusCode.OK
                };
              
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Kunne ikke hente prosjekter")
            };
        }
        public HttpResponseMessage Post(Prosjekt prosjektInn)
        {
            string userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
               bool ok = _ProsjektBLL.RegistrerProsjekt(prosjektInn, userName);
                if (ok)
                {
                    var response = Request.CreateResponse(HttpStatusCode.Created, prosjektInn);
                    string uri = Url.Link("DefaultApi", new { id = prosjektInn.ProsjektId });
                    response.Headers.Location = new Uri(uri);
                    return response;
                }
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Kunne ikke sette inn i databasen")
                };
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
        public HttpResponseMessage Delete(int id)
        {
            string userName = User.Identity.Name;
            bool ok =_ProsjektBLL.SlettProsjekt(userName,id);
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
                Content = new StringContent("Kunne ikke slette prosjekt")
            };
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
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Kunne ikke sette inn databasen")
                };
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
    }
}
