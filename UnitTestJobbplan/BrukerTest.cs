using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Mvc;
using Jobbplan;
using Jobbplan.Controllers;
using Jobbplan.Models;

namespace UnitTestJobbplan
{
    [TestClass]
    public class BrukerTest
    {
        [TestMethod]
        public void RegistrerTest()
        {
            //Arrange
            var controller = new BrukerController();
            //Act
            var result = controller.Registrer() as ViewResult;
            //Assert
            Assert.AreEqual(result.ViewName, "");
        }
        [TestMethod]
        public void RegistrerFeilValideringTest()
        {
            //Arrange
            var controller = new BrukerController();
            var innBruker = new Registrer();
            controller.ViewData.ModelState.AddModelError("Fornavn", "Ikke oppgitt fornavn");            //Act
            var result = (ViewResult)controller.Registrer(innBruker);
            //Assert
            Assert.IsTrue(result.ViewData.ModelState.Count == 1);
            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod]
        public void RegistrerFeilPostTest()
        {
            // Arrange
            var controller = new BrukerController();
            var innBruker = new Registrer();
            innBruker.Fornavn = ""; // gir en feil i innsetingen fra stub.
           // Act
            var actionResult = (ViewResult)controller.Registrer(innBruker);
            // Assert
            Assert.AreEqual(actionResult.ViewName, "");
        }
    }
}
