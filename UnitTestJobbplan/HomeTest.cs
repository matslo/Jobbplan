using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using Jobbplan;
using Jobbplan.Controllers;
using System.Web.Mvc;

namespace UnitTestJobbplan  
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
    }
}
