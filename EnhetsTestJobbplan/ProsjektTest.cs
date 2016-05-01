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
                    },
                    new dbBruker
                    {
                        BrukerId = 3, Email = "ikkeadmin@hotmail.com"
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
                    ProsjektDeltakerId=1,
                    BrukerId = 1,
                    ProsjektId = 1,
                    Admin = false,
                    Medlemsdato = Convert.ToDateTime("22.12.2012 16.43")
                },
                 new Prosjektdeltakelse
                {
                    ProsjektDeltakerId=2,
                    BrukerId = 2,
                    ProsjektId = 1,
                    Admin = true,
                    Medlemsdato = Convert.ToDateTime("22.12.2012 16.43")
                },
                 new Prosjektdeltakelse
                {
                    ProsjektDeltakerId=3,
                    BrukerId = 3,
                    ProsjektId = 1,
                    Admin = false,
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
                  var TestOk = (from x in prosjekter
                                 where x.EierId == bId && x.ProsjektId == i
                                 select x.EierId).SingleOrDefault();
                                
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

            mockProductRepository.Setup(mr => mr.HentProsjekter(It.IsAny<string>()))
             .Returns(
                 (string u) =>
                 {
                     int id = this.mockProductRepository.BrukerId(u);
                     List<ProsjektVis> pros = (from p in deltakelser
                                               from s in prosjekter
                                               where p.BrukerId == id && p.ProsjektId == s.ProsjektId
                                               select
                                                   new ProsjektVis()
                                                   {
                                                       Id = p.ProsjektId,
                                                       Arbeidsplass = s.Arbeidsplass
                                                   }).ToList();
                     return pros;

                 });

            mockProductRepository.Setup(mr => mr.SlettBrukerFraProsjekt(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(
                (string u, int i) =>
                {
                    
                    var SlettProsjekt = deltakelser.FirstOrDefault(p => p.ProsjektDeltakerId == i);
                    var sjekkEier = prosjekter.FirstOrDefault(p => p.EierId == SlettProsjekt.BrukerId && p.ProsjektId == SlettProsjekt.ProsjektId);
                    if (sjekkEier != null)
                    {
                        return false; // kan ikke slette eier fra prosjekt
                    }
                    if (this.mockProductRepository.ErAdmin(u, SlettProsjekt.ProsjektId) == true || this.mockProductRepository.ErEier(u, SlettProsjekt.ProsjektId) == true)
                    {
                        try
                        {
                            deltakelser.Remove(SlettProsjekt);
                            return true;
                        }
                        catch (Exception feil)
                        {
                            return false;
                        }
                    }
                    return false;

                });

            mockProductRepository.Setup(mr => mr.RegistrerProsjekt(It.IsAny<Prosjekt>(), It.IsAny<string>()))
            .Returns(
                (Prosjekt p, string u) =>
                {
                    int userId = this.mockProductRepository.BrukerId(u);
                    if (p.Arbeidsplass == "" || userId == 0)
                    {
                        return false;
                    }
                    var nyProsjekt = new Prosjekt()
                    {
                        Arbeidsplass = p.Arbeidsplass,
                        EierId = userId

                    };
                    var nyProsjektDeltakelse = new Prosjektdeltakelse()
                    {
                        BrukerId = userId,
                        Medlemsdato = DateTime.Now,
                        ProsjektId = nyProsjekt.ProsjektId
                    };

                        try
                        {
                            prosjekter.Add(nyProsjekt);
                            deltakelser.Add(nyProsjektDeltakelse);
                            
                            return true;
                        }
                        catch (Exception feil)
                        {
                            return false;
                        }
                    
                });

            mockProductRepository.Setup(mr => mr.LeggTilBrukerRequest(It.IsAny<ProsjektrequestSkjema>(), It.IsAny<string>()))
          .Returns(
              (ProsjektrequestSkjema p, string u) =>
              {
                  int bId = this.mockProductRepository.BrukerId(u);
                  int bIdTil = this.mockProductRepository.BrukerId(p.TilBruker);
                  int pId = p.ProsjektId;

                  if (!this.mockProductRepository.ErAdmin(u, p.ProsjektId) && !this.mockProductRepository.ErEier(u, p.ProsjektId))
                  {
                      return false;
                  }

                  if (bIdTil == 0)
                  {
                      return false;
                  }
                  var nyRequest = new Prosjektrequest()
                  {
                      BrukerIdFra = bId,
                      BrukerIdTil = bIdTil,
                      ProsjektId = pId,
                      Akseptert = false,
                      Sendt = DateTime.Now
                  };
                  try
                      {
                          req.Add(nyRequest);
                          return true;
                      }
                      catch (Exception feil)
                      {
                          return false;
                      }
            
              });


            mockProductRepository.Setup(mr => mr.RegistrerProsjektdeltakelse(It.IsAny<ProsjektrequestMelding>(), It.IsAny<string>()))
          .Returns(
              (ProsjektrequestMelding p, string u) =>
              {
                 
                  int id = this.mockProductRepository.BrukerId(u);
                  IEnumerable<ProsjektDeltakelseOverforing> prosjektReq = from prosj in req
                                                                          from b in bruker
                                                                          from s in prosjekter
                                                                          where prosj.BrukerIdTil == id && prosj.BrukerIdFra == b.BrukerId && prosj.ProsjektId == p.ProsjektId
                                                                          select new ProsjektDeltakelseOverforing()
                                                                          {
                                                                              BrukerId = prosj.BrukerIdTil,
                                                                              ProsjektId = prosj.ProsjektId

                                                                          };
                  var prosjektD = new Prosjektdeltakelse();
                  foreach (var a in prosjektReq)
                  {
                      prosjektD.BrukerId = a.BrukerId;
                      prosjektD.ProsjektId = a.ProsjektId;
                      prosjektD.Medlemsdato = DateTime.Now;
                  }
                  try
                  {

                      deltakelser.Add(prosjektD);
                      this.mockProductRepository.SlettRequest(prosjektD.ProsjektId, u);
                      return true;

                  }
                  catch (Exception feil)
                  {
                      return false;
                  }

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
            bool testProduct = this.mockProductRepository.ErAdmin("testing123@hotmail.com", 2);

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
        public void RegistrerProsjektdeltakelse()
        {
            ProsjektrequestMelding p = new ProsjektrequestMelding();
            p.ProsjektId = 1;
            p.TilBruker = "testing1@hotmail.com";
            
            bool testProduct = this.mockProductRepository.RegistrerProsjektdeltakelse(p, "testing123@hotmail.com");

            Assert.IsTrue(testProduct);
        }
        [TestMethod]
        public void Legg_Til_Bruker_Request_OK()
        {
            ProsjektrequestSkjema p = new ProsjektrequestSkjema();
            p.ProsjektId = 1;
            p.TilBruker = "ikkeadmin@hotmail.com";
            bool testProduct = this.mockProductRepository.LeggTilBrukerRequest(p, "testing1@hotmail.com");

            Assert.IsTrue(testProduct);
        }
        [TestMethod]
        public void Legg_Til_Bruker_Request_False_Bruker_finnesIkke()
        {
            ProsjektrequestSkjema p = new ProsjektrequestSkjema();
            p.ProsjektId = 1;
            p.TilBruker = "nouser@hotmail.com";
            bool testProduct = this.mockProductRepository.LeggTilBrukerRequest(p, "testing1@hotmail.com");

            Assert.IsFalse(testProduct);
        }

        [TestMethod]
        public void Legg_Til_Bruker_Request_False_Ikke_EIER()
        {
            ProsjektrequestSkjema p = new ProsjektrequestSkjema();
            p.ProsjektId = 1;
            p.TilBruker = "testing1@hotmail.com";
            bool testProduct = this.mockProductRepository.LeggTilBrukerRequest(p, "ikkeadmin@hotmail.com");

            Assert.IsFalse(testProduct);
        }
        [TestMethod]
        public void Legg_Til_Bruker_Request_OK_Admin()
        {
            ProsjektrequestSkjema p = new ProsjektrequestSkjema();
            p.ProsjektId = 1;
            p.TilBruker = "ikkeadmin@hotmail.com";
            bool testProduct = this.mockProductRepository.LeggTilBrukerRequest(p, "testing1@hotmail.com");

            Assert.IsTrue(testProduct);
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
        [TestMethod]
        public void Hent_prosjekt()
        {
            List<ProsjektVis> testProduct = this.mockProductRepository.HentProsjekter("testing123@hotmail.com");
            Assert.AreNotEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<ProsjektVis>)); // Test type
        }

        [TestMethod]
        public void Slett_Bruker_fra_prosjekt_ok()
        {
             bool testProduct = this.mockProductRepository.SlettBrukerFraProsjekt("testing123@hotmail.com",2);

            Assert.IsTrue(testProduct);
        }
        [TestMethod]
        public void Slett_Bruker_fra_prosjekt_False()
        {
            bool testProduct = this.mockProductRepository.SlettBrukerFraProsjekt("testing123@hotmail.com", 1);
            //Kan ikke slette eier av bedrift
            Assert.IsFalse(testProduct);
        }
        [TestMethod]
        public void Slett_Bruker_fra_prosjekt_False_Admin()
        {
            bool testProduct = this.mockProductRepository.SlettBrukerFraProsjekt("ikkeadmin@hotmail.com", 1);

            Assert.IsFalse(testProduct);
        }
        [TestMethod]
        public void Slett_Bruker_fra_prosjekt_OK_Admin()
        {
            bool testProduct = this.mockProductRepository.SlettBrukerFraProsjekt("testing1@hotmail.com", 3);

            Assert.IsTrue(testProduct);
        }
        [TestMethod]
        public void RegistrerProsjekt()
        {
            Prosjekt p = new Prosjekt();
            p.Arbeidsplass = "Test";
            bool testProduct = this.mockProductRepository.RegistrerProsjekt(p,"testing1@hotmail.com");

            Assert.IsTrue(testProduct);
        }
        [TestMethod]
        public void RegistrerProsjekt_Tom_False()
        {
            Prosjekt p = new Prosjekt();
            p.Arbeidsplass = "";
            bool testProduct = this.mockProductRepository.RegistrerProsjekt(p, "testing1@hotmail.com");

            Assert.IsFalse(testProduct);
        }
        [TestMethod]
        public void RegistrerProsjekt_UgyldigBruker_False()
        {
            Prosjekt p = new Prosjekt();
            bool testProduct = this.mockProductRepository.RegistrerProsjekt(p, "hacker@hacker.com");

            Assert.IsFalse(testProduct);
        }
        [TestMethod]
        public void BrukerNavn()
        {
            string testProduct = this.mockProductRepository.BrukerNavn(1);
            Assert.AreEqual("testing123@hotmail.com",testProduct);
        }
        [TestMethod]
        public void BrukerId()
        {
            int testProduct = this.mockProductRepository.BrukerId("testing1@hotmail.com");
            Assert.AreEqual(2,testProduct);
        }
        [TestMethod]
        public void SjekkTIlgang_Prosjekt()
        {
            var testProduct = this.mockProductRepository.SjekkTilgangProsjekt("testing1@hotmail.com");
          
            Assert.AreNotEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<ProsjektVis>)); // Test type
        }

    }
}
