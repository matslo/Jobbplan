using System;
using Jobbplan.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jobbplan.Model;
using Moq;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http.Results;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using Jobbplan.DAL;
using Jobbplan.BLL;

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
        //ProsjektApiController
        [TestMethod]
        public void Opprett_Prosjekt_POST_Ok()
        {
            var _mock = new Mock<ProsjektApiController>();

            var innProsjekt = new Prosjekt()
            {
                ProsjektId = 1,
                Arbeidsplass = "Kiwi",
                EierId = 1
            };

            var http = new HttpResponseMessage();
            http.StatusCode = HttpStatusCode.OK;

            _mock.Setup(x => x.Post(innProsjekt)).Returns(http);
            _mock.Verify(framework => framework.Post(innProsjekt), Times.AtMostOnce());

            ProsjektApiController lovable = _mock.Object;
            var actual = lovable.Post(innProsjekt);

            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
        }
        [TestMethod]
        public void Opprett_Prosjekt_Feil_Validering()
        {
            //Arrange
            var controller = new ProsjektApiController();
            var innBruker = new Prosjekt();
            controller.ModelState.AddModelError("Arbeidsplass", "Arbeidsplass må oppgis");
            //Act
            var result = controller.Post(innBruker);
            //Assert

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }
        //ProsjektDeltakelseApiController
        [TestMethod]
        public void RegistrerDeltakelse_POST_Ok()
        {
            var _mock = new Mock<ProsjektDeltakelseApiController>();

            var innBruker = new ProsjektrequestMelding()
            {
                ProsjektId = 1,
                FraBruker = "mats_loekken@hotmail.com",
                TilBruker = "gordo@hotmail.com",
                MeldingId = 2,
                Prosjektnavn = "Bunnpris",
                Tid = Convert.ToDateTime("22.12.2012 16.43")
            };
            var http = new HttpResponseMessage();
            http.StatusCode = HttpStatusCode.OK;

            _mock.Setup(x => x.Post(innBruker)).Returns(http);
            _mock.Verify(framework => framework.Post(innBruker), Times.AtMostOnce());

            ProsjektDeltakelseApiController lovable = _mock.Object;
            var download = lovable.Post(innBruker);

            Assert.AreEqual(download.StatusCode, HttpStatusCode.OK);
        }
        [TestMethod]
        public void RegistrerDeltakelse_Feil_Validering()
        {
            //Arrange
            var controller = new ProsjektDeltakelseApiController();
            var melding = new ProsjektrequestMelding();
            melding.ProsjektId = 0;
            controller.ModelState.AddModelError("ProsjektId", "");
            //Act
            var result = controller.Post(melding);
            //Assert

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }
        // DbtransaksjonerProsjekt
        [TestMethod]
        public void Registrer_Prosjekt_OK()
        {
            /*
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTProsjekt DBtp = new DbTransaksjonerProsjekt();
                var nyProsjekt = new Prosjekt()
                {
                   ProsjektId = 1000,
                   Arbeidsplass = "testing123"
                 };
                bool actual = DBtp.RegistrerProsjekt(nyProsjekt, "mats_loekken@hotmail.com");
                Assert.AreEqual(true, actual);
            }*/
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
        [TestMethod]
        public void Legg_Til_Bruker_Request_OK()
        {
            var _mock = new Mock<InterfaceDbTProsjekt>();

            var pReq = new ProsjektrequestSkjema();
            var bruker = new Registrer();
            bruker.Email = "mats_loekken@hotmail.com";
            pReq.TilBruker = "gordo@hotmail.com";
            pReq.ProsjektId = 1;

            _mock.Setup(x => x.LeggTilBrukerRequest(pReq, bruker.Email)).Returns(true);
            _mock.Verify(framework => framework.LeggTilBrukerRequest(pReq, "mats_loekken@hotmail.com"), Times.AtMostOnce());

            InterfaceDbTProsjekt lovable = _mock.Object;
            var actual = lovable.LeggTilBrukerRequest(pReq, "mats_loekken@hotmail.com");

            Assert.AreEqual(true, actual);
        }
        [TestMethod]
        public void Legg_Til_Bruker_Request_Ikke_OK_moq()
        {
            var _mock = new Mock<InterfaceDbTProsjekt>();

            var pReq = new ProsjektrequestSkjema();
            pReq.TilBruker = null;
            var bruker = new Registrer();

            bruker.Email = "mats_loekken@hotmail.com";

            _mock.Setup(x => x.LeggTilBrukerRequest(pReq, bruker.Email)).Returns(false);
            _mock.Verify(framework => framework.LeggTilBrukerRequest(pReq, "mats_loekken@hotmail.com"), Times.AtMostOnce());

            InterfaceDbTProsjekt lovable = _mock.Object;
            var actual = lovable.LeggTilBrukerRequest(pReq, "mats_loekken@hotmail.com");

            Assert.AreEqual(false, actual);
        }
        [TestMethod]
        public void Legg_Til_Bruker_Request_Ikke_OK()
        {/*
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTProsjekt Dbt = new DbTransaksjonerProsjekt();
                var pReq = new ProsjektrequestSkjema();
                pReq.TilBruker = null;
                var bruker = new Registrer();
                bool actual = Dbt.LeggTilBrukerRequest(pReq, "mats_loekken@hotmail.com");
                Assert.AreEqual(false, actual);
            }*/
        }
        [TestMethod]
        public void Hent_Bruker_Request()
        {

            var _mock = new Mock<InterfaceDbTProsjekt>();

            var req = new List<ProsjektrequestMelding>() { new ProsjektrequestMelding()
            {
               ProsjektId = 1,
               FraBruker = "mats_loekk@hotmail.com",
               TilBruker = "gordo@hotmail.com",
               Melding = "",
               Prosjektnavn = "Bunnpris",
               MeldingId = 1,
               Tid = Convert.ToDateTime("22.12.2012 16.43")

             } };

            _mock.Setup(x => x.VisRequester("gordo@hotmail.com")).Returns(req);
            _mock.Verify(framework => framework.VisRequester("gordo@hotmail.com"), Times.AtMostOnce());

            InterfaceDbTProsjekt lovable = _mock.Object;
            var actual = lovable.VisRequester("gordo@hotmail.com");

            Assert.AreEqual(req, actual);

        }
        [TestMethod]
        public void Hent_Bruker_Request_prosjekt()
        {

            var _mock = new Mock<InterfaceDbTProsjekt>();

            var req = new List<ProsjektrequestMelding>() { new ProsjektrequestMelding()
            {
               ProsjektId = 1,
               FraBruker = "mats_loekk@hotmail.com",
               TilBruker = "gordo@hotmail.com",
               Melding = "",
               Prosjektnavn = "Bunnpris",
               MeldingId = 1,
               Tid = Convert.ToDateTime("22.12.2012 16.43")

             } };

            _mock.Setup(x => x.VisRequesterForProsjekt(1,"gordo@hotmail.com")).Returns(req);
            _mock.Verify(framework => framework.VisRequesterForProsjekt(1,"gordo@hotmail.com"), Times.AtMostOnce());

            InterfaceDbTProsjekt lovable = _mock.Object;
            var actual = lovable.VisRequesterForProsjekt(1,"gordo@hotmail.com");

            Assert.AreEqual(req, actual);

        }
       
    }
}
