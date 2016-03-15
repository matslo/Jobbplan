﻿using System;
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
            var nyProsjektDeltakelse = new Prosjektdeltakelse()
            {
                BrukerId = userId,
                Medlemsdato = DateTime.Now,
                ProsjektId = nyProsjekt.ProsjektId
            };
            
            using (var db = new Dbkontekst())
            {
                try
                {
                    db.Prosjekter.Add(nyProsjekt);
                    db.Prosjektdeltakelser.Add(nyProsjektDeltakelse);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    return false;
                }

            }
        }
        public bool FjernAnsatt (int id, string brukernavn)
        {

        }
        public bool LeggTilBrukerRequest(ProsjektrequestSkjema pReq, string brukernavn)
        {
            int bId = BrukerId(brukernavn);
            int bIdTil = BrukerId(pReq.TilBruker);
            int pId = pReq.ProsjektId;

            if (bIdTil != 0)
            {
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
            return false;
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
        public List<ProsjektrequestMelding> VisRequesterForProsjekt(int id,string brukernavn)
        {
            Dbkontekst db = new Dbkontekst();

            var tilganger = SjekkTilgangProsjekt(brukernavn);
           
            List<Prosjektrequest> proReq = db.Prosjektrequester.ToList();
            var eventer = (from k in proReq
                           from s in tilganger
                           where k.ProsjektId == id && k.ProsjektId == s.Id
                           select new ProsjektrequestMelding
                           {
                               TilBruker = BrukerNavn(k.BrukerIdTil),
                               Tid = k.Sendt
                           }).ToList();
            return eventer;
        }
        public List<ProsjektVis> HentProsjekter (string Brukernavn)
        {
            int id = BrukerId(Brukernavn);
               List<ProsjektVis> pros = (from p in dbs.Prosjektdeltakelser 
                                         from s in dbs.Prosjekter
                                         where p.BrukerId==id && p.ProsjektId == s.ProsjektId
                                       select
                                           new ProsjektVis()
                                           {
                                               Id=p.ProsjektId,
                                               Arbeidsplass = s.Arbeidsplass                                              
                                           }).ToList();
                return pros;
            
        }
        public bool FjernBrukerFraProsjekt (string brukernavn, int PId)
        {
            if (ErAdmin(brukernavn, PId) == true || ErEier(brukernavn, PId) == true)
            {

            }

            int SjekkTilgang = (from x in dbs.Sjefer
                                where x.BrukerId == id && x.ProsjektId == PId
                                select x.BrukerId).SingleOrDefault();
            if (SjekkTilgang != 0)
            {
                return true;
            }
            return false;
        }
        public bool ErAdmin (string brukernavn, int PId)
        {
            int id = BrukerId(brukernavn);
          
            int SjekkTilgang = (from x in dbs.Sjefer
                          where x.BrukerId == id && x.ProsjektId == PId
                          select x.BrukerId).SingleOrDefault();
            if (SjekkTilgang != 0)
            {
                return true;
            }
            return false;
        }
        public bool ErEier (string brukernavn, int PId)
        {
            int id = BrukerId(brukernavn);

            int SjekkTilgang = (from x in dbs.Prosjekter
                                where x.EierId == id && x.ProsjektId == PId
                                select x.EierId).SingleOrDefault();
            if (SjekkTilgang != 0)
            {
                return true;
            }
            return false;
        }
        public int BrukerId (string brukernavn)
        {
            int userId = (from x in dbs.Brukere
                          where x.Email == brukernavn
                          select x.BrukerId).SingleOrDefault();
            return userId;
        }
        public List<ProsjektVis> SjekkTilgangProsjekt (string brukernavn)
        {
            int id = BrukerId(brukernavn);
            List<ProsjektVis> pros = (from p in dbs.Prosjektdeltakelser
                                      from s in dbs.Prosjekter
                                      where p.BrukerId == id && p.ProsjektId == s.ProsjektId
                                      select
                                          new ProsjektVis()
                                          {
                                              Id = p.ProsjektId,
                                          }).ToList();
            return pros;

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
        public bool SlettProsjekt (string Brukernavn, int id)
        { 
           Dbkontekst db = new Dbkontekst();
            if (ErEier(Brukernavn, id) == true)
            {
                try
                {
                    var SlettProsjekt = db.Prosjekter.FirstOrDefault(p => p.ProsjektId == id);
                    db.Prosjekter.Remove(SlettProsjekt);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    return false;
                }

            }
           return false;
           
        }   
        public string BrukerNavn (int id)
        {
            string brukernavn = (from x in dbs.Brukere
                          where x.BrukerId == id
                          select x.Email).SingleOrDefault();
            return brukernavn;
        }
        public bool EndreProsjekt(Prosjekt EndreProsjekt)
        {
            Dbkontekst db = new Dbkontekst();

            try
            {
                var NyEndreProsjekt = db.Prosjekter.FirstOrDefault(p => p.ProsjektId == EndreProsjekt.ProsjektId);
                NyEndreProsjekt.Arbeidsplass = EndreProsjekt.Arbeidsplass;
 

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