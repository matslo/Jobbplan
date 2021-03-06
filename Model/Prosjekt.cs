﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jobbplan.Model
{
    public class Prosjekt
    {
        [Key]
        public int ProsjektId { get; set; }
        
        public int EierId { get; set; }
        [Required]
        public string Arbeidsplass { get; set; }
        
        public virtual dbBruker dbBruker { get; set; }
        public virtual List<Prosjektdeltakelse> Prosjektdeltakelse { get; set; }
        public virtual List<Maler> Maler { get; set; }



    }
    public class ProsjektVis
    {
        public int Id { get; set; }
        public string Arbeidsplass { get; set; }
    }
}