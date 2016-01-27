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
        public decimal Timer { get; set; }
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public DateTime PeriodeStart { get; set; }
        public DateTime PeriodeSlutt { get; set; }
        public decimal Sum { get; set; }

    }

}