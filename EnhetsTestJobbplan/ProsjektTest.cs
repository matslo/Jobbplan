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
    public class ProsjektTest
    {
        private InterfaceDbTProsjekt mockProductRepository;
        public ProsjektTest(){
         // Lager mocks som vi kan gjøre spørringer mot
           
            List<Vaktkalender> vakter = new List<Vaktkalender>
            {
                // new Vaktkalender {ProsjektId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"},
                // new Vaktkalender {ProsjektId = 2, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring" },
                //new Vaktkalender {ProsjektId = 3, Brukernavn = "", start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"}
            };
            var req = new List<Prosjektrequest>() { new Prosjektrequest()
            {
               ProsjektId = 1,
               BrukerIdFra = 1,
               BrukerIdTil = 2,
               MeldingId = 1,
               Sendt = Convert.ToDateTime("22.12.2012 16.43")

             } };
            List<dbBruker> bruker = new List<dbBruker>
                {
                    new dbBruker
                    {
                        BrukerId = 1, Email = "testing123@hotmail.com"
                    },
                    new dbBruker
                    {
                        BrukerId = 2, Email = "testing1@hotmail.com"
                    }
                };
        List<Prosjekt> prosjekter = new List<Prosjekt>
                {
                    new Prosjekt
                    {
                        ProsjektId=1,EierId=1
                    }

                };
            List<Prosjektdeltakelse> deltakelser = new List<Prosjektdeltakelse>
            {
                new Prosjektdeltakelse
                {
                    BrukerId = 1,
                    ProsjektId = 1,
                    Admin = false,
                    Medlemsdato = Convert.ToDateTime("22.12.2012 16.43")
                },
                 new Prosjektdeltakelse
                {
                    BrukerId = 2,
                    ProsjektId = 1,
                    Admin = true,
                    Medlemsdato = Convert.ToDateTime("22.12.2012 16.43")
                }
            };
            var reqMelding = new List<ProsjektrequestMelding>() { new ProsjektrequestMelding()
            {
               ProsjektId = 1,
               FraBruker = "mats_loekk@hotmail.com",
               TilBruker = "gordo@hotmail.com",
               Melding = "",
               Prosjektnavn = "Bunnpris",
               MeldingId = 1,
               Tid = Convert.ToDateTime("22.12.2012 16.43")

             } };

            Mock<InterfaceDbTProsjekt> mockProductRepository = new Mock<InterfaceDbTProsjekt>();


          
            bool ok = false;
            mockProductRepository.Setup(mr => mr.ErEier(It.IsAny<string>(), It.IsAny<int>()))
              .Returns(
                (string u, int i) =>
              {
                  var bId = this.mockProductRepository.BrukerId(u);
                  var TestOk = (from x in deltakelser
                             where x.ProsjektId == i && x.BrukerId == bId
                             select x.ProsjektId).SingleOrDefault();
                                
                  if (TestOk != 0)
                  {
                      ok = true;
                      return ok;
                  }
                  return ok;
              });

            mockProductRepository.Setup(mr => mr.ErAdmin(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(
              (string u, int i) =>
              {
                  var okAdmin = false;
                  var bId = this.mockProductRepository.BrukerId(u);
                  var TestOk = (from x in deltakelser
                                where x.ProsjektId == i && x.BrukerId == bId
                                select x.Admin).SingleOrDefault();

                  if (TestOk != false)
                  {
                      okAdmin = true;
                      return okAdmin;
                  }
                  return okAdmin;
              });

            
            mockProductRepository.Setup(mr => mr.BrukerId(It.IsAny<string>()))
              .Returns(
                (string u) =>
                {
                    int userId = (from x in bruker
                                  where x.Email == u
                                  select x.BrukerId).SingleOrDefault();
                    return userId;
                });

            mockProductRepository.Setup(mr => mr.BrukerNavn(It.IsAny<int>()))
            .Returns(
              (int i) =>
              {
                
                  var brukernavn = (from x in bruker
                             where x.BrukerId == i
                             select x.Email).SingleOrDefault();
                  return brukernavn;
              });




            mockProductRepository.Setup(mr => mr.VisRequesterForProsjekt(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(
                    (int i, string u) =>
                    {
                        string testProduct = this.mockProductRepository.BrukerNavn(1);
                        return null;
                    });
                /*Dbkontekst db = new Dbkontekst();

            var tilganger = SjekkTilgangProsjekt(brukernavn);
           
            List<Prosjektrequest> proReq = db.Prosjektrequester.ToList();
            var eventer = (from k in proReq
                           from s in tilganger
                           where k.ProsjektId == id && k.ProsjektId == s.Id
                           select new ProsjektrequestMelding
                           {
                               MeldingId = k.MeldingId,
                               TilBruker = BrukerNavn(k.BrukerIdTil),
                               Tid = k.Sendt
                           }).ToList();
            return eventer;*/
                // Complete the setup of our Mock Product Repository
            this.mockProductRepository = mockProductRepository.Object;
        }
        //Inneholder Tester for ProsjektApiController,ProsjektDeltakelseApiController, ProsjektReqApiController, ProsjektController, DbtransaksjonerProsjekt

        [TestMethod]
        public void Er_eier_ok()
        {
            // Try finding a product by id
            bool testProduct = this.mockProductRepository.ErEier("testing123@hotmail.com",1);
         
            Assert.IsTrue(testProduct); // Test if null
           
        }
        [TestMethod]
        public void Er_Ikke_eier()
        {
            // Try finding a product by id
            bool testProduct = this.mockProductRepository.ErEier("testing123@hotmail.com", 0);

            Assert.IsFalse(testProduct); // Test if null

        }
        [TestMethod]
        public void Er_admin_ok()
        {
            // Try finding a product by id
            bool testProduct = this.mockProductRepository.ErAdmin("testing1@hotmail.com", 1);

            Assert.IsTrue(testProduct); // Test if null

        }
        [TestMethod]
        public void Er_Ïkke_Admin()
        {
            // Try finding a product by id
            bool testProduct = this.mockProductRepository.ErAdmin("testing123@hotmail.com", 0);

            Assert.IsFalse(testProduct); // Test if null

        }
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
        /*
        [TestMethod]
        public void Hent_Bruker_Request_prosjekt()
        {

            var _mock = new Mock<InterfaceDbTProsjekt>();

           

           
            _mock.Verify(framework => framework.VisRequesterForProsjekt(1,"gordo@hotmail.com"), Times.AtMostOnce());

            InterfaceDbTProsjekt lovable = _mock.Object;
            var actual = lovable.VisRequesterForProsjekt(1,"gordo@hotmail.com");

            Assert.AreEqual(req, actual);

        }
       */
    }
}
