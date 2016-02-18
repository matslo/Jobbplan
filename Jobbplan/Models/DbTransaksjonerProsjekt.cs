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
        public bool RegistrerProsjektdeltakelse(string brukernavn)
        {
            Dbkontekst dbs = new Dbkontekst();
            int id = BrukerId(brukernavn);
            var prosjektDeltakelse = (from prosj in dbs.Prosjektrequester
                                      from b in dbs.Brukere
                                      from s in dbs.Prosjekter
                                      where prosj.BrukerIdTil == id && prosj.BrukerIdFra == b.BrukerId && prosj.ProsjektId == s.ProsjektId
                                      select new ProsjektDeltakelseOverforing
                                      {
                                          BrukerId = id,
                                          ProsjektId = prosj.ProsjektId,
                                          Start = DateTime.Now,

                                      }).ToList();

            var prosjektD = new Prosjektdeltakelse();
             
         
            using (var db = new Dbkontekst())
            {
                try
                {
                    
                 //   db.Prosjektdeltakelser.Add(prosjektDeltakelse);
                    db.SaveChanges();
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
       
        public string BrukerNavn (int id)
        {
            string brukernavn = (from x in dbs.Brukere
                          where x.BrukerId == id
                          select x.Email).SingleOrDefault();
            return brukernavn;
        }
    }
}