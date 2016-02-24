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
            if (innBruker.Fornavn == "")
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
        public List<BrukerListe> HentBrukere (int ProsjektId)
        {
            Dbkontekst dbs = new Dbkontekst();
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