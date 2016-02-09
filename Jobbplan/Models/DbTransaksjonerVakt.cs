using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jobbplan.Models
{
    public class DbTransaksjonerVakt : InterfaceDbTVakt
    {
        public bool RegistrerVakt (Vakt innVakt)
        {
            
            var nyVakt = new Vakt()
            {
               start = innVakt.start,
               end = innVakt.end,
               title = innVakt.title,
               Beskrivelse = innVakt.Beskrivelse
               //ProsjektId = Session ----Kommer

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
        public bool LedigVakt (Vakt innVakt)
        {
            if(innVakt.BrukerId==0)
            {
                return true;
            }
            return false;
        }
    }

}