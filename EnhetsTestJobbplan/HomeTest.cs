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


namespace EnhetsTestJobbplan
{
    [TestClass]
    public class HomeTest
    {
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
        
        [TestMethod]
        public void IndexLoggInnTestFeilBrukernavnPassord()
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
        public void IndexBrukerErI_DB_OK()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                InterfaceDbTBruker studentRepository = new DbTransaksjonerBruker();
                LogInn NyBruker = new LogInn()
                {
                    Brukernavn = "gordo@hotmail.com",
                    Passord = "Mats1414"
                };

                bool actual = studentRepository.BrukerIdb(NyBruker);
                Assert.AreEqual(true, actual);
            }
        }
    }
}
