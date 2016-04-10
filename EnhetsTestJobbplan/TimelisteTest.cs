using System;
using Jobbplan.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace EnhetsTestJobbplan
{
    [TestClass]
    public class TimelisteTest
    {
        [TestMethod]
        public void Index_ViewName()
        {
            //Arrange
            var controller = new TimelisteController();
            //Act
            var result = controller.Index() as ViewResult;
            //Assert
            Assert.AreEqual(result.ViewName, "");
        }
    }
}
}
