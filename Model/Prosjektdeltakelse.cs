﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Jobbplan.Model
{
    public class Prosjektdeltakelse
    {
        [Key]
        public int ProsjektDeltakerId { get; set; }
        [Required]
        public int BrukerId { get; set; }
        [Required]
        public int ProsjektId { get; set; }
        [Required]
        public DateTime Medlemsdato { get; set; }
        [Required]
        public bool Admin { get; set; }

        public virtual Prosjekt Prosjekt { get; set; }
        public virtual dbBruker dbBruker { get; set; }
        //public virtual List<Sjef> Sjef { get; set; }

    }
    public class Prosjektrequest
    {
        [Key]
        public int MeldingId { get; set; }
        [Required]
        public int BrukerIdFra { get; set; }
        [Required]
        public int BrukerIdTil { get; set; }
        [Required]
        public int ProsjektId { get; set; }
        [Required]
        public bool Akseptert { get; set; }
        [Required]
        public DateTime Sendt { get; set; }

        public virtual Prosjekt Prosjekt { get; set; }
        public virtual dbBruker dbBruker {get;set;}

    }
    public class ProsjektrequestSkjema
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        ErrorMessage = "Email er ikke gyldig")]
        [Display(Name = "Email")]
        public string TilBruker { get; set; }

        public int ProsjektId { get; set; }
    }
    public class ProsjektDeltakelseOverforing
    {
       public int BrukerId { get; set; }
       public int ProsjektId { get; set; }
       public DateTime Start { get; set; }                                      
    }
    public class ProsjektrequestMelding
    {
        public int MeldingId { get; set; }
        public DateTime Tid { get; set; }
        public int ProsjektId { get; set; }
        public string TilBruker { get; set; }
        public string FraBruker { get; set; }
        public string Prosjektnavn { get; set; }
        public string Melding { get; set; }
    }
}