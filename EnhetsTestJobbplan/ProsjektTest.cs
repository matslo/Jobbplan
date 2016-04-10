﻿using System;
using Jobbplan.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jobbplan.Models;
using Moq;
using System.Web.Mvc;
using System.Net;
using System.Transactions;

namespace EnhetsTestJobbplan
{
    [TestClass]
    public class ProsjektTest
    {
        //Inneholder Tester for ProsjektApiController,ProsjektDeltakelseApiController, ProsjektReqApiController, ProsjektController, DbtransaksjonerProsjekt
        
        //ProsjektController
        [TestMethod]
        public void Index_ViewName()
        {
            //Arrange
            var controller = new ProsjektController();
            //Act
            var result = controller.Index() as ViewResult;
            //Assert
            Assert.AreEqual(result.ViewName, "");
        }
        [TestMethod]
        public void LeggTilBruker_ViewName()
        {
            //Arrange
            var controller = new ProsjektController();
            //Act
            var result = controller.LeggTilBruker() as ViewResult;
            //Assert
            Assert.AreEqual(result.ViewName, "");
        }
        [TestMethod]
        public void Meldingsboks_ViewName()
        {
            //Arrange
            var controller = new ProsjektController();
            //Act
            var result = controller.Meldingsboks() as ViewResult;
            //Assert
            Assert.AreEqual(result.ViewName, "");
        }

        //ProsjektDeltakelseApiController
        [TestMethod]
        public void RegistrerDeltakelse_POST_Ok()
        {

        }
        [TestMethod]
        public void RegistrerDeltakelse_Feil_Validering()
        {
            //Arrange
            var controller = new ProsjektDeltakelseApiController();
            var melding = new ProsjektrequestMelding();
            melding.ProsjektId = 0;
            //Act
            var result = controller.Post(melding);
            //Assert

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }



        // DbtransaksjonerProsjekt
        [TestMethod]
        public void Registrer_Prosjekt_OK()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTProsjekt DBtp = new DbTransaksjonerProsjekt();
                var nyProsjekt = new Prosjekt()
                {
                   Arbeidsplass = "kiwi"
                 };
                bool actual = DBtp.RegistrerProsjekt( nyProsjekt, "mats_loekken@hotmail.com");
                Assert.AreEqual(true, actual);
            }
        }
        [TestMethod]
        public void Registrer_Prosjekt_Mangler_Arbeidplass()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTProsjekt DBtp = new DbTransaksjonerProsjekt();
                var nyProsjekt = new Prosjekt();

                bool actual = DBtp.RegistrerProsjekt(nyProsjekt, "mats_loekken@hotmail.com");
                Assert.AreEqual(false, actual);
            }
        }




    }
}
