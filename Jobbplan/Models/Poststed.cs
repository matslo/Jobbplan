using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jobbplan.Models
{
    public class Poststed
    {
        [Key]
        public string Postnummer { get; set; }
        public string PostSted { get; set; }

        public virtual List<dbBruker> bruker { get; set; }


    }
}