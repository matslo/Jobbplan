using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Mvc;
using Jobbplan;
using Jobbplan.Controllers;
using Jobbplan.Models;
using System.Collections.Generic;


namespace EnhetsTestJobbplan
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
        public void RegistrerPostOK()
        {
            // Arrange
            var controller = new BrukerController();

            var forventetBruker = new Registrer()
            {
                id = 1,
                Fornavn = "Per",
                Etternavn = "Olsen",
                Adresse = "Osloveien 82",
                Postnummer = "1234",
                Email = "PerOslo@oslo.com",
                Telefonnummer = "12345678",
                Passord = "12345688"
            };
            // Act
            var result = (RedirectToRouteResult)controller.Registrer(forventetBruker);

            // Assert
            Assert.AreEqual(result.RouteName, "");
        }
        
        [TestMethod]
        public void Registrer_Post_DB_feil()
        {
            // Arrange
            var controller = new BrukerController();
            var forventetKunde = new Registrer();
            forventetKunde.Fornavn = "";

            // Act
            var actionResult = (ViewResult)controller.Registrer(forventetKunde);

            // Assert
            Assert.AreEqual(actionResult.ViewName, "");
        }
    }
}
