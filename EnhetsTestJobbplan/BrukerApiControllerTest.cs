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
    public class BrukerApiControllerTest
    {
        [TestMethod]
        public void Post_Bruker_Ok()
        {
            Registrer nyBruker = new Registrer()
            {
                Fornavn = "Mats",
                Etternavn = "Lokken",
                Email = "tesbruker@hotmail.com",
                Telefonnummer = "93686771",
                BekreftPassord = "password"
            };
            var commandBus = new Mock<IBrukerLogikk>();
            commandBus.Setup(c => c.RegistrerBruker(It.IsAny<Registrer>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "BrukerApi" } });
            var controller = new BrukerApiController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/BrukerApi/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            // Act
            var response = controller.Post(nyBruker);
            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            // var newCategory = JsonConvert.DeserializeObject<CategoryModel>(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(string.Format("http://localhost/api/BrukerApi/{0}", nyBruker.id), response.Headers.Location.ToString());
        }
        [TestMethod]
        public void Post_Bruker_Bad_request()
        {

            var commandBus = new Mock<IBrukerLogikk>();
            commandBus.Setup(c => c.RegistrerBruker(It.IsAny<Registrer>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "BrukerApi" } });
            var controller = new BrukerApiController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/BrukerApi/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            Registrer nyBruker = new Registrer();
            nyBruker.Email = "";
            // The ASP.NET pipeline doesn't run, so validation don't run. 
            controller.ModelState.AddModelError("Email", "mock error message");
            var response = controller.Post(nyBruker);
            // Assert
            commandBus.Verify(e => e.RegistrerBruker(nyBruker), Times.Never);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            // Act

        }
        [TestMethod]
        public void PUT_Bruker_Ok()
        {
            Profil nyBruker = new Profil()
            {
                Fornavn = "Mats",
                Etternavn = "Lokken",
                Telefonnummer = "93686771"
            };
            var commandBus = new Mock<IBrukerLogikk>();
            commandBus.Setup(c => c.EndreBrukerInfo(It.IsAny<Profil>(),It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "Profil" } });
            var controller = new ProfilController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Put, "http://localhost/api/Profil/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            // Act
            var response = controller.Put(nyBruker);
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // var newCategory = JsonConvert.DeserializeObject<CategoryModel>(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(string.Format("http://localhost/api/Profil/{0}", nyBruker.id), response.Headers.Location.ToString());
        }
        [TestMethod]
        public void PUT_Bruker_Bad_request()
        {

            var commandBus = new Mock<IBrukerLogikk>();
            commandBus.Setup(c => c.EndreBrukerInfo(It.IsAny<Profil>(), It.IsAny<string>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "Profil" } });
            var controller = new ProfilController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Put, "http://localhost/api/Profil/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            Profil nyBruker = new Profil();
            nyBruker.Fornavn = "";
            // The ASP.NET pipeline doesn't run, so validation don't run. 
            controller.ModelState.AddModelError("Fornavn", "mock error message");
            var response = controller.Put(nyBruker);
            // Assert
            commandBus.Verify(e => e.EndreBrukerInfo(nyBruker,"brukernavn"), Times.Never);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            // Act

        }
        [TestMethod]
        public void Post_Logginn_Ok()
        {
            LogInn nyBruker = new LogInn()
            { 
                Brukernavn = "tesbruker@hotmail.com",
                Passord = "password"
            };
            var commandBus = new Mock<IBrukerLogikk>();
            commandBus.Setup(c => c.BrukerIdb(It.IsAny<LogInn>())).Returns(true);
            
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "Authenticate" } });
            var controller = new AuthenticateController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Authenticate/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            // Act
            var response = controller.Post(nyBruker);
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
          
           // Assert.AreEqual(string.Format("http://localhost/api/Authenticate/", nyBruker.Brukernavn), response.Headers.Location.ToString());
        }
        [TestMethod]
        public void Post_Logginn_Not_Found()
        {

            var commandBus = new Mock<IBrukerLogikk>();
            commandBus.Setup(c => c.BrukerIdb(It.IsAny<LogInn>())).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "Authenticate" } });
            var controller = new AuthenticateController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Authenticate/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            LogInn nyBruker = new LogInn();
            nyBruker.Brukernavn = "";
            // The ASP.NET pipeline doesn't run, so validation don't run. 
            controller.ModelState.AddModelError("Email", "mock error message");
            var response = controller.Post(nyBruker);
            // Assert
            commandBus.Verify(e => e.BrukerIdb(nyBruker), Times.Never);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            // Act

        }
        [TestMethod]
        public void Post_Logginn_NOT_Found_I_DB()
        {
            LogInn bruker = new LogInn();
            bruker.Brukernavn = "test@test.com";
            bruker.Passord = "passord123";

            var commandBus = new Mock<IBrukerLogikk>();
            commandBus.Setup(c => c.BrukerIdb(bruker)).Returns(true);
            // Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "Authenticate" } });
            var controller = new AuthenticateController(commandBus.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Authenticate/")
                {
                    Properties =
            {
                { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
            }
                }
            };
            LogInn nyBruker = new LogInn();
            nyBruker.Brukernavn = "ikkeIDB@test.com";
            nyBruker.Passord = "yolo1231";
            var response = controller.Post(nyBruker);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            // Act

        }
    }
}
