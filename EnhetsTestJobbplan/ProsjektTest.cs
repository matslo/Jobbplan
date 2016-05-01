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
            var req = new List<Prosjektrequest>() {

                new Prosjektrequest()
            {
               ProsjektId = 1,
               BrukerIdFra = 1,
               BrukerIdTil = 2,
               MeldingId = 1,
               Sendt = Convert.ToDateTime("22.12.2012 16.43")

             }, new Prosjektrequest() { ProsjektId = 2,
               BrukerIdFra = 2,
               BrukerIdTil = 1,
               MeldingId = 1,
               Sendt = Convert.ToDateTime("22.12.2012 16.43")

             }
            };
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
                    new Prosjekt()
                    {
                        ProsjektId=1,EierId=1
                    },
                     new Prosjekt()
                    {
                        ProsjektId=2,EierId=2
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


            mockProductRepository.Setup(mr => mr.SjekkTilgangProsjekt(It.IsAny<string>()))
               .Returns(
                   (string u) =>
                   {
                       int BrukerId = this.mockProductRepository.BrukerId(u);
                       List<ProsjektVis> pros = (from p in deltakelser
                                                 from s in prosjekter
                                                 where p.BrukerId == BrukerId && p.ProsjektId == s.ProsjektId
                                                 select
                                                     new ProsjektVis()
                                                     {
                                                         Id = p.ProsjektId,
                                                     }).ToList();

                       return pros;
                   });

            mockProductRepository.Setup(mr => mr.VisRequesterForProsjekt(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(
                    (int i, string u) =>
                    {
                        var tilganger = this.mockProductRepository.SjekkTilgangProsjekt(u);
                        var eventer = (from k in req
                                       from s in tilganger
                                       where k.ProsjektId == i && k.ProsjektId == s.Id
                                       select new ProsjektrequestMelding
                                       {
                                           MeldingId = k.MeldingId,
                                           TilBruker = this.mockProductRepository.BrukerNavn(k.BrukerIdTil),
                                           Tid = k.Sendt,
                                           ProsjektId = k.ProsjektId
                                           
                                       }).ToList();
                        return eventer;
 
                    });
            mockProductRepository.Setup(mr => mr.VisRequester(It.IsAny<string>()))
              .Returns(
                  (string u) =>
                  {
                      int id = this.mockProductRepository.BrukerId(u);

                      List<ProsjektrequestMelding> pros = (from p in req
                                                           from b in bruker
                                                           from s in prosjekter
                                                           where p.BrukerIdTil == id && p.BrukerIdFra == b.BrukerId && p.ProsjektId == s.ProsjektId
                                                           select
                                                               new ProsjektrequestMelding()
                                                               {
                                                                   ProsjektId = p.ProsjektId,
                                                                   FraBruker = b.Email,
                                                                   Melding = " har invitert deg til å bli medlem av: ",
                                                                   Prosjektnavn = p.Prosjekt.Arbeidsplass,
                                                                   Tid = p.Sendt,
                                                                   TilBruker = u
                                                               }).ToList();
                      return pros;

                  });

            /*
            
            
            
            */

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
            bool testProduct = this.mockProductRepository.ErEier("testing123@hotmail.com", 0);

            Assert.IsFalse(testProduct); 
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
           
        }
        [TestMethod]
        public void Legg_Til_Bruker_Request_Ikke_OK_moq()
        {
          
        }
      
        [TestMethod]
        public void Hent_Bruker_Request()
        {

            List<ProsjektrequestMelding> testProduct = this.mockProductRepository.VisRequester("testing123@hotmail.com");
            for (var i = 0; i < testProduct.Count; i++)
            {
                Assert.AreEqual("testing123@hotmail.com", testProduct[i].TilBruker);

            }
            Assert.AreNotEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<ProsjektrequestMelding>)); // Test type

        }
        
        [TestMethod]
        public void Hent_Bruker_Request_prosjekt()
        {

            List<ProsjektrequestMelding> testProduct = this.mockProductRepository.VisRequesterForProsjekt(1, "testing123@hotmail.com");
            for (var i = 0; i < testProduct.Count; i++)
            {
                Assert.AreEqual(1, testProduct[i].ProsjektId);

            }
            Assert.AreNotEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<ProsjektrequestMelding>)); // Test type
        }
      
    }
}
