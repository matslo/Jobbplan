using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jobbplan.Models
{
    public class DbTransaksjonerProsjekt
    {
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
       
    }
}