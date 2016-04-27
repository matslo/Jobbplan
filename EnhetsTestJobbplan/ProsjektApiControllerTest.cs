using System;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Jobbplan;
using Jobbplan.Controllers;
using Jobbplan.Model;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Transactions;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Jobbplan.BLL;
using Jobbplan.DAL;
using System.Web.Http.Hosting;

namespace EnhetsTestJobbplan
{
    [TestClass]
    public class ProsjektApiControllerTest
    {
        [TestMethod]
        public void Post_Registrer_Prosjekt_Ok()
        {
            Prosjekt nyttProsjekt = new Prosjekt()
            {
                Arbeidsplass = "TestShop"   
            };
            var commandBus = new Mock<IProsjektLogikk>();
            commandBus.Setup(c => c.RegistrerProsjekt(It.IsAny<Prosjekt>(), It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "ProsjektApi" } });
            var controller = new ProsjektApiController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/ProsjektApi/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            // Act
            var response = controller.Post(nyttProsjekt);
            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            // var newCategory = JsonConvert.DeserializeObject<CategoryModel>(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(string.Format("http://localhost/api/ProsjektApi/{0}", nyttProsjekt.ProsjektId), response.Headers.Location.ToString());
        }
        [TestMethod]
        public void Post_Legg_til_prosjekt_Bad_Request()
        {
            var commandBus = new Mock<IProsjektLogikk>();
            commandBus.Setup(c => c.RegistrerProsjekt(It.IsAny<Prosjekt>(), It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "ProsjektApi" } });
            var controller = new ProsjektApiController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/ProsjektApi/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            // Act
            var pros = new Prosjekt();
            pros.Arbeidsplass = "";
            // The ASP.NET pipeline doesn't run, so validation don't run. 
            controller.ModelState.AddModelError("Arbeidsplass", "mock error message");
            var response = controller.Post(pros);
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [TestMethod]
        public void Post_Legg_til_prosjekt_NOT_FOUND()
        {
            var pros = new Prosjekt();
            pros.Arbeidsplass = "";
            var commandBus = new Mock<IProsjektLogikk>();
            commandBus.Setup(c => c.RegistrerProsjekt(pros, "test")).Returns(false);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "ProsjektApi" } });
            var controller = new ProsjektApiController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/ProsjektApi/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            // Act
          
            
            var response = controller.Post(pros);
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
