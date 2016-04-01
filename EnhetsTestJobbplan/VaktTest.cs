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

namespace EnhetsTestJobbplan
{
   
    [TestClass]
    public class VaktTest
    {

        [TestMethod]
        public void SettInnVaktOk()
        {
            /* using (TransactionScope scope = new TransactionScope())
             {
                 InterfaceDbTVakt studentRepository = new DbTransaksjonerVakt();
                 Vakt student = new Vakt()
                 {
                     start = 22.12.12T1215.44,
                     end = "",
                     title = "Dagvakt",
                     Beskrivelse = "Opplæring",
                     BrukerId = 1,
                     ProsjektId = 1
                 };
                 int id = studentRepository.Save(student);
                 Assert.AreNotEqual(0, id);
             }*/
        }
    }
}
