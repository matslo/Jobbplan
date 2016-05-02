using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jobbplan.Model;

namespace Jobbplan.DAL
{
    public class DbTransaksjonerBruker : InterfaceDbTBruker
    {
        public bool RegistrerBruker(Registrer innBruker)
        {
            if (EmailDb(innBruker))
            {
                return false;
            }

            byte[] passordDb = lagHash(innBruker.BekreftPassord);

            var nyBruker = new dbBruker()
            {
                Passord = passordDb,
                Fornavn = innBruker.Fornavn,
                Etternavn = innBruker.Etternavn,
                Email = innBruker.Email,
                Telefonnummer = innBruker.Telefonnummer
            };

            using (var db = new Dbkontekst())
            {
                try
                {
                    db.Brukere.Add(nyBruker);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    return false;
                }
            }
        }
        public bool GiBrukerAdminTilgang(Sjef innBruker, string brukernavn)
        {
            
            Dbkontekst dbs = new Dbkontekst();
            DbTransaksjonerProsjekt DbTp = new DbTransaksjonerProsjekt();

            if (!DbTp.ErEier(brukernavn,innBruker.ProsjektId))
            {
                return false;
            }
            var AdminTilgang = dbs.Prosjektdeltakelser.FirstOrDefault(p => p.ProsjektId == innBruker.ProsjektId && p.BrukerId == innBruker.BrukerId);
            if (AdminTilgang == null)
            {
                return false;
            }
            try
            {
                AdminTilgang.Admin = true;
                dbs.SaveChanges();
                return true;
            }
            catch (Exception feil)
            {
                return false;
            }
        }
        public bool FjernAdminTilgang(Sjef innBruker, string brukernavn)
        {
            Dbkontekst dbs = new Dbkontekst();
            DbTransaksjonerProsjekt DbTp = new DbTransaksjonerProsjekt();
            if (!DbTp.ErEier(brukernavn, innBruker.ProsjektId))
            {
                return false;
            }
            try
            {
                var AdminTilgang = dbs.Prosjektdeltakelser.FirstOrDefault(p => p.ProsjektId == innBruker.ProsjektId && p.BrukerId == innBruker.BrukerId);
                AdminTilgang.Admin = false;
                dbs.SaveChanges();
                return true;
            }
            catch (Exception feil)
            {
                return false;
            }
        }
        public List<BrukerListe> HentBrukere(int ProsjektId, string brukernavn)
        {
            Dbkontekst dbs = new Dbkontekst();
            DbTransaksjonerProsjekt dbtp = new DbTransaksjonerProsjekt();
     
            if (dbtp.ErAdmin(brukernavn, ProsjektId) == true || dbtp.ErEier(brukernavn, ProsjektId) == true)
            {
                List<BrukerListe> pros = (from p in dbs.Prosjektdeltakelser
                                          from s in dbs.Brukere
                                      
                                          where p.ProsjektId == ProsjektId && p.BrukerId == s.BrukerId
                                          select
                                              new BrukerListe()
                                              {
                                                  ProsjektDeltakerId = p.ProsjektDeltakerId,
                                                  Navn = s.Fornavn + " " + s.Etternavn,
                                                  BrukerId = p.BrukerId,
                                                  Brukernavn = s.Email,
                                                  Admin = p.Admin
                                                  
                                              }).ToList();
                return pros;
            }
            else
            {
                List<BrukerListe> prosFeil = null;
                return prosFeil;
            }

        }
        public bool BrukerIdb(LogInn innBruker)
        {   //Sjekker om bruker er i db
            using (var db = new Dbkontekst())
            {
                byte[] passordDb = lagHash(innBruker.Passord);
                dbBruker funnetBruker = db.Brukere.FirstOrDefault
                    (b => b.Passord == passordDb && b.Email == innBruker.Brukernavn);
                if (funnetBruker == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool EmailDb(Registrer innBruker)
        {   //Sjekker om bruker er i db
            using (var db = new Dbkontekst())
            {
                dbBruker funnetBruker = db.Brukere.FirstOrDefault
                    (b => b.Email == innBruker.Email);
                if (funnetBruker == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public int AntallMeldinger(string brukernavn)
        {
            var db = new Dbkontekst();
            var dbTP = new DbTransaksjonerProsjekt();
            var dbTV = new DbTransaksjonerVakt();
            var ProsjektReq = dbTP.VisRequester(brukernavn);
            var VaktReq = dbTV.visVaktRequester(brukernavn);
            int AntallMeldinger = 0;
            foreach (var a in ProsjektReq)
            {
                AntallMeldinger++;
            }
            foreach (var a in VaktReq)
            {
                AntallMeldinger++;
            }
            return AntallMeldinger;
        }
        private static byte[] lagHash(string innPassord)
        {
            //Hash passord
            byte[] innData, utData;
            var algoritme = System.Security.Cryptography.SHA256.Create(); 
            if (innPassord != null)
            {
                innData = System.Text.Encoding.ASCII.GetBytes(innPassord);
                utData = algoritme.ComputeHash(innData);
                return utData;

            }
            return null;
        }
        public List<Timeliste> HentVakter(string Brukernavn, int Pid)
        {
            var db = new Dbkontekst();
            var dbtb = new DbTransaksjonerProsjekt();

            int id = dbtb.BrukerId(Brukernavn);
            
            List<Timeliste> pros = (from p in db.Vakter
                                    where p.BrukerId == id && p.ProsjektId == Pid
                                    select
                                        new Timeliste()
                                        {
                                            PeriodeStart = p.start,
                                            PeriodeSlutt = p.end
                                        }).ToList();
           
            return pros;
        }
        public List<Profil> HentBruker(string Brukernavn)
        {
            var db = new Dbkontekst();
            var dbtb = new DbTransaksjonerProsjekt();

            int id = dbtb.BrukerId(Brukernavn);
            List<Profil> pros = (from p in db.Brukere
                                   where p.BrukerId == id
                                   select
                                      new Profil()
                                      {
                                          id = p.BrukerId,
                                          Fornavn = p.Fornavn,
                                          Etternavn = p.Etternavn,
                                          Email = p.Email                                                                         
                                      }).ToList();
            return pros;
        }
        public bool EndreBrukerInfo(Profil EndreBrukerInfo, string brukernavn)
        {
            var dbtp = new DbTransaksjonerProsjekt();

            Dbkontekst db = new Dbkontekst();
            int id = dbtp.BrukerId(brukernavn);
            try
            {
          
                var nyEndreBrukerInfo = db.Brukere.FirstOrDefault(p => p.BrukerId == id);


                if (EndreBrukerInfo.Fornavn != "")
                {
                    nyEndreBrukerInfo.Fornavn = EndreBrukerInfo.Fornavn;
                }
                if (EndreBrukerInfo.Etternavn != "")
                {
                    nyEndreBrukerInfo.Etternavn = EndreBrukerInfo.Etternavn;
                }
              
              
                db.SaveChanges();
                return true;
            }
            catch (Exception feil)
            {
                return false;
            }
        }

    }
}