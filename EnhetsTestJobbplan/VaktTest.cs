using System;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Jobbplan;
using Jobbplan.Controllers;
using Jobbplan.Models;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Transactions;
using System.Net;
using System.Linq;
using System.Collections.Generic;
namespace EnhetsTestJobbplan
{
   
    [TestClass]
    public class VaktTest
    {

        [TestMethod]
        public void Hent_Vakter_Moq()
        {

            var _mock = new Mock<InterfaceDbTVakt>();
          
            var vakter = new List<Vaktkalender>() { new Vaktkalender()
            {  start = Convert.ToDateTime("22.12.2012 16.43"),
                end =  Convert.ToDateTime("22.12.2012 16.43"),
                title = "Dagvakt",
                Beskrivelse = "Opplæring"
             } };

            _mock.Setup(x => x.hentAlleVakter(1, It.IsAny<string>())).Returns(vakter);
            _mock.Verify(framework => framework.hentAlleVakter(1, "mats_loekken@hotmail.com"), Times.AtMostOnce());

            InterfaceDbTVakt lovable = _mock.Object;
            var download = lovable.hentAlleVakter(1, "mats_loekken@hotmail.com");

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
            InterfaceDbTVakt lovable = Mock.Of<InterfaceDbTVakt>(l =>l.RegistrerVakt(vakter, "mats_loekken@hotmail.com") == false);

            // Hand the instance as a collaborator and exercise it, 
            // like calling methods on it...
            bool download = lovable.RegistrerVakt(vakter, "mats_loekken@hotmail.com");

            // Simply assert the returned state:
            Assert.IsFalse(download);

            // If you really want to go beyond state testing and want to 
            // verify the mock interaction instead...
            Mock.Get(lovable).Verify(framework => framework.RegistrerVakt(vakter, "mats_loekken@hotmail.com"));
        }
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
        [TestMethod]
        public void SettInnVaktOk()
        {
            using (TransactionScope scope = new TransactionScope())
             {
                 InterfaceDbTVakt studentRepository = new DbTransaksjonerVakt();
                 Vaktskjema vakt = new Vaktskjema()
                  {
                      start = "22.12.2012 07.43",
                      end = "22.12.2012 15.43",
                      title = "Dagvakt",
                      Beskrivelse = "Opplæring",
                      BrukerId = 1,
                      ProsjektId = 1
                  };
       
                bool id = studentRepository.RegistrerVakt(vakt,"mats_loekken@hotmail.com");
                 Assert.AreEqual(true, id);
             }
        }
        [TestMethod]
        public void SettInnVakt_End_Before_start()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTVakt Dbt = new DbTransaksjonerVakt();
                Vaktskjema vakt = new Vaktskjema()
                {
                    start = "22.12.2012 16.43",
                    end = "22.12.2012 15.43",
                    title = "Dagvakt",
                    Beskrivelse = "Opplæring",
                    BrukerId = 1,
                    ProsjektId = 1
                };
                bool id = Dbt.RegistrerVakt(vakt, "mats_loekken@hotmail.com");
                Assert.AreEqual(false, id);
            }
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
