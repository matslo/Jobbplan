using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Mvc;
using Jobbplan;
using Jobbplan.Controllers;
using Jobbplan.Models;
using System.Collections.Generic;
using System.Transactions;
using Moq;
using System.Net;


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
        public void RegistrerFeilValideringTestBruker()
        {
            //Arrange
            var controller = new BrukerApiController();
            var innBruker = new Registrer();
            controller.ModelState.AddModelError("Fornavn","Fornavn må oppgis");
            //Act
            var result = controller.Post(innBruker);
            //Assert

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }
        [TestMethod]
        public void SettInnBrukerOk()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTBruker studentRepository = new DbTransaksjonerBruker();
                Registrer NyBruker = new Registrer()
                {
                    Fornavn="Mats",
                    Etternavn="Lokken",
                    Adresse="Kirkeveien 67",
                    Email="tesbruker@hotmail.com",
                    Postnummer="0364",
                    Poststed="Oslo",
                    Telefonnummer="93686771",
                    BekreftPassord="password"
                };

                bool actual = studentRepository.RegistrerBruker(NyBruker);
                Assert.AreEqual(true, actual);
            }
        }
        /* Skriver inn i Db 
        [TestMethod]
        public void RegistrerPostOK()
        {
            // Arrange
            var controller = new BrukerController(new DbTransaksjonerBrukerStub());

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
            when(controller.Registrer(any()).thenReturn(forventetBruker)

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
        }¨*/
    }
}
