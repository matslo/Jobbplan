using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jobbplan.Models
{
    public class Bruker
    {
            public int BrukerId { get; set; }
            public string Etternavn { get; set; }
            public string Fornavn { get; set; }
            public string Email { get; set; }


        }

        public class LogInn
        {
            [Required(ErrorMessage = "Du må skrive brukernavn")]
            [Display(Name = "Brukernavn")]
            public string Brukernavn { get; set; }

            [Required(ErrorMessage = "Du må skrive inn passord!")]
            [DataType(DataType.Password)]
            [Display(Name = "Passord")]
            public string Passord { get; set; }

        }
    }
}