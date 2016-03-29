using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace Jobbplan.Models
{
    public class DbTransaksjonerVakt : InterfaceDbTVakt
    {
        public bool RegistrerVakt (Vaktskjema innVakt, string brukernavn)
        {
            var dbtp = new DbTransaksjonerProsjekt();
            if (!dbtp.ErAdmin(brukernavn, innVakt.ProsjektId) && !dbtp.ErEier(brukernavn, innVakt.ProsjektId))
            {
                return false;
            }
            

            // Alternate choice: If the string has been input by an end user, you might  
            // want to format it according to the current culture: 
             IFormatProvider culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            DateTime dt1 = DateTime.ParseExact(innVakt.start, "dd.MM.yyyy HH.mm", culture, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime dt2 = DateTime.ParseExact(innVakt.end, "dd.MM.yyyy HH.mm", culture, System.Globalization.DateTimeStyles.AssumeLocal);

         //  DateTime d1 = Convert.ToDateTime(innVakt.start);
                //Convert.ToDateTime(innVakt.start);
          //  DateTime d2 = Convert.ToDateTime(innVakt.end);
            //Convert.ToDateTime(innVakt.end);
            int result = DateTime.Compare(dt1, dt2);
            if (result > 0 || result==0)
            {
                return false;
            }
            var nyVakt = new Vakt()
            {
               start = dt1,
               end = dt2,
               title = innVakt.title,
               Beskrivelse = innVakt.Beskrivelse,
               BrukerId = innVakt.BrukerId,
               ProsjektId = innVakt.ProsjektId
            };
            if (LedigVakt(innVakt))
            {
                nyVakt.Ledig = true;
                nyVakt.color = "#5CB85C";
            }

            using (var db = new Dbkontekst())
            {
                try
                {
                    db.Vakter.Add(nyVakt);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    return false;
                }

            }

        }
        public bool LedigVakt (Vaktskjema innVakt)
        {
            if(innVakt.BrukerId==0)
            {
                return true;
            }
            return false;
        }    
        public List<Vakt> VakterProsjekt (int id)
        {
            Dbkontekst db = new Dbkontekst();
            var eventer = (from k in db.Vakter
                           where k.ProsjektId == id
                           select k
                           ).ToList();
            return eventer;
        }
        public List<Vaktkalender> hentAlleVakter(int id,string brukernavn)
        {
            
            Dbkontekst db = new Dbkontekst();
            var dbtB = new DbTransaksjonerProsjekt();
            var liste = dbtB.SjekkTilgangProsjekt(brukernavn);
            List<Vakt> vakter = VakterProsjekt(id);

            var eventer = (from k in vakter
                           from s in liste
                           where k.ProsjektId==id && k.ProsjektId==s.Id
                           select new Vaktkalender
                           {
                               start = k.start.ToString("s"),
                               end = k.end.ToString("s"),
                               Brukernavn = dbtB.BrukerNavn(k.BrukerId),
                               title = k.title +": "+ dbtB.FultNavn(k.BrukerId),
                               color = k.color,
                               VaktId = k.VaktId
                           }).ToList();
            return eventer;
        }
        public List<Vaktkalender> hentAlleVakterBruker(int id, string brukernavn)
        {

            Dbkontekst db = new Dbkontekst();
            var dbtB = new DbTransaksjonerProsjekt();
            var liste = dbtB.SjekkTilgangProsjekt(brukernavn);
            int brukerId = dbtB.BrukerId(brukernavn);


            List<Vakt> vakter = db.Vakter.ToList();
            var eventer = (from k in vakter
                           from s in liste
                           where k.ProsjektId == id && k.ProsjektId == s.Id && k.BrukerId == brukerId
                           select new Vaktkalender
                           {
                               start = k.start.ToString("s"),
                               end = k.end.ToString("s"),
                               Brukernavn = dbtB.BrukerNavn(k.BrukerId),
                               title = k.title,
                               color = k.color,
                               VaktId = k.VaktId
                           }).ToList();
            return eventer;
        }
        public List<Vaktkalender> hentAlleLedigeVakter(int id, string brukernavn)
        {

            Dbkontekst db = new Dbkontekst();
            var dbtB = new DbTransaksjonerProsjekt();
            var liste = dbtB.SjekkTilgangProsjekt(brukernavn);
            int brukerId = dbtB.BrukerId(brukernavn);


            List<Vakt> vakter = db.Vakter.ToList();
            var eventer = (from k in vakter
                           from s in liste
                           where k.ProsjektId == id && k.ProsjektId == s.Id && k.BrukerId == 0
                           select new Vaktkalender
                           {
                               start = k.start.ToString("s"),
                               end = k.end.ToString("s"),
                               Brukernavn = dbtB.BrukerNavn(k.BrukerId),
                               title = k.title,
                               color = k.color,
                               VaktId = k.VaktId
                           }).ToList();
            return eventer;
        }
        public bool taLedigVakt(int id, string brukernavn)
        {
            var dbt = new DbTransaksjonerProsjekt();
            var db = new Dbkontekst();
            try
            {
                // finn personen i databasen
                Vakt taVakt = db.Vakter.FirstOrDefault(p => p.VaktId == id);

                VaktRequest nyRequest = new VaktRequest();
                // oppdater vakt fra databasen

                var pId = taVakt.ProsjektId;
                Prosjekt prosjekt = db.Prosjekter.FirstOrDefault(p => p.ProsjektId == pId);

                nyRequest.VaktId = taVakt.VaktId;
                nyRequest.Sendt = DateTime.Now;
                nyRequest.BrukerIdFra = dbt.BrukerId(brukernavn);
                nyRequest.BrukerIdTil = prosjekt.EierId;
                nyRequest.ProsjektId = prosjekt.ProsjektId;
                db.Vaktrequester.Add(nyRequest);
                
                db.SaveChanges();
                return true;
            }
            catch (Exception feil)
            {
                return false;
            }
        }
        public bool EndreVakt(Vaktskjema EndreVakt)
        {
            Dbkontekst db = new Dbkontekst();

            try
            {
                var NyEndreVakt = db.Vakter.FirstOrDefault(p => p.VaktId == EndreVakt.Vaktid);
                NyEndreVakt.Beskrivelse = EndreVakt.Beskrivelse;
                NyEndreVakt.BrukerId = EndreVakt.BrukerId;
                NyEndreVakt.start = Convert.ToDateTime(EndreVakt.start);
                NyEndreVakt.end = Convert.ToDateTime(EndreVakt.end);
                NyEndreVakt.title = EndreVakt.title;             

               if (LedigVakt(EndreVakt))
                {
                    NyEndreVakt.Ledig = true;
                    NyEndreVakt.color = "#5CB85C";
                }
               else
                {
                    NyEndreVakt.Ledig = false;
                    NyEndreVakt.color = "#3A87AD";
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception feil)
            {
                return false;
            }
        }
        public List<VaktRequestMelding> visVaktRequester(string Brukernavn)
        {
            var dbt = new DbTransaksjonerProsjekt();
            int id = dbt.BrukerId(Brukernavn);
            var dbs = new Dbkontekst();
          
            List<VaktRequestMelding> req = (from p in dbs.Vaktrequester
                                            from b in dbs.Brukere
                                            from s in dbs.Vakter
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
                                                    TilBruker = Brukernavn
                                                }).ToList();
            
            return req;
        }
        public bool requestOk(int id)
        {
            Dbkontekst db = new Dbkontekst();
            try
            {
                var Requester = db.Vaktrequester.FirstOrDefault(p => p.MeldingId == id);
                var OppdaterVakt = db.Vakter.FirstOrDefault(p => p.VaktId == Requester.VaktId);
                OppdaterVakt.BrukerId = Requester.BrukerIdFra;
                OppdaterVakt.Ledig = false;
                OppdaterVakt.color = "#3A87AD";
                db.Vaktrequester.Remove(Requester);
                db.SaveChanges();
                return true;
            }
            catch (Exception feil)
            {
                return false;
            }
        }
        public bool SlettVaktRequest (int Meldingid)
        {
            Dbkontekst db = new Dbkontekst();
            try
            {
                var SlettRequest = db.Vaktrequester.FirstOrDefault(p => p.MeldingId == Meldingid);
                db.Vaktrequester.Remove(SlettRequest);
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