using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using Jobbplan;
using Jobbplan.Controllers;
using System.Web.Mvc;
using Jobbplan.Models;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;


namespace EnhetsTestJobbplan
{
    [TestClass]
    public class HomeTest
    {
        [TestMethod]
        public void IndexTest()
        {
            //Arrange
            var controller = new HomeController();
            //Act
            var result = controller.Index() as ViewResult;
            //Assert
            Assert.AreEqual(result.ViewName, ""); 
        }
        /*
        [TestMethod]
        public void IndexLoggInnTest()
        {
            // Arrange
            AuthenticateController controller = new AuthenticateController();

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Authenticate")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "Authenticate" } });

            // Act
            var product = new LogInn() { Brukernavn = "matslll@hotmail.com", Passord = "Product1" };
            var response = controller.Post(product);

            // Assert
            Assert.AreEqual("http://localhost/api/Authenticate/", response.Headers.Location.AbsoluteUri);
        }*/
    }
}
