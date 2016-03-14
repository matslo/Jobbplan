using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Jobbplan.Models
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
            Adresse = innBruker.Adresse,
            Postnr = innBruker.Postnummer,
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
        public bool GiBrukerAdminTilgang(Sjef innBruker)
        {
            
            var nySjef = new Sjef()
            {
                BrukerId = innBruker.BrukerId,
                ProsjektId = innBruker.ProsjektId
            };

            using (var db = new Dbkontekst())
            {
                try
                {
                    db.Sjefer.Add(nySjef);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    return false;
                }
            }
        }
        public List<BrukerListe> HentBrukere (int ProsjektId, string brukernavn)
        {
            Dbkontekst dbs = new Dbkontekst();
            DbTransaksjonerProsjekt dbtp = new DbTransaksjonerProsjekt();

            int ProsjektidTilgang = dbtp.BrukerId(brukernavn);
            Prosjekt funnetTilgang = dbs.Prosjekter.FirstOrDefault
                   (b => b.EierId == ProsjektidTilgang && b.ProsjektId == ProsjektId);

            if (funnetTilgang != null)
            { 
            List<BrukerListe> pros = (from p in dbs.Prosjektdeltakelser
                                      from s in dbs.Brukere
                                      where p.ProsjektId == ProsjektId && p.BrukerId == s.BrukerId 
                                      select
                                          new BrukerListe()
                                          {
                                              Navn = s.Fornavn+" "+s.Etternavn,
                                              BrukerId = p.BrukerId,
                                              Brukernavn = s.Email
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
    }
}