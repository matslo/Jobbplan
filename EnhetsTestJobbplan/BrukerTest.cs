using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Mvc;
using Jobbplan;
using Jobbplan.Controllers;
using Jobbplan.Models;
using System.Collections.Generic;
using System.Transactions;
using Moq;
using System.Net;

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
          
        }*/
        [TestMethod]
        public void Get_Brukere_Ok()
        {
           
        }
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
                    Email = "gordo@hotmail.com"        
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
                    Adresse="Kirkeveien 67",
                    Email="tesbruker@hotmail.com",
                    Postnummer="0364",
                    Poststed="Oslo",
                    Telefonnummer="93686771",
                    BekreftPassord="password"
                };

                bool actual = studentRepository.RegistrerBruker(NyBruker);
                Assert.AreEqual(true, actual);
            }
        }
        


    }
}
