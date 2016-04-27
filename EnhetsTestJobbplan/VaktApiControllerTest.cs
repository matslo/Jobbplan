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
    public class VaktApiControllerTest
    {
        [TestMethod]
        public void Post_Vakt_Ok()
        {
            Vaktskjema vakter = new Vaktskjema()
            {
                start = "22.12.2012",
                startTid = "16.43",
                endTid = "18.43",
                title = "Dagvakt",
                Beskrivelse = "Opplæring",
                BrukerId = 1,
                ProsjektId = 1
            };
            var commandBus = new Mock<IVaktLogikk>();
            commandBus.Setup(c => c.RegistrerVakt(It.IsAny<Vaktskjema>(), It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "VaktApi" } });
            var controller = new Jobbplan.Controllers.VaktApiController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/VaktApi/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            // Act
            var response = controller.Post(vakter);
            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            // var newCategory = JsonConvert.DeserializeObject<CategoryModel>(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(string.Format("http://localhost/api/VaktApi/{0}", vakter.Vaktid), response.Headers.Location.ToString());
        }
        [TestMethod]
        public void Post_Vakt_Bad_request()
        {

            var commandBus = new Mock<IVaktLogikk>();
            commandBus.Setup(c => c.RegistrerVakt(It.IsAny<Vaktskjema>(), It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "VaktApi" } });
            var controller = new Jobbplan.Controllers.VaktApiController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/VaktApi/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            Vaktskjema vakter = new Vaktskjema();
            vakter.start = "";
            vakter.end = "";
            // The ASP.NET pipeline doesn't run, so validation don't run. 
            controller.ModelState.AddModelError("start", "mock error message");
            var response = controller.Post(vakter);
            // Assert
            commandBus.Verify(e => e.RegistrerVakt(vakter, "mats"), Times.Never);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            // Act

        }
        [TestMethod]
        public void Post_Vakt_NOT_FOUND()
        {
            Vaktskjema vakter = new Vaktskjema()
            {
                start = "22.12.2012",
                startTid = "16.43",
                endTid = "18.43",
                title = "Dagvakt",
                Beskrivelse = "Opplæring",
                BrukerId = 1,
                ProsjektId = 1
            };
            Vaktskjema vakter1 = new Vaktskjema()
            {
                start = "22.12.2012",
                startTid = "19.43",
                endTid = "18.43",
                title = "Dagvakt",
                Beskrivelse = "Opplæring",
                BrukerId = 1,
                ProsjektId = 1
            };
            var commandBus = new Mock<IVaktLogikk>();
            commandBus.Setup(c => c.RegistrerVakt(vakter, It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "VaktApi" } });
            var controller = new Jobbplan.Controllers.VaktApiController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/VaktApi/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            var response = controller.Post(vakter1);
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            // Act

        }
        [TestMethod]
        public void Post_Ta_Ledig_Vakt_Ok()
        {

            var commandBus = new Mock<IVaktLogikk>();
            commandBus.Setup(c => c.taLedigVakt(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "VaktApi3" } });
            var controller = new VaktApi3Controller(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/VaktApi3/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            // Act
            var response = controller.Post(1);
            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            // var newCategory = JsonConvert.DeserializeObject<CategoryModel>(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(string.Format("http://localhost/api/VaktApi3/{0}", 1), response.Headers.Location.ToString());
        }
        [TestMethod]
        public void Post_Ta_Ledig_Vakt_Bad_Request()
        {
            var commandBus = new Mock<IVaktLogikk>();
            commandBus.Setup(c => c.taLedigVakt(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "VaktApi3" } });
            var controller = new VaktApi3Controller(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/VaktApi3/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            // Act
            int id = 0;
            // The ASP.NET pipeline doesn't run, so validation don't run. 
            controller.ModelState.AddModelError("start", "mock error message");
            var response = controller.Post(id);
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [TestMethod]
        public void Post_Ta_Ledig_Vakt_Not_Found()
        {
            var commandBus = new Mock<IVaktLogikk>();
            commandBus.Setup(c => c.taLedigVakt(0, It.IsAny<string>())).Returns(false);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "VaktApi3" } });
            var controller = new VaktApi3Controller(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/VaktApi3/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            // Act
            int id = 0;
            // The ASP.NET pipeline doesn't run, so validation don't run. 
            var response = controller.Post(id);
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        [TestMethod]
        public void PUT_Endre_Vakt_Ok()
        {
            Vaktskjema vakter = new Vaktskjema()
            {
                start = "22.12.2012",
                startTid = "16.43",
                endTid = "18.43",
                title = "Dagvakt",
                Beskrivelse = "Opplæring",
                BrukerId = 1,
                ProsjektId = 1
            };
            var commandBus = new Mock<IVaktLogikk>();
            commandBus.Setup(c => c.EndreVakt(It.IsAny<Vaktskjema>(), It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "VaktAPi3" } });
            var controller = new VaktApi2Controller(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Put, "http://localhost/api/VaktAPi2/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            // Act
            var response = controller.Put(vakter);
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // var newCategory = JsonConvert.DeserializeObject<CategoryModel>(response.Content.ReadAsStringAsync().Result);
         
        }
        [TestMethod]
        public void PUT_Endre_vakt_Bad_request()
        {

            var commandBus = new Mock<IVaktLogikk>();
            commandBus.Setup(c => c.EndreVakt(It.IsAny<Vaktskjema>(), It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "VaktApi2" } });
            var controller = new VaktApi2Controller(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Put, "http://localhost/api/VaktApi2/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            Vaktskjema nyVakt = new Vaktskjema();
            nyVakt.start = "";
            // The ASP.NET pipeline doesn't run, so validation don't run. 
            controller.ModelState.AddModelError("start", "mock error message");
            var response = controller.Put(nyVakt);
            // Assert
            commandBus.Verify(e => e.EndreVakt(nyVakt, "brukernavn"), Times.Never);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            // Act

        }
        [TestMethod]
        public void PUT_Endre_vakt_NOT_found()
        {
            Vaktskjema vakt = new Vaktskjema();
            vakt.Vaktid = 0;
            var commandBus = new Mock<IVaktLogikk>();
            commandBus.Setup(c => c.EndreVakt(vakt, It.IsAny<string>())).Returns(false);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "VaktApi2" } });
            var controller = new VaktApi2Controller(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Put, "http://localhost/api/VaktApi2/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            var response = controller.Put(vakt);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            
        }
    }
}
