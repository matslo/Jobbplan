using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Mvc;
using Jobbplan;
using Jobbplan.Controllers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using Moq;
using System.Net;
using System.Threading;
using Jobbplan.Model;
using Jobbplan.BLL;
using Jobbplan.DAL;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Net.Http;
using System.Web.Http.Hosting;

namespace EnhetsTestJobbplan
{
    [TestClass]
    public class MalerApiController
    {
       

        [TestMethod]
        public void Post_Mal_Ok()
        {
            MalerSkjema nyMal = new MalerSkjema()
            {
                ProsjektId = 1,
                Tittel = "Dagvakt1",
                startTid = "07.30",
                sluttTid = "14.45"
            };
            var commandBus = new Mock<IVaktLogikk>();
            commandBus.Setup(c => c.RegistrerMal(It.IsAny<MalerSkjema>(), It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "Maler" } });
            var controller = new MalerController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Maler/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            // Act
            var response = controller.Post(nyMal);
            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
             Assert.AreEqual(string.Format("http://localhost/api/Maler"), response.Headers.Location.ToString());
        }

        [TestMethod] public void Post_MAl_Bad_request_Modelstate()
        {

            var commandBus = new Mock<IVaktLogikk>();
            commandBus.Setup(c => c.RegistrerMal(It.IsAny<MalerSkjema>(),It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "BrukerApi" } });
            var controller = new MalerController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Maler/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            MalerSkjema nyMal = new MalerSkjema();
            nyMal.startTid = "";
            // The ASP.NET pipeline doesn't run, so validation don't run. 
            controller.ModelState.AddModelError("startTIid", "mock error message");
            var response = controller.Post(nyMal);
            // Assert
            commandBus.Verify(e => e.RegistrerMal(nyMal,"brukernavn"), Times.Never);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            // Act

        }
        [TestMethod]
        public void Post_MAl_Bad_request_Ikke_Satt_I_DB()
        {
            MalerSkjema nyMal = new MalerSkjema()
            {
                Tittel = "Dagvakt1",
                startTid = "07.30",
                sluttTid = "14.45"
            };
            MalerSkjema nyMal1 = new MalerSkjema()
            {
                Tittel = "Dagvakt1",
                startTid = "07.30",
                sluttTid = "14.45"
            };

            var commandBus = new Mock<IVaktLogikk>();
            commandBus.Setup(c => c.RegistrerMal(nyMal,It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "Maler" } });
            var controller = new MalerController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Maler/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
           
            var response = controller.Post(nyMal1);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }
    }
    
}
