using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jobbplan.Models
{
    public class Prosjekt
    {
        [Key]
        public int ProsjektId { get; set; }
        public int EierId { get; set; }
        public string Arbeidsplass { get; set; }
        
        public virtual dbBruker dbBruker { get; set; }
        public virtual List<Prosjektdeltakelse> Prosjektdeltakelse { get; set; }
        public virtual List<Sjef> Sjef { get; set; }


    }
    public class ProsjektVis
    {
        public int Id { get; set; }
        public string Arbeidsplass { get; set; }
    }
}