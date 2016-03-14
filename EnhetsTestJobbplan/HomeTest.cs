using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using Jobbplan;
using Jobbplan.Controllers;
using System.Web.Mvc;
using Jobbplan.Models;

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

       /* [TestMethod]
        public void IndexLoggInnTest()
        {
            var controller = new AuthenticateController();

            var forventetKunde = new LogInn()
            {
                Brukernavn = "perperper@perper.com",
                Passord = "12345688"
            };
            // Act
            var result = (RedirectToRouteResult)controller.Post(forventetKunde);

            // Assert
            Assert.AreEqual(result.RouteName, "");
        }*/
    }
}
