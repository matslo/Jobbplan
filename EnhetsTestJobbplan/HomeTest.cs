using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using Jobbplan;
using Jobbplan.Controllers;
using System.Web.Mvc;
using Jobbplan.Models;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Net;
using System.Transactions;
using Moq;


namespace EnhetsTestJobbplan
{
    [TestClass]
    public class HomeTest
    {
        //Inneholder Tester for HomeController, AuthenticateController, DbtransaksjonerBruker

        //BrukerController
        [TestMethod]
        public void IndexTest()
        {
            //Arrange
            var controller = new HomeController();
            //Act
            var result = controller.Index() as ViewResult;
            //Assert
            Assert.AreEqual(result.ViewName, ""); 
        }
        //AuthenticateController
        [TestMethod]
        public void LoggInn_POST_Ok()
        {
          // Test på FormsAuthentication --
        }
        [TestMethod]
        public void LoggInn_Feil_Validering()
        {
            //Arrange
            var controller = new AuthenticateController();
            var innBruker = new LogInn();
            controller.ModelState.AddModelError("Brukernavn", "Brukernavn må oppgis");
            //Act
            var result = controller.Post(innBruker);
            //Assert

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }
        //DbtransaksjonerBruker
        [TestMethod]
        public void Bruker_Er_Ikke_I_DB()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTBruker studentRepository = new DbTransaksjonerBruker();
                LogInn NyBruker = new LogInn()
                { 
                    Brukernavn = "test",
                    Passord = "123123"
                };

                bool actual = studentRepository.BrukerIdb(NyBruker);
                Assert.AreEqual(false, actual);
            }
        }
        [TestMethod]
        public void Bruker_Er_I_DB_OK()
        {
          var _mock = new Mock<InterfaceDbTBruker>();
            var Loggin = new LogInn()
            {
                Brukernavn = "mats_loekken@hotmail.com",
                Passord = "Password123",          
             };
            _mock.Setup(x => x.BrukerIdb(Loggin)).Returns(true);
            _mock.Verify(framework => framework.BrukerIdb(Loggin), Times.AtMostOnce());

            InterfaceDbTBruker lovable = _mock.Object;
            var actual = lovable.BrukerIdb(Loggin);

            Assert.AreEqual(true, actual);
            
        }
   

    }
}
