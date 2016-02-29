﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jobbplan.Models
{
    public class DbTransaksjonerVakt : InterfaceDbTVakt
    {
        public bool RegistrerVakt (Vaktskjema innVakt)
        {
            
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
                               title = k.title,
                               color = k.color,
                               VaktId = k.VaktId
                           }).ToList();
            return eventer;
        }
        public void taLedigVakt(int id, string brukernavn)
        {
            var dbt = new DbTransaksjonerProsjekt();
            var db = new Dbkontekst();
            try
            {
                // finn personen i databasen
                Vakt taVakt = db.Vakter.FirstOrDefault(p => p.VaktId == id);

                // oppdater vakt fra databasen
                taVakt.BrukerId = dbt.BrukerId(brukernavn);
                taVakt.Ledig = false;
                taVakt.color = "#3A87AD";

                db.SaveChanges();
            }
            catch (Exception feil)
            {

            }
        }

    }

}