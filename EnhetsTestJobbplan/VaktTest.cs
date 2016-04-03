using System;
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

namespace EnhetsTestJobbplan
{
   
    [TestClass]
    public class VaktTest
    {
        
        [TestMethod]
        public void SettInnVaktOkMOCK()
        {
            var vakter = new Vaktskjema();
            
            var moq = new Mock<InterfaceDbTVakt>();
            moq.Setup(foo => foo.RegistrerVakt(vakter, "mats_loekken@hotmail.com")).Returns(true);
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
                InterfaceDbTVakt studentRepository = new DbTransaksjonerVakt();
                Vaktskjema vakt = new Vaktskjema()
                {
                    start = "22.12.2012 16.43",
                    end = "22.12.2012 15.43",
                    title = "Dagvakt",
                    Beskrivelse = "Opplæring",
                    BrukerId = 1,
                    ProsjektId = 1
                };
                bool id = studentRepository.RegistrerVakt(vakt, "mats_loekken@hotmail.com");
                Assert.AreEqual(false, id);
            }
        }
    }
}
