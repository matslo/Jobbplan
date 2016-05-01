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
        private InterfaceDbTProsjekt mockProductRepositoryProsjekt;
        public VaktTest()
        { 
            // Lager mocks som vi kan gjøre spørringer mot
           
            List<Vaktkalender> vakter = new List<Vaktkalender>
                {
                   // new Vaktkalender {ProsjektId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"},
                   // new Vaktkalender {ProsjektId = 2, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring" },
                    //new Vaktkalender {ProsjektId = 3, Brukernavn = "", start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"}
                };
            List<VaktRequest> vaktReq = new List<VaktRequest>
            {
                new VaktRequest
                {
                    BrukerIdTil = 1,
                    BrukerIdFra = 2,
                    MeldingId = 1,
                    VaktId = 1,
                    ProsjektId = 1
                }
            };

            List<Vakt> vakterDB = new List<Vakt>
                {
                    new Vakt {VaktId = 1, ProsjektId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"},
                    new Vakt {ProsjektId = 2, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring" },
                    new Vakt {ProsjektId = 3, BrukerId = 1, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"},
                    new Vakt {ProsjektId = 3, BrukerId = 0, Ledig = true, start = Convert.ToDateTime("22.12.2012 16.43"),end =  Convert.ToDateTime("22.12.2012 16.43"),title = "Dagvakt",Beskrivelse = "Opplæring"}
            };
             List<Maler> maler = new List<Maler>
                {
                    new Maler {ProsjektId=1,startTid = "07.15", sluttTid = "14.15", Tittel = "dagvakt"}
                };

            List<dbBruker> bruker = new List<dbBruker>
                {
                    new dbBruker
                    {
                        BrukerId = 1, Email = "testing123@hotmail.com"
                    },
                    new dbBruker
                    {
                        BrukerId = 2, Email = "testing1@hotmail.com"
                    },
                    new dbBruker
                    {
                        BrukerId = 3, Email = "ikkeadmin@hotmail.com"
                    }
                };
            List<Prosjekt> prosjekter = new List<Prosjekt>
                {
                    new Prosjekt()
                    {
                        ProsjektId=1,EierId=1
                    },
                     new Prosjekt()
                    {
                        ProsjektId=2,EierId=2
                    }

                };
            List<Prosjektdeltakelse> deltakelser = new List<Prosjektdeltakelse>
            {
                new Prosjektdeltakelse
                {
                    ProsjektDeltakerId=1,
                    BrukerId = 1,
                    ProsjektId = 1,
                    Admin = false,
                    Medlemsdato = Convert.ToDateTime("22.12.2012 16.43")
                },
                 new Prosjektdeltakelse
                {
                    ProsjektDeltakerId=2,
                    BrukerId = 2,
                    ProsjektId = 1,
                    Admin = true,
                    Medlemsdato = Convert.ToDateTime("22.12.2012 16.43")
                },
                 new Prosjektdeltakelse
                {
                    ProsjektDeltakerId=3,
                    BrukerId = 3,
                    ProsjektId = 1,
                    Admin = false,
                    Medlemsdato = Convert.ToDateTime("22.12.2012 16.43")
                }
            };
            Mock<InterfaceDbTProsjekt> mockProductRepositoryProsjekt = new Mock<InterfaceDbTProsjekt>();

            Mock<InterfaceDbTVakt> mockProductRepository = new Mock<InterfaceDbTVakt>();

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


            mockProductRepository.Setup(mr => mr.visVaktRequester(It.IsAny<string>()))
           .Returns(
           (string u) =>
           {
               int id = this.mockProductRepositoryProsjekt.BrukerId(u);
              
               List<VaktRequestMelding> req = (from p in vaktReq
                                               from b in bruker
                                               from s in vakterDB
                                               where p.BrukerIdTil == id && p.BrukerIdFra == b.BrukerId && p.VaktId == s.VaktId
                                               select new VaktRequestMelding()
                                               {
                                                   MeldingId = p.MeldingId,
                                                   ProsjektId = p.ProsjektId,
                                                   FraBruker = b.Email,
                                                   Melding = " vil ta vakten: ",
                                                   title = p.Vakt.title,
                                                   start = p.Vakt.start,
                                                   end = p.Vakt.end,
                                                   VaktId = p.VaktId,
                                                   Prosjektnavn = p.Prosjekt.Arbeidsplass,
                                                   Tid = p.Sendt,
                                                   TilBruker = u
                                               }).ToList();

               return req;
           });

            mockProductRepository.Setup(mr => mr.requestOk(It.IsAny<int>()))
          .Returns(
          (int i) =>
          {
              try
              {
                  var Requester = vaktReq.FirstOrDefault(p => p.MeldingId == i);
                  var OppdaterVakt = vakterDB.FirstOrDefault(p => p.VaktId == Requester.VaktId);
                  OppdaterVakt.BrukerId = Requester.BrukerIdFra;
                  OppdaterVakt.Ledig = false;
                  OppdaterVakt.color = "#3A87AD";
                  vaktReq.Remove(Requester);
                 
                  return true;
              }
              catch (Exception feil)
              {
                  return false;
              }
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

            mockProductRepository.Setup(mr => mr.SlettVakt(It.IsAny<int>(), It.IsAny<string>()))
           .Returns((int i, string u) =>
           {
               var slettVakt = vakterDB.FirstOrDefault(p => p.VaktId == i);
               if (!this.mockProductRepositoryProsjekt.ErAdmin(u, slettVakt.ProsjektId) && !this.mockProductRepositoryProsjekt.ErEier(u, slettVakt.ProsjektId))
               {
                   return false;
               }
               try
               {
                   vakterDB.Remove(slettVakt);
                   return true;
               }
               catch (Exception feil)
               {
                   return false;
               }
           });

            mockProductRepository.Setup(mr => mr.LedigVakt(It.IsAny<Vaktskjema>()))
         .Returns((Vaktskjema i) =>
         {
             if (i.BrukerId == 0)
             {
                 return true;
             }
             return false;
         });

            mockProductRepository.Setup(mr => mr.taLedigVakt(It.IsAny<int>(), It.IsAny<string>()))
          .Returns((int i, string u) =>
          {
              try
              {
                  // finn personen i databasen
                  Vakt taVakt = vakterDB.FirstOrDefault(p => p.VaktId == i);

                  VaktRequest nyRequest = new VaktRequest();
                  // oppdater vakt fra databasen

                  var pId = taVakt.ProsjektId;
                  Prosjekt prosjekt = prosjekter.FirstOrDefault(p => p.ProsjektId == pId);

                  nyRequest.VaktId = taVakt.VaktId;
                  nyRequest.Sendt = DateTime.Now;
                  nyRequest.BrukerIdFra = this.mockProductRepositoryProsjekt.BrukerId(u);
                  nyRequest.BrukerIdTil = prosjekt.EierId;
                  nyRequest.ProsjektId = prosjekt.ProsjektId;
                  vaktReq.Add(nyRequest);
                  return true;
              }
              catch (Exception feil)
              {
                  return false;
              }
          });

            mockProductRepository.Setup(mr => mr.VakterProsjekt(It.IsAny<int>()))
          .Returns((int i) =>
          {
              var eventer = (from k in vakterDB
                             where k.ProsjektId == i
                             select k
                             ).ToList();
              return eventer;
          });
            

            mockProductRepository.Setup(mr => mr.RegistrerVakt(It.IsAny<Vaktskjema>(), It.IsAny<string>())).Returns(
                (Vaktskjema vakt, string u) =>
                {

                    if (!this.mockProductRepositoryProsjekt.ErAdmin(u, vakt.ProsjektId) && !this.mockProductRepositoryProsjekt.ErEier(u, vakt.ProsjektId))
                    {
                        return false;
                    }

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

            mockProductRepository.Setup(mr => mr.EndreVakt(It.IsAny<Vaktskjema>(), It.IsAny<string>())).Returns(
               (Vaktskjema vakt, string u) =>
               {
                   if (!this.mockProductRepositoryProsjekt.ErAdmin(u, vakt.ProsjektId) && !this.mockProductRepositoryProsjekt.ErEier(u, vakt.ProsjektId))
                   {
                       return false;
                   }
                   
                   var NyEndreVakt = vakterDB.FirstOrDefault(p => p.VaktId == vakt.Vaktid);
                   string start = vakt.start + " " + vakt.startTid;
                   string end;

                   IFormatProvider culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                   DateTime dt1 = DateTime.ParseExact(start, "dd.MM.yyyy H:mm", culture, System.Globalization.DateTimeStyles.AssumeLocal);
                   DateTime dt2;

                   if (vakt.end != "" && vakt.endDato == true)
                   {
                       end = vakt.end + " " + vakt.endTid;
                       dt2 = DateTime.ParseExact(end, "dd.MM.yyyy H:mm", culture, System.Globalization.DateTimeStyles.AssumeLocal);
                   }
                   else
                   {
                       end = vakt.start + " " + vakt.endTid;
                       dt2 = DateTime.ParseExact(end, "dd.MM.yyyy H:mm", culture, System.Globalization.DateTimeStyles.AssumeLocal);
                   }

                   int result = DateTime.Compare(dt1, dt2);
                   if (result > 0 || result == 0)
                   {
                       return false;
                   }
                   if (!this.mockProductRepositoryProsjekt.ErAdmin(u, NyEndreVakt.ProsjektId) && !this.mockProductRepositoryProsjekt.ErEier(u, NyEndreVakt.ProsjektId))
                   {
                       return false;
                   }
                   try
                   {
                       NyEndreVakt.Beskrivelse = vakt.Beskrivelse;
                       NyEndreVakt.BrukerId = vakt.BrukerId;
                       NyEndreVakt.start = dt1;
                       NyEndreVakt.end = dt2;
                       NyEndreVakt.title = vakt.title;

                       if (this.mockProductRepository.LedigVakt(vakt))
                       {
                           NyEndreVakt.Ledig = true;
                           NyEndreVakt.color = "#5CB85C";
                       }
                       else
                       {
                           NyEndreVakt.Ledig = false;
                           NyEndreVakt.color = "#3A87AD";
                       }

                       return true;
                   }
                   catch (Exception feil)
                   {
                       return false;
                   }
               });



            mockProductRepositoryProsjekt.Setup(mr => mr.ErEier(It.IsAny<string>(), It.IsAny<int>()))
             .Returns(
               (string u, int i) =>
               {
                   var bId = this.mockProductRepositoryProsjekt.BrukerId(u);
                   var TestOk = (from x in prosjekter
                                 where x.EierId == bId && x.ProsjektId == i
                                 select x.EierId).SingleOrDefault();
                   bool ok = false;
                   if (TestOk != 0)
                   {
                       ok = true;
                       return ok;
                   }
                   return ok;
               });

            mockProductRepositoryProsjekt.Setup(mr => mr.ErAdmin(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(
              (string u, int i) =>
              {
                  var okAdmin = false;
                  var bId = this.mockProductRepositoryProsjekt.BrukerId(u);
                  var TestOk = (from x in deltakelser
                                where x.ProsjektId == i && x.BrukerId == bId
                                select x.Admin).SingleOrDefault();

                  if (TestOk != false)
                  {
                      okAdmin = true;
                      return okAdmin;
                  }
                  return okAdmin;
              });
            mockProductRepositoryProsjekt.Setup(mr => mr.BrukerId(It.IsAny<string>()))
              .Returns(
                (string u) =>
                {
                    int userId = (from x in bruker
                                  where x.Email == u
                                  select x.BrukerId).SingleOrDefault();
                    return userId;
                });
            
            // Complete the setup of our Mock Product Repository
            this.mockProductRepositoryProsjekt = mockProductRepositoryProsjekt.Object;
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
            bool ok = this.mockProductRepository.RegistrerVakt(vakt, "testing123@hotmail.com");

            Assert.IsFalse(ok);


        }
        [TestMethod]
        public void Registrer_Vakt_OK()
        {
            
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
            bool ok = this.mockProductRepository.RegistrerVakt(vakt, "testing123@hotmail.com");

            Assert.IsTrue(ok);
        }
        [TestMethod]
        public void Registrer_Vakt_False_Ikke_admin()
        {

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
            bool ok = this.mockProductRepository.RegistrerVakt(vakt, "ikkeadmin@hotmail.com");

            Assert.IsFalse(ok);
        }
        [TestMethod]
        public void Registrer_Vakt_OK_admin()
        {

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
            bool ok = this.mockProductRepository.RegistrerVakt(vakt, "testing1@hotmail.com");

            Assert.IsTrue(ok);
        }
        [TestMethod]
        public void Ta_Ledig_Vakt_OK()
        {

            bool ok = this.mockProductRepository.taLedigVakt(1, "testing123@hotmail.com");

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
        [TestMethod]
        public void Endre_Vakt_OK()
        {

            Vaktskjema vakt = new Vaktskjema()
            {
                Vaktid=1,
                start = "22.12.2012",
                startTid = "16.43",
                endTid = "18.43",
                title = "Dagvakt",
                Beskrivelse = "Opplæring",
                BrukerId = 1,
                ProsjektId = 1
            };
            bool ok = this.mockProductRepository.EndreVakt(vakt, "testing123@hotmail.com");

            Assert.IsTrue(ok);
        }
        [TestMethod]
        public void Endre_Vakt_Ikke_Admin()
        {

            Vaktskjema vakt = new Vaktskjema()
            {
                Vaktid = 1,
                start = "22.12.2012",
                startTid = "16.43",
                endTid = "18.43",
                title = "Dagvakt",
                Beskrivelse = "Opplæring",
                BrukerId = 1,
                ProsjektId = 1
            };
            bool ok = this.mockProductRepository.EndreVakt(vakt, "ikkeadmin@hotmail.com");

            Assert.IsFalse(ok);
        }
        [TestMethod]
        public void Endre_vakt_end_before_start()
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
            bool ok = this.mockProductRepository.EndreVakt(vakt, "testing123@hotmail.com");

            Assert.IsFalse(ok);


        }
        [TestMethod]
        public void SlettVakt_OK()
        {
            bool ok = this.mockProductRepository.SlettVakt(1, "testing123@hotmail.com");
            Assert.IsTrue(ok);   
        }
        [TestMethod]
        public void SlettVakt_Ikke_admin()
        {
            bool ok = this.mockProductRepository.SlettVakt(1, "ikkeadmin@hotmail.com");
            Assert.IsFalse(ok);
        }

        [TestMethod]
        public void Vis_Vaktrequester_ok()
        {
            // Try finding a product by id
            List<VaktRequestMelding> testProduct = this.mockProductRepository.visVaktRequester("testing123@hotmail.com");
            for (var i = 0; i < testProduct.Count; i++)
            {
                Assert.AreEqual("testing123@hotmail.com", testProduct[i].TilBruker);

            }
            Assert.AreNotEqual(0, testProduct.Count); // Test if null
            Assert.IsInstanceOfType(testProduct, typeof(List<VaktRequestMelding>)); // Test type
        }

        [TestMethod]
        public void Request_ok()
        {
            bool ok = this.mockProductRepository.requestOk(1);
            Assert.IsTrue(ok);
        }

    }
}
