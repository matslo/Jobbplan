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
        public BrukerTest()
        {
            // Lager mock som vi kan gjøre spørringer mot

            List<BrukerListe> brukere = new List<BrukerListe>
                {
                   new BrukerListe() {Brukernavn = "mats_loekken@hotmail.com", ProsjektDeltakerId = 1, Admin = false, Navn="Mats"},
                    new BrukerListe() {Brukernavn = "gordo@hotmail.com", ProsjektDeltakerId = 2, Admin=true, Navn = "Gordo"}
                };
            List<dbBruker> brukereDB = new List<dbBruker>
                {
                   new dbBruker() {Email = "mats_loekken@hotmail.com", BrukerId = 1},
                   new dbBruker() {Email = "gordo@hotmail.com", BrukerId = 2}
                };
            List<Vakt> vakterDB = new List<Vakt>
                {
                    new Vakt {ProsjektId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"},
                    new Vakt {ProsjektId = 2, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring" },
                    new Vakt {ProsjektId = 3, BrukerId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"}
                };
            List<Prosjektdeltakelse> deltakelser = new List<Prosjektdeltakelse>
                {
                    new Prosjektdeltakelse() {BrukerId = 1, ProsjektId = 1, ProsjektDeltakerId = 1, Admin = false},
                    new Prosjektdeltakelse() {BrukerId = 2, ProsjektId = 2, ProsjektDeltakerId = 1, Admin = false}
                };
            List<Prosjekt> prosjekt  = new List<Prosjekt>
                {
                    new Prosjekt() {ProsjektId = 1, EierId = 2},
                    new Prosjekt() {ProsjektId = 2, EierId = 1}
                };

            // Mock the Products Repository using Moq
            Mock<InterfaceDbTBruker> mockProductRepository = new Mock<InterfaceDbTBruker>();

            // Return all the products

            // return a product by Id
            /* mockProductRepository.Setup(mr => mr.HentBrukere(It.IsAny<int>(), It.IsAny<string>()))
                 .Returns((int i, string u) =>
                 brukere.Where(x => x.ProsjektDeltakerId == i).ToList());*/

            //.Callback((int i, string u) =>

            //deltakelser.Where(x => x.ProsjektId == i).ToList())
                
             
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

                   // {});

                   //new List<Vaktkalender>() {new Vaktkalender(){ProsjektId = i}});
                   //vakter.Where(x => x.ProsjektId == i).ToList());



                   //.Callback((int i, string u) => vakterDB.Where(x => x.ProsjektId == i))
                   //.Returns((int i, string u) => vakter.Where(x => x.ProsjektId == i).ToList());


                   // return a product by Name
                   //  mockProductRepository.Setup(mr => mr.hentAlleLedigeVakter(It.IsAny<int>(), It.IsAny<string>())).Returns((int s, string u) => vakter.Where(x => x.ProsjektId == s && x.Brukernavn == u).ToList());

                   // Allows us to test saving a product

                   // Complete the setup of our Mock Product Repository
                   this.mockProductRepository = mockProductRepository.Object;
        }

        //Inneholder Tester for BrukerApiController, BrukerController, DbtransaksjonerBruker
        //REGISTRER Bruker
        //BrukerController
        [TestMethod]
        public void Hent_brukere_ok()
        {
            // Try finding a product by id
            List<BrukerListe> testProduct = this.mockProductRepository.HentBrukere(2, "mats_loekken@hotmail.com");
            for (var i = 0; i < testProduct.Count; i++)
            {
                Assert.AreEqual(2, testProduct[i].ProsjektId);

            }
            Assert.AreNotEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<BrukerListe>)); // Test type
        }
        [TestMethod]
        public void Hent_brukere_Ikke_Vis_Ikke_Admin()
        {
            // Try finding a product by id
            List<BrukerListe> testProduct = this.mockProductRepository.HentBrukere(1, "mats_loekken@hotmail.com");
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
        //BrukerApiController
        /* MÅ TESTES
        // GET api/BrukerApi/4
        public List<BrukerListe> Get (int id)
       {
            string brukernavn = User.Identity.Name;
            return db.HentBrukere(id,brukernavn);
          
        }
        *//*
        [TestMethod]
        public void Get_Brukere_Ok()
        {
            var data = new List<dbBruker>
            {
                new dbBruker() { Fornavn = "mats", Etternavn = "Løkken", BrukerId = 1, Email = "mats_loekken@hotmail.com"}
            }.AsQueryable();
            var data1 = new List<Profil>
            {
                new Profil() {Fornavn = "mats", Etternavn = "Løkken", id = 1, Email = "mats_loekken@hotmail.com"}
            };

            var mockSet = new Mock<DbSet<dbBruker>>();
            mockSet.As<IQueryable<dbBruker>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<dbBruker>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<dbBruker>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<dbBruker>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IBrukerLogikk>();
            mockContext.Setup(c => c.HentBruker("mats_loekken@hotmail.com")).Returns(data1);

            var service = new Mock<InterfaceDbTBruker>(mockContext.Object);
            var blogs = service.HentBruker("mats_loekken@hotmail.com");

            Assert.AreEqual(1, blogs.Count);
            Assert.AreEqual(1, blogs[0].id);
        }*/
       
     
       
        //DBTransaksjonerBruker
        [TestMethod]
        public void Registrer_Test_Bruker_Finnes()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTBruker studentRepository = new DbTransaksjonerBruker();
                Registrer NyBruker = new Registrer()
                {
                    Fornavn = "Mats",
                    Etternavn = "Lokken",
                    Email = "mats_loekken@hotmail.com",
                    Telefonnummer = "93686771",
                    BekreftPassord = "password"
                };

                bool actual = studentRepository.RegistrerBruker(NyBruker);
                Assert.AreEqual(false, actual);
            }
        }    
        [TestMethod]
        public void Sett_Inn_Bruker_I_Db_Ok()
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
        public void Sett_Inn_Bruker_I_Db_Ok_moq()
        {
            Registrer NyBruker = new Registrer()
            {
                Fornavn = "Mats",
                Etternavn = "Lokken",
                Email = "tesbruker@hotmail.com",
                Telefonnummer = "93686771",
                BekreftPassord = "password"
            };
         
            var _mock = new Mock<InterfaceDbTBruker>();
            var _target = new BrukerBLL(_mock.Object);

            _mock.Setup(x => x.RegistrerBruker(It.IsAny<Registrer>())).Returns(true);

            bool expected = true;
            bool actual;
            actual = _target.RegistrerBruker(NyBruker);
            //_mock.Verify(e => e.RegistrerBruker(It.Is<Registrer>(d => d. == "22.12.2012" && d.startTid == "17.43" && d.endTid == "16.43"), It.IsAny<String>()), Times.Never);
            _mock.Verify(m => m.RegistrerBruker(It.IsAny<Registrer>()), Times.Once);
            Assert.AreEqual(expected, actual); 
        }
        [TestMethod]
        public void Sett_inn_bruker_Ikke_ok()
        {

            Registrer NyBruker = new Registrer()
            {
                Fornavn = "Mats",
                Etternavn = "Lokken",
                Email = "tesbruker@hotmail.com",
                Telefonnummer = "93686771",
                BekreftPassord = "password"
            };

            var _mock = new Mock<InterfaceDbTBruker>();
            var _target = new BrukerBLL(_mock.Object);

            _mock.Setup(x => x.RegistrerBruker(NyBruker)).Returns(false);

            bool expected = false;
            bool actual;
            actual = _target.RegistrerBruker(NyBruker);
//            _mock.Verify(e => e.RegistrerVakt(It.Is<Registrer>(d => d.start == "22.12.2012" && d.startTid == "17.43" && d.endTid == "16.43"), It.IsAny<String>()), Times.Never);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void Hent_Timeliste_Moq()
        {
            var _mock = new Mock<InterfaceDbTBruker>();

            var timer = new List<Timeliste>() { new Timeliste()
            {
                PeriodeStart = Convert.ToDateTime("22.12.2012 16.43"),
                PeriodeSlutt =  Convert.ToDateTime("22.12.2012 16.43")
             }};

            _mock.Setup(x => x.HentVakter(It.IsAny<string>())).Returns(timer);
            _mock.Verify(framework => framework.HentVakter("mats_loekken@hotmail.com"), Times.AtMostOnce());

            InterfaceDbTBruker lovable = _mock.Object;
            var actual = lovable.HentVakter("mats_loekken@hotmail.com");

            Assert.AreEqual(timer,actual);
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
        public void Gi_AdminTilgang_Moq()
        {
            var _mock = new Mock<InterfaceDbTBruker>();

            Sjef adminBruker = new Sjef()
            {
                 ProsjektId= 1,
                 BrukerId = 2
            };
            
            _mock.Setup(x => x.GiBrukerAdminTilgang(adminBruker,It.IsAny<string>())).Returns(true);
            _mock.Verify(framework => framework.GiBrukerAdminTilgang(adminBruker, "mats_loekken@hotmail.com"), Times.AtMostOnce());

            InterfaceDbTBruker dbtb = _mock.Object;
            var actual = dbtb.GiBrukerAdminTilgang(adminBruker, "mats_loekken@hotmail.com");

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void Gi_AdminTilgang_Feil()
        {/*
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTBruker Bruker = new DbTransaksjonerBruker();
                Sjef tomBruker = new Sjef();
                
                bool actual = Bruker.GiBrukerAdminTilgang(tomBruker,"mats_loekken@hotmail.com");
                Assert.AreEqual(false, actual);
            }*/
        }

        
    }
}
