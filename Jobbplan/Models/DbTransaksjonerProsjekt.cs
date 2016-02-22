using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Jobbplan.Models
{
    public class DbTransaksjonerProsjekt
    {
        Dbkontekst dbs = new Dbkontekst();
        public bool RegistrerProsjekt(Prosjekt innProsjekt, string brukernavn)
        {
            Dbkontekst dbs = new Dbkontekst();

            int userId = (from x in dbs.Brukere
                          where x.Email == brukernavn
                          select x.BrukerId).SingleOrDefault();


            var nyProsjekt = new Prosjekt()
            {
                
                Arbeidsplass = innProsjekt.Arbeidsplass,
                EierId = userId
                
            };
            
            using (var db = new Dbkontekst())
            {
                try
                {

                    db.Prosjekter.Add(nyProsjekt);
                    db.SaveChanges();
                    return true;

                }
                catch (Exception feil)
                {
                    return false;
                }

            }
        }
        public bool LeggTilBrukerRequest(ProsjektrequestSkjema pReq, string brukernavn)
        {
            int bId = BrukerId(brukernavn);
            int bIdTil = BrukerId(pReq.TilBruker);
            int pId = pReq.ProsjektId;

            var nyRequest = new Prosjektrequest()
            {
                BrukerIdFra = bId,
                BrukerIdTil = bIdTil,
                ProsjektId = pId,
                Akseptert = false,
                Sendt = DateTime.Now
            };
            using (var db = new Dbkontekst())
            {
                try
                {
                    db.Prosjektrequester.Add(nyRequest);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    return false;
                }
            }
        }
        public bool RegistrerProsjektApi(Prosjekt innProsjekt, string brukernavn)
        {
            Dbkontekst dbs = new Dbkontekst();

            int userId = (from x in dbs.Brukere
                          where x.Email == brukernavn
                          select x.BrukerId).SingleOrDefault();


            var nyProsjekt = new Prosjekt()
            {

                Arbeidsplass = innProsjekt.Arbeidsplass,
                EierId = userId

            };

            using (var db = new Dbkontekst())
            {
                try
                {

                    db.Prosjekter.Add(nyProsjekt);
                    db.SaveChanges();
                    return true;

                }
                catch (Exception feil)
                {
                    return false;
                }

            }
        }
        public bool RegistrerProsjektdeltakelse(ProsjektrequestMelding pid,string brukernavn)
        {
            Dbkontekst dbs = new Dbkontekst();
            int id = BrukerId(brukernavn);
            IEnumerable<ProsjektDeltakelseOverforing> prosjektReq = from prosj in dbs.Prosjektrequester
                                                                    from b in dbs.Brukere
                                                                    from s in dbs.Prosjekter
                                                                    where prosj.BrukerIdTil == id && prosj.BrukerIdFra == b.BrukerId && prosj.ProsjektId == pid.ProsjektId
                                                                    select new ProsjektDeltakelseOverforing()
                                                                    {
                                                                        BrukerId = prosj.BrukerIdTil,
                                                                        ProsjektId = prosj.ProsjektId
                                                                        
                                                                    };
                                                                            

            var prosjektD = new Prosjektdeltakelse();
            foreach(var a in prosjektReq)
            {
                prosjektD.BrukerId = a.BrukerId;
                prosjektD.ProsjektId = a.ProsjektId;
                prosjektD.Medlemsdato = DateTime.Now;
            }
            
         
            using (var db = new Dbkontekst())
            {
                try
                {
                    
                    db.Prosjektdeltakelser.Add(prosjektD);
                    db.SaveChanges();
                    SlettRequest(prosjektD.ProsjektId, brukernavn);
                    return true;

                }
                catch (Exception feil)
                {
                    return false;
                }

            }

        }
        public List<ProsjektrequestMelding> VisRequester (string Brukernavn)
        {
            int id = BrukerId(Brukernavn);

            List<ProsjektrequestMelding> pros = (from p in dbs.Prosjektrequester
                                          from b in dbs.Brukere
                                          from s in dbs.Prosjekter
                                      where p.BrukerIdTil == id && p.BrukerIdFra == b.BrukerId && p.ProsjektId == s.ProsjektId
                                      select
                                          new ProsjektrequestMelding()
                                          {
                                              ProsjektId = p.ProsjektId,
                                              FraBruker = b.Email,
                                              Melding = " har invitert deg til å bli medlem av: ",
                                              Prosjektnavn = p.Prosjekt.Arbeidsplass,
                                              Tid = p.Sendt,
                                              TilBruker = Brukernavn
                                          }).ToList();
            return pros;
        }
        public List<ProsjektVis> HentProsjekter (string Brukernavn)
        {
            int id = BrukerId(Brukernavn);
               List<ProsjektVis> pros = (from p in dbs.Prosjekter 
                                       where p.EierId == id
                                       select
                                           new ProsjektVis()
                                           {
                                               Id=p.ProsjektId,
                                               Arbeidsplass = p.Arbeidsplass                                              
                                           }).ToList();
                return pros;
            
        }
        public int BrukerId (string brukernavn)
        {
            int userId = (from x in dbs.Brukere
                          where x.Email == brukernavn
                          select x.BrukerId).SingleOrDefault();
            return userId;
        }

        public bool SlettRequest(int id, string brukernavn)
        {
            Dbkontekst db = new Dbkontekst();
            int bid = BrukerId(brukernavn);
            try
            {
                var SlettRequest = db.Prosjektrequester.FirstOrDefault(p => p.BrukerIdTil == bid && p.ProsjektId == id);
                db.Prosjektrequester.Remove(SlettRequest);
                db.SaveChanges();
                return true;
            }
            catch (Exception feil)
            {
                return false;
            }
        }
       
        public string BrukerNavn (int id)
        {
            string brukernavn = (from x in dbs.Brukere
                          where x.BrukerId == id
                          select x.Email).SingleOrDefault();
            return brukernavn;
        }
    }
}