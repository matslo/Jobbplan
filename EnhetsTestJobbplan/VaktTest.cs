using System;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Jobbplan;
using Jobbplan.Controllers;
using Jobbplan.Model;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Transactions;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Jobbplan.BLL;
using Jobbplan.DAL;

namespace EnhetsTestJobbplan
{
   
    [TestClass]
    public class VaktTest
    {
        private InterfaceDbTVakt mockProductRepository;
        public VaktTest()
        { 
            // create some mock products to play with
           
            List<Vaktkalender> vakter = new List<Vaktkalender>
                {
                    new Vaktkalender {ProsjektId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"},
                    new Vaktkalender {ProsjektId = 2, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring" },
                    new Vaktkalender {ProsjektId = 3, Brukernavn = "", start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"}
                };
            List<Vakt> vakterDB = new List<Vakt>
                {
                    new Vakt {ProsjektId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"},
                    new Vakt {ProsjektId = 2, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring" },
                    new Vakt {ProsjektId = 3, BrukerId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"}
                };
            List<dbBruker> bruker = new List<dbBruker>
                {
                    new dbBruker {BrukerId = 1, Email = "testing123@hotmail.com"}
                };
            // Mock the Products Repository using Moq
            Mock<InterfaceDbTVakt> mockProductRepository = new Mock<InterfaceDbTVakt>();

            // Return all the products

            // return a product by Id
            mockProductRepository.Setup(mr => mr.hentAlleVakter(It.IsAny<int>(),It.IsAny<string>()))
                .Returns((int i,string u) => 
                vakter.Where(x => x.ProsjektId == i).ToList());

            /*mockProductRepository.Setup(mr => mr.hentAlleVakter(It.IsAny<int>(), It.IsAny<string>()))
                .Callback((int i, string u) =>
                    vakterDB.Where(x => x.ProsjektId == i ))
                .Returns((int i,string u) =>
                    new List<Vaktkalender>() {new Vaktkalender(){ProsjektId = i}});*/
               //vakter.Where(x => x.ProsjektId == i).ToList());



            //.Callback((int i, string u) => vakterDB.Where(x => x.ProsjektId == i))
            //.Returns((int i, string u) => vakter.Where(x => x.ProsjektId == i).ToList());


            // return a product by Name
            mockProductRepository.Setup(mr => mr.hentAlleLedigeVakter(It.IsAny<int>(),It.IsAny<string>())).Returns((int s, string u) => vakter.Where(x => x.ProsjektId == s && x.Brukernavn==u).ToList());

            // Allows us to test saving a product
         
            // Complete the setup of our Mock Product Repository
            this.mockProductRepository = mockProductRepository.Object;
        }
        [TestMethod]
        public void Hent_alle_vakter_ok()
        {
            // Try finding a product by id
            List<Vaktkalender> testProduct = this.mockProductRepository.hentAlleVakter(3,"mats_loekken@hotmail.com");
            for (var i = 0; i < testProduct.Count; i++)
            {
              Assert.AreEqual(3, testProduct[i].ProsjektId);
               
            }
           Assert.AreNotEqual(0,testProduct.Count); // Test if null
           Assert.IsInstanceOfType(testProduct, typeof(List<Vaktkalender>)); // Test type
          }
        //Inneholder Tester for VaktApiController/2/3, VaktController, DbtransaksjonerVakt
        //VaktController
        [TestMethod]
        public void Index_Test_Vakt()
        {
            //Arrange
            var controller = new VaktController();
            //Act
            var result = controller.Index() as ViewResult;
            //Assert
            Assert.AreEqual(result.ViewName, "");
        }
        //VaktApiController
        [TestMethod]
        public void RegistrerFeilValideringTestVakt()
        {
            //Arrange
            var controller = new VaktApiController();
            var innBruker = new Vaktskjema();
            controller.ModelState.AddModelError("start", "Dato må oppgis");
            //Act
            var result = controller.Post(innBruker);
            //Assert
           
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }
        //DbtransaksjonerVakt
        [TestMethod]
        public void Hent_Vakter_Moq()
        {

            var _mock = new Mock<InterfaceDbTVakt>();
            List<Vaktkalender> vakter = new List<Vaktkalender>
                {
                    new Vaktkalender {ProsjektId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"},
                    new Vaktkalender {ProsjektId = 2, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring" },
                    new Vaktkalender {ProsjektId = 3, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"}
                };
         

            _mock.Setup(x => x.hentAlleVakter(1, It.IsAny<string>())).Returns(vakter);
            _mock.Verify(framework => framework.hentAlleVakter(1, "mats_loekken@hotmail.com"), Times.AtMostOnce());

            InterfaceDbTVakt lovable = _mock.Object;
            var download = lovable.hentAlleVakter(1, "mats_loekken@hotmail.com");

            Assert.AreEqual(download, vakter);

        }
        [TestMethod]
        public void Hent_Vakter_Bruker_Moq()
        {

            var _mock = new Mock<InterfaceDbTVakt>();

            var vakter = new List<Vaktkalender>() { new Vaktkalender()

            {
                Brukernavn = "mats_loekken@hotmail.com",
                start = Convert.ToDateTime("22.12.2012 16.43"),
                end =  Convert.ToDateTime("22.12.2012 16.43"),
                title = "Dagvakt",
                Beskrivelse = "Opplæring"
             } };

            _mock.Setup(x => x.hentAlleVakterBruker(1, "mats_loekken@hotmail.com")).Returns(vakter);
            _mock.Verify(framework => framework.hentAlleVakterBruker(1, "mats_loekken@hotmail.com"), Times.AtMostOnce());

            InterfaceDbTVakt lovable = _mock.Object;
            var download = lovable.hentAlleVakterBruker(1, "mats_loekken@hotmail.com");

            Assert.AreEqual(download, vakter);

        }
        [TestMethod]
        public void SettInnVaktOkMOCK()
        {
            var vakter = new Vaktskjema();
            var vakte = new Vaktskjema();
            var moq = new Mock<InterfaceDbTVakt>();
            moq.Setup(foo => foo.RegistrerVakt(vakter, "mats_loekken@hotmail.com")).Returns(true);

            InterfaceDbTVakt lovable = moq.Object;
            bool download = lovable.RegistrerVakt(vakter, "mats_loekken@hotmail.com");

            moq.Verify(framework => framework.RegistrerVakt(vakte, "mats_loekken@hotmail.com"), Times.AtMostOnce());
        }
        [TestMethod]
        public void SettInnVaktikkeOkMOCK()
        {

            Vaktskjema vakter = new Vaktskjema()
            {
                start = "22.12.2012 16.43",
                end = "22.12.2012 15.43",
                title = "Dagvakt",
                Beskrivelse = "Opplæring",
                BrukerId = 1,
                ProsjektId = 1
            };
            InterfaceDbTVakt lovable = Mock.Of<InterfaceDbTVakt>(l => l.RegistrerVakt(vakter, "mats_loekken@hotmail.com") == false);

            bool download = lovable.RegistrerVakt(vakter, "mats_loekken@hotmail.com");

            Assert.IsFalse(download);
            Mock.Get(lovable).Verify(framework => framework.RegistrerVakt(vakter, "mats_loekken@hotmail.com"));
        }     
        [TestMethod]
        public void SettInnVakt_End_Before_start()
        {
            
                Vaktskjema vakt = new Vaktskjema()
                {
                    start = "22.12.2012",
                    startTid = "16.43",
                    endTid = "15.43",
                    title = "Dagvakt",
                    Beskrivelse = "Opplæring",
                    BrukerId = 1,
                    ProsjektId = 1
                };
            Vaktskjema vakt2 = new Vaktskjema()
            {
                start = "22.12.2012",
                end = "22.12.2012",
                startTid = "16.43",
                endTid = "17.43",
                title = "Dagvakt",
                Beskrivelse = "Opplæring",
                BrukerId = 1,
                ProsjektId = 1
            };
            var _mock = new Mock<InterfaceDbTVakt>();
            var _target = new VaktBLL(_mock.Object);
           
            _mock.Setup(x => x.RegistrerVakt(vakt2,It.IsAny<String>())).Returns(true);
            
            bool expected = false;
            bool actual;
            actual = _target.RegistrerVakt(vakt,"mats_loekken@hotmail.com");
            _mock.Verify(e => e.RegistrerVakt(It.Is<Vaktskjema>(d => d.start == "22.12.2012" && d.startTid=="17.43" && d.endTid=="16.43"),It.IsAny<String>()), Times.Never);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void SettInnMal_OK()
        {
            var _mock = new Mock<InterfaceDbTVakt>();

            MalerSkjema maler = new MalerSkjema()
            {
                ProsjektId = 1,
                startTid = "12:15",
                sluttTid = "13.15"
            };
            var brukernavn = new dbBruker();
            

            _mock.Setup(x => x.RegistrerMal(maler, brukernavn.Email)).Returns(true);
            _mock.Verify(framework => framework.RegistrerMal(maler, brukernavn.Email), Times.AtMostOnce());

            InterfaceDbTVakt lovable = _mock.Object;
            var actual = lovable.RegistrerMal(maler, brukernavn.Email);

            Assert.AreEqual(true, actual);
        }
       
        [TestMethod]
        public void Ledig_Vakt()
        {
            var dbtv = new DbTransaksjonerVakt();
            
            var innVakt = new Vaktskjema();
            innVakt.BrukerId = 0;

            var actual = dbtv.LedigVakt(innVakt);

            Assert.AreEqual(true, actual);
        }
        [TestMethod]
        public void Ikke_Ledig_Vakt()
        {

            var dbtv = new DbTransaksjonerVakt();

            var innVakt = new Vaktskjema();
            innVakt.BrukerId = 1;

            var actual = dbtv.LedigVakt(innVakt);

            Assert.AreEqual(false, actual);

        }
        
    }
}
