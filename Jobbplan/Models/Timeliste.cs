using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jobbplan.Models
{
    public class Timeliste
    {
        [Key]
        public int BrukerId { get; set; }
        public int Timer { get; set; }
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public DateTime Start { get; set; }
        public DateTime Slutt { get; set; }
        public decimal Sum { get; set; }

    }

}