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
            var result = controller.RegistrerAPI() as ViewResult;
            //Assert
            Assert.AreEqual(result.ViewName, "");
        }
        [TestMethod]
        public void RegistrerTestOk()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var controller = new BrukerApiController();
                var innBruker = new Registrer();

                //Act
                var result = controller.Post(innBruker);
                //Assert

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            }
        }
        [TestMethod]
        public void RegistrerTestBrukerFinnes()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTBruker studentRepository = new DbTransaksjonerBruker();
                Registrer NyBruker = new Registrer()
                {
                    Email = "gordo@hotmail.com"        
                };

                bool actual = studentRepository.RegistrerBruker(NyBruker);
                Assert.AreEqual(false, actual);
            }
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
        
        
      
    }
}
