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
using Jobbplan.Model;
using Jobbplan.BLL;
using Jobbplan.DAL;

namespace EnhetsTestJobbplan
{
    
    [TestClass]
    public class BrukerTest
    {

        //Inneholder Tester for BrukerApiController, BrukerController, DbtransaksjonerBruker
        //REGISTRER Bruker
        //BrukerController
      
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
        [TestMethod]
        public void Registrer_POST_Ok()
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
        public void Registrer_Feil_Validering_Bruker()
        {
            //Arrange
            var controller = new BrukerApiController();
            var innBruker = new Registrer();
            controller.ModelState.AddModelError("Fornavn", "Fornavn må oppgis");
            //Act
            var result = controller.Post(innBruker);
            //Assert

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }
        //DBTransaksjonerBruker
        [TestMethod]
        public void Registrer_Test_Bruker_Finnes()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTBruker studentRepository = new DbTransaksjonerBruker();
                Registrer NyBruker = new Registrer()
                {
                    Email = "mats123@hotmail.com"        
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
                    Email="tesbruker@hotmail.com",
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
            // Arrange 
            var mockEntityRepository = new Mock<InterfaceDbTBruker>();
            mockEntityRepository.Setup(m => m.RegistrerBruker(It.IsAny<Registrer>()));
            var entity = new BrukerBLL();
            // Act 
            var name = entity.RegistrerBruker(NyBruker);
            // Assert
            mockEntityRepository.Verify(m => m.RegistrerBruker(It.IsAny<Registrer>()), Times.AtLeastOnce);
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
        {
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTBruker Bruker = new DbTransaksjonerBruker();
                Sjef tomBruker = new Sjef();

                bool actual = Bruker.GiBrukerAdminTilgang(tomBruker,"");
                Assert.AreEqual(false, actual);
            }
        }

        
    }
}
