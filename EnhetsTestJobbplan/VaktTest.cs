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
using System.Web.Http.Hosting;

namespace EnhetsTestJobbplan
{
   
    [TestClass]
    public class VaktTest
    {
        private InterfaceDbTVakt mockProductRepository;
        public VaktTest()
        { 
            // Lager mocks som vi kan gjøre spørringer mot
           
            List<Vaktkalender> vakter = new List<Vaktkalender>
                {
                   // new Vaktkalender {ProsjektId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"},
                   // new Vaktkalender {ProsjektId = 2, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring" },
                    //new Vaktkalender {ProsjektId = 3, Brukernavn = "", start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"}
                };
            List<Vakt> vakterDB = new List<Vakt>
                {
                    new Vakt {ProsjektId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"},
                    new Vakt {ProsjektId = 2, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring" },
                    new Vakt {ProsjektId = 3, BrukerId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"},
                    new Vakt {ProsjektId = 3, BrukerId = 0, Ledig = true, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"}
            };
            List<dbBruker> bruker = new List<dbBruker>
                {
                    new dbBruker {BrukerId = 1, Email = "testing123@hotmail.com"}
                };
            List<Maler> maler = new List<Maler>
                {
                    new Maler {ProsjektId=1,startTid = "07.15", sluttTid = "14.15", Tittel = "dagvakt"}
                };

            Mock<InterfaceDbTVakt> mockProductRepository = new Mock<InterfaceDbTVakt>();


            /* mockProductRepository.Setup(mr => mr.hentAlleVakter(It.IsAny<int>(),It.IsAny<string>()))
                 .Returns((int i,string u) => 
                 vakter.Where(x => x.ProsjektId == i).ToList());

             */
            List<Vakt> myFilteredFoos = null;
            mockProductRepository.Setup(mr => mr.hentAlleVakter(It.IsAny<int>(), It.IsAny<string>()))
              .Callback((int i, string u) =>
                myFilteredFoos = vakterDB.Where(x => x.ProsjektId == i).ToList())
              .Returns(() =>
              {
                  List<Vaktkalender> testliste = new List<Vaktkalender>();
                  foreach (var vakt in myFilteredFoos)
                  {
                      testliste.Add(new Vaktkalender() { ProsjektId = vakt.ProsjektId,  start = vakt.start, end = vakt.end, title = vakt.title, Beskrivelse = vakt.Beskrivelse });
                  }
                  return testliste;
              });

            List<Maler> malene = null;
            mockProductRepository.Setup(mr => mr.hentAlleMaler(It.IsAny<int>(), It.IsAny<string>()))
            .Callback((int i, string u) =>
            malene = maler.Where(x => x.ProsjektId == i).ToList())
            .Returns(() =>
             {
              List<VisMaler> testliste = new List<VisMaler>();
                foreach (var mal in malene)
                {
                testliste.Add(new VisMaler() { ProsjektId = mal.ProsjektId, startTid = mal.startTid, sluttTid = mal.sluttTid, Tittel= mal.Tittel });
                }
                return testliste;
            });


            mockProductRepository.Setup(mr => mr.hentAlleLedigeVakter(It.IsAny<int>(), It.IsAny<string>()))
             .Callback((int i, string u) =>
               myFilteredFoos = vakterDB.Where(x => x.ProsjektId == i && x.BrukerId == 0).ToList())
             .Returns(() =>
             {
                 List<Vaktkalender> testliste = new List<Vaktkalender>();
                 foreach (var vakt in myFilteredFoos)
                 {
                     testliste.Add(new Vaktkalender() { ProsjektId = vakt.ProsjektId, Ledig = vakt.Ledig, start = vakt.start, end = vakt.end, title = vakt.title, Beskrivelse = vakt.Beskrivelse });
                 }
                 return testliste;
             });
          
            mockProductRepository.Setup(mr => mr.hentAlleVakterForBruker(It.IsAny<string>()))
           .Returns((string u) =>
           {
               var user = bruker.Where(x => x.Email == u ).Single();
               int id = user.BrukerId;
               myFilteredFoos = vakterDB.Where(x => x.BrukerId == id).ToList();
               List<Vaktkalender> testliste = new List<Vaktkalender>();
               foreach (var vakt in myFilteredFoos)
               {
                   testliste.Add(new Vaktkalender() { ProsjektId = vakt.ProsjektId, Brukernavn = user.Email, Ledig = vakt.Ledig, start = vakt.start, end = vakt.end, title = vakt.title, Beskrivelse = vakt.Beskrivelse });
               }
               return testliste;
           });
            //.Callback((int i, string u) => vakterDB.Where(x => x.ProsjektId == i))
            //.Returns((int i, string u) => vakter.Where(x => x.ProsjektId == i).ToList());
            // Allows us to test saving a product

          

            mockProductRepository.Setup(mr => mr.RegistrerVakt(It.IsAny<Vaktskjema>(), It.IsAny<string>())).Returns(
                (Vaktskjema vakt, string u) =>
                {

                    Vakt nyVakt = new Vakt();
                    nyVakt.ProsjektId = vakt.ProsjektId;
                    nyVakt.BrukerId = vakt.ProsjektId;
                    string start = vakt.start + " " + vakt.startTid;

                    IFormatProvider culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                    DateTime dt1 = DateTime.ParseExact(start, "dd.MM.yyyy H:mm", culture, System.Globalization.DateTimeStyles.AssumeLocal);
                    DateTime dt2;
                    string end = "";
                    if (vakt.end != null)
                    {
                        end = vakt.end + " " + vakt.endTid;
                        dt2 = DateTime.ParseExact(end, "dd.MM.yyyy H:mm", culture, System.Globalization.DateTimeStyles.AssumeLocal);
                    }
                    else
                    {
                        end = vakt.start + " " + vakt.endTid;
                        dt2 = DateTime.ParseExact(end, "dd.MM.yyyy H:mm", culture, System.Globalization.DateTimeStyles.AssumeLocal);
                    }
                    nyVakt.start = dt1;
                    nyVakt.end = dt2;

                    int result = DateTime.Compare(dt1, dt2);
                    if (result > 0 || result == 0)
                    {
                        return false;
                       
                    }
                    else
                    {
                        vakterDB.Add(nyVakt);
                    }

                    return true;
                });

            // return a product by Name
            //  mockProductRepository.Setup(mr => mr.hentAlleLedigeVakter(It.IsAny<int>(),It.IsAny<string>())).Returns((int s, string u) => vakter.Where(x => x.ProsjektId == s && x.Brukernavn==u).ToList());

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
        [TestMethod]
        public void Hent_alle_vakter_Ikke_ok()
        {
            // Try finding a product by id
            List<Vaktkalender> testProduct = this.mockProductRepository.hentAlleVakter(0, "mats_loekken@hotmail.com");
            Assert.AreEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<Vaktkalender>)); // Test type
        }
        [TestMethod]
        public void Hent_alle_Ledige_vakter_ok()
        {
            // Try finding a product by id
            List<Vaktkalender> testProduct = this.mockProductRepository.hentAlleLedigeVakter(3,"mats_loekken@hotmail.com");
            for (var i = 0; i < testProduct.Count; i++)
            {
                Assert.AreEqual(true, testProduct[i].Ledig);

            }
            Assert.AreNotEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<Vaktkalender>)); // Test type
        }
      
        [TestMethod]
        public void Hent_alle_vakter_for_bruker_ok()
        {
            // Try finding a product by id
            List<Vaktkalender> testProduct = this.mockProductRepository.hentAlleVakterForBruker("testing123@hotmail.com");
            for (var i = 0; i < testProduct.Count; i++)
            {
                Assert.AreEqual("testing123@hotmail.com", testProduct[i].Brukernavn);

            }
            Assert.AreNotEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<Vaktkalender>)); // Test type
        }
        [TestMethod]
        public void Hent_alle_Maler_ok()
        {
            // Try finding a product by id
            List<VisMaler> testProduct = this.mockProductRepository.hentAlleMaler(1, "mats_loekken@hotmail.com");
            for (var i = 0; i < testProduct.Count; i++)
            {
                Assert.AreEqual(1, testProduct[i].ProsjektId);

            }
            Assert.AreNotEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<VisMaler>)); // Test type
        }


        [TestMethod]
        public void Registrer_Vakt_End_before_start()
        {
            // Try finding a product by id

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
            bool ok = this.mockProductRepository.RegistrerVakt(vakt, "mats_loekken@hotmail.com");

            Assert.IsFalse(ok);


        }
        [TestMethod]
        public void Registrer_Vakt_OK()
        {
            // Try finding a product by id

            Vaktskjema vakt = new Vaktskjema()
            {
                start = "22.12.2012",
                startTid = "16.43",
                endTid = "18.43",
                title = "Dagvakt",
                Beskrivelse = "Opplæring",
                BrukerId = 1,
                ProsjektId = 1
            };
            bool ok = this.mockProductRepository.RegistrerVakt(vakt, "mats_loekken@hotmail.com");

            Assert.IsTrue(ok);
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
       
        //DbtransaksjonerVakt
       
      
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
