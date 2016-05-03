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
    public class BrukerTest
    {
        private InterfaceDbTBruker mockProductRepository;
        private InterfaceDbTProsjekt mockProductRepositoryProsjekt;
        public BrukerTest()
        {
            // Lager mock som vi kan gjøre spørringer mot

            List<BrukerListe> brukere = new List<BrukerListe>
                {
                   new BrukerListe() {Brukernavn = "testing123@hotmail.com", ProsjektDeltakerId = 1, Admin = false, Navn="Mats"},
                    new BrukerListe() {Brukernavn = "testing1@hotmail.com", ProsjektDeltakerId = 2, Admin=true, Navn = "Gordo"}
                };
            List<dbBruker> brukereDB = new List<dbBruker>
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
            List<Vakt> vakterDB = new List<Vakt>
                {
                    new Vakt {ProsjektId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"},
                    new Vakt {ProsjektId = 2, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring" },
                    new Vakt {ProsjektId = 1, BrukerId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"}
                };
            List<Prosjektdeltakelse> deltakelser = new List<Prosjektdeltakelse>
                {
                    new Prosjektdeltakelse() {BrukerId = 1, ProsjektId = 1, ProsjektDeltakerId = 1, Admin = false},
                    new Prosjektdeltakelse() {BrukerId = 2, ProsjektId = 1, ProsjektDeltakerId = 2, Admin = false}
                };
            List<Prosjekt> prosjekter  = new List<Prosjekt>
                {
                    new Prosjekt() {ProsjektId = 1, EierId = 1},
                    new Prosjekt() {ProsjektId = 2, EierId = 2}
                };

            // Mock the Products Repository using Moq
            Mock<InterfaceDbTBruker> mockProductRepository = new Mock<InterfaceDbTBruker>();
            Mock<InterfaceDbTProsjekt> mockProductRepositoryProsjekt = new Mock<InterfaceDbTProsjekt>();



            mockProductRepository.Setup(mr => mr.HentBrukere(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(
                (int i, string u) =>
                {
                    var deltakels = deltakelser.Where(x => x.ProsjektId == i).ToList();
                    List<BrukerListe> testliste = new List<BrukerListe>();
                    foreach (var del in deltakels)
                    {
                        testliste.Add(new BrukerListe() { ProsjektId = del.ProsjektId, ProsjektDeltakerId = del.ProsjektDeltakerId, Admin = del.Admin });
                    }
                    return testliste;
                });

            mockProductRepository.Setup(mr => mr.GiBrukerAdminTilgang(It.IsAny<Sjef>(), It.IsAny<string>()))
              .Returns(
              (Sjef s, string u) =>
              {
                  if (!this.mockProductRepositoryProsjekt.ErEier(u, s.ProsjektId))
                  {
                      return false;
                  }
                  var AdminTilgang = deltakelser.FirstOrDefault(p => p.ProsjektId == s.ProsjektId && p.BrukerId == s.BrukerId);
                  if (AdminTilgang == null)
                  {
                      return false;
                  }
                  try
                  {
                      AdminTilgang.Admin = true;
                      return true;
                  }
                  catch (Exception feil)
                  {
                      return false;
                  }
              });


           


            mockProductRepository.Setup(mr => mr.RegistrerBruker(It.IsAny<Registrer>()))
           .Returns(
           (Registrer r) =>
           {
               if (this.mockProductRepository.EmailDb(r))
               {
                   return false;
               }
                var algoritme = System.Security.Cryptography.SHA256.Create();
                var innData = System.Text.Encoding.ASCII.GetBytes(r.BekreftPassord);
                var passordDb = algoritme.ComputeHash(innData);
              
               var nyBruker = new dbBruker()
               {
                   Passord = passordDb,
                   Fornavn = r.Fornavn,
                   Etternavn = r.Etternavn,
                   Email = r.Email,
                   Telefonnummer = r.Telefonnummer
               };

               using (var db = new Dbkontekst())
               {
                   try
                   {
                       db.Brukere.Add(nyBruker);
                       db.SaveChanges();
                       return true;
                   }
                   catch (Exception feil)
                   {
                       return false;
                   }
               }
           });

            mockProductRepository.Setup(mr => mr.EmailDb(It.IsAny<Registrer>()))
          .Returns(
          (Registrer r) =>
          {
              dbBruker funnetBruker = brukereDB.FirstOrDefault
                     (b => b.Email == r.Email);
              if (funnetBruker == null)
              {
                  return false;
              }
              else
              {
                  return true;
              }
          });



            mockProductRepository.Setup(mr => mr.HentVakter(It.IsAny<string>(), It.IsAny<int>()))
           .Returns(
           (string u, int i)=>
           {
              
               int id = this.mockProductRepositoryProsjekt.BrukerId(u);

               List<Timeliste> pros = (from p in vakterDB
                                       where p.BrukerId == id && p.ProsjektId == i
                                       select
                                           new Timeliste()
                                           {
                                               ProsjektId = p.ProsjektId,
                                               PeriodeStart = p.start,
                                               PeriodeSlutt = p.end
                                           }).ToList();

               return pros;
           });
            mockProductRepository.Setup(mr => mr.FjernAdminTilgang(It.IsAny<Sjef>(), It.IsAny<string>()))
              .Returns(
              (Sjef s, string u) =>
              {
                
                  if (!this.mockProductRepositoryProsjekt.ErEier(u, s.ProsjektId))
                  {
                      return false;
                  }
                  try
                  {
                      var AdminTilgang = deltakelser.FirstOrDefault(p => p.ProsjektId == s.ProsjektId && p.BrukerId == s.BrukerId);
                      AdminTilgang.Admin = false;
                 
                      return true;
                  }
                  catch (Exception feil)
                  {
                      return false;
                  }
              });


            mockProductRepositoryProsjekt.Setup(mr => mr.ErEier(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(
              (string u, int i) =>
              {
                  var bId = this.mockProductRepositoryProsjekt.BrukerId(u);
                  var TestOk = (from x in prosjekter
                                where x.EierId == bId && x.ProsjektId == i
                                select x.EierId).SingleOrDefault();
                  bool ok = false;
                  if (TestOk != 0)
                  {
                      ok = true;
                      return ok;
                  }
                  return ok;
              });

            mockProductRepositoryProsjekt.Setup(mr => mr.ErAdmin(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(
              (string u, int i) =>
              {
                  var okAdmin = false;
                  var bId = this.mockProductRepositoryProsjekt.BrukerId(u);
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

            mockProductRepositoryProsjekt.Setup(mr => mr.BrukerId(It.IsAny<string>()))
              .Returns(
                (string u) =>
                {
                    int userId = (from x in brukereDB
                                  where x.Email == u
                                  select x.BrukerId).SingleOrDefault();
                    return userId;
                });

            // Allows us to test saving a product

            // Complete the setup of our Mock Product Repository
            this.mockProductRepository = mockProductRepository.Object;
                  this.mockProductRepositoryProsjekt = mockProductRepositoryProsjekt.Object;
        }

        //Inneholder Tester for BrukerApiController, BrukerController, DbtransaksjonerBruker
        //REGISTRER Bruker
        //BrukerController
        [TestMethod]
        public void Hent_brukere_ok()
        {
            // Try finding a product by id
            List<BrukerListe> testProduct = this.mockProductRepository.HentBrukere(1, "testing123@hotmail.com");
            for (var i = 0; i < testProduct.Count; i++)
            {
                Assert.AreEqual(1, testProduct[i].ProsjektId);

            }
            Assert.AreNotEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<BrukerListe>)); // Test type
        }
        [TestMethod]
        public void Hent_brukere_Ikke_Vis_Ikke_Admin()
        {
            // Try finding a product by id
            List<BrukerListe> testProduct = this.mockProductRepository.HentBrukere(1, "ikkeadmin@hotmail.com");
            for (var i = 0; i < testProduct.Count; i++)
            {
                Assert.AreEqual(1, testProduct[i].ProsjektId);

            }
            Assert.AreNotEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<BrukerListe>)); // Test type
        }
        [TestMethod]
        public void Registrer_ViewName()
        {
            //Arrange
            var controller = new BrukerController();
            //Act
            var result = controller.RegistrerAPI() as ViewResult;
            //Assert
            Assert.AreEqual(result.ViewName, "");
        }
        [TestMethod]
        public void MinProfil_ViewName()
        {
            //Arrange
            var controller = new BrukerController();
            //Act
            var result = controller.MinProfil() as ViewResult;
            //Assert
            Assert.AreEqual(result.ViewName, "");
        }   
        //DBTransaksjonerBruker
      
        [TestMethod]
        public void Sett_Inn_Bruker_I_Db_Ok() //Integrasjonstest
        {
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTBruker studentRepository = new DbTransaksjonerBruker();
                Registrer NyBruker = new Registrer()
                {
                    Fornavn="Mats",
                    Etternavn="Lokken",
                    Email="tesbruker111@hotmail.com",
                    Telefonnummer="93686771",
                    BekreftPassord="password"
                };

                bool actual = studentRepository.RegistrerBruker(NyBruker);
                Assert.AreEqual(true, actual);
            }
        }
        [TestMethod]
        public void Registrerbruker_OK()
        {
            Registrer NyBruker = new Registrer()
            {
                Fornavn = "Mats",
                Etternavn = "Lokken",
                Email = "nybruker@hotmail.com",
                Telefonnummer = "93686771",
                BekreftPassord = "password"
            };

            bool ok = this.mockProductRepository.RegistrerBruker(NyBruker);
            Assert.IsTrue(ok);

        }
        [TestMethod]
        public void Sett_inn_bruker_Ikke_ok()
        {

            Registrer NyBruker = new Registrer()
            {
                Fornavn = "Mats",
                Etternavn = "Lokken",
                Email = "testing123@hotmail.com",
                Telefonnummer = "93686771",
                BekreftPassord = "password"
            };
            
            bool ok = this.mockProductRepository.RegistrerBruker(NyBruker);
            Assert.IsFalse(ok);
        }
        [TestMethod]
        public void Hent_Vakter_Ok()
        {
            var testProduct = this.mockProductRepository.HentVakter("testing123@hotmail.com",1);
            for (var i = 0; i < testProduct.Count; i++)
            {
                Assert.AreEqual(1, testProduct[i].ProsjektId);

            }
            Assert.AreNotEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<Timeliste>)); // Test type

        }
        [TestMethod]
        public void Hent_BrukerInfo_Moq()
        {
            var _mock = new Mock<InterfaceDbTBruker>();

            var info = new List<Profil>() { new Profil()
            {
                id = 1,
                Fornavn = "mats",
                Etternavn = "Løkken",
                Email = "mats_loekken@hotmail.com",
                Adresse = "Kirkeveien 67"
             }};

            _mock.Setup(x => x.HentBruker(It.IsAny<string>())).Returns(info);
            _mock.Verify(framework => framework.HentBruker("mats_loekken@hotmail.com"), Times.AtMostOnce());

            InterfaceDbTBruker lovable = _mock.Object;
            var actual = lovable.HentBruker("mats_loekken@hotmail.com");

            Assert.AreEqual(info, actual);
        }

        [TestMethod]
        public void Gi_AdminTilgang_Feil()
        {
            Sjef s = new Sjef();
            s.BrukerId = 2;
            s.ProsjektId = 1;

            bool ok = this.mockProductRepository.GiBrukerAdminTilgang(s, "ikkeadmin@hotmail.com");
            Assert.IsFalse(ok);
        }

        [TestMethod]
        public void Gi_AdminTilgang_Ok()
        {
            Sjef s = new Sjef();
            s.BrukerId = 2;
            s.ProsjektId = 1;
            
            bool ok = this.mockProductRepository.GiBrukerAdminTilgang(s,"testing123@hotmail.com");
            Assert.IsTrue(ok);
        }
        [TestMethod]
        public void Fjern_AdminTilgang_Feil()
        {
            Sjef s = new Sjef();
            s.BrukerId = 2;
            s.ProsjektId = 1;

            bool ok = this.mockProductRepository.FjernAdminTilgang(s, "ikkeadmin@hotmail.com");
            Assert.IsFalse(ok);
        }

        [TestMethod]
        public void Fjern_AdminTilgang_Ok()
        {
            Sjef s = new Sjef();
            s.BrukerId = 2;
            s.ProsjektId = 1;

            bool ok = this.mockProductRepository.FjernAdminTilgang(s, "testing123@hotmail.com");
            Assert.IsTrue(ok);
        }
        [TestMethod]
        public void EmailDB_Ok()
        {
            Registrer s = new Registrer();
            s.Email = "testing123@hotmail.com";
           
            bool ok = this.mockProductRepository.EmailDb(s);
            Assert.IsTrue(ok);
        }
    }
}
