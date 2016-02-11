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
               ProsjektId = innVakt.ProsjektId

            };
            if (LedigVakt(innVakt))
            {
                nyVakt.Ledig = true;
                nyVakt.color = "#E64759";
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
       
        public List<Vaktkalender> hentAlleVakter(int id)
        {
            Dbkontekst db = new Dbkontekst();
            List<Vakt> vakter = db.Vakter.ToList();

            var eventer = (from k in vakter
                           where k.ProsjektId==id
                           select new Vaktkalender
                           {
                               
                               start = k.start.ToString("s"),
                               end = k.end.ToString("s"),
                               title = k.title,
                               color = k.color
                           }).ToList();
            return eventer;
        }
    }

}