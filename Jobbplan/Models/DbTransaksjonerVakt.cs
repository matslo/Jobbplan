using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jobbplan.Models
{
    public class DbTransaksjonerVakt : InterfaceDbTVakt
    {
        public bool RegistrerVakt (Vaktskjema innVakt)
        {
            DateTime d1 = Convert.ToDateTime(innVakt.start);
            DateTime d2 = Convert.ToDateTime(innVakt.end);
            int result = DateTime.Compare(d1, d2);
            if (result > 0 || result==0)
            {
                return false;
            }
            var nyVakt = new Vakt()
            {
               start = Convert.ToDateTime(innVakt.start),
               end = Convert.ToDateTime(innVakt.end),
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
        public List<Vaktkalender> hentAlleVakter(int id,string brukernavn)
        {
            
            Dbkontekst db = new Dbkontekst();
            var dbtB = new DbTransaksjonerProsjekt();
            var liste = dbtB.SjekkTilgangProsjekt(brukernavn);

            

            List<Vakt> vakter = db.Vakter.ToList();
            var eventer = (from k in vakter
                           from s in liste
                           where k.ProsjektId==id && k.ProsjektId==s.Id
                           select new Vaktkalender
                           {
                               start = k.start.ToString("s"),
                               end = k.end.ToString("s"),
                               Brukernavn = dbtB.BrukerNavn(k.BrukerId),
                               title = k.title +": "+ dbtB.BrukerNavn(k.BrukerId),
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
                                            select
                                                new VaktRequestMelding()
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
    }


}