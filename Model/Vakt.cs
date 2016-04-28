using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jobbplan.Model
{
    public class Vakt
    {
        [Key]
        public int VaktId { get; set; }
        [Required]
        public DateTime start { get; set; }
        [Required]
        public DateTime end { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public bool Ledig { get; set; }
        public string Beskrivelse { get; set; }

        public int BrukerId { get; set; }
        public int ProsjektId { get; set; } 

    }
    public class Vaktkalender
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public int VaktId { get; set; }
        public int ProsjektId { get; set; }
        public string Brukernavn { get; set; }
        public bool Ledig { get; set; }
        public string Beskrivelse { get; set; }

    }
    public class Vaktskjema
    {
        
        public int Vaktid { get; set; }
        [Required(ErrorMessage = "Dato må oppgis")]
        [Display(Name = "Fra Dato")]
        public string start { get; set; }

        [Required(ErrorMessage = "Tid må oppgis")]
        [Display(Name = "Fra Dato")]
        public string startTid { get; set; }

        [Required(ErrorMessage = "tid må oppgis")]
        [Display(Name = "Tid Dato")]
        public string endTid { get; set; }

        public bool endDato { get; set; }

        [Display(Name = "Til Dato")]
        public string end { get; set; }

        [Display(Name = "Tittel")]
        [StringLength(160)]
        public string title { get; set; }

        [Display(Name = "Beskrivelse")]
        [StringLength(400)]
        public string Beskrivelse { get; set; }

        [Display(Name = "Bruker")]
        public int BrukerId { get; set; }
        public int ProsjektId { get; set; }

    }
    public class VaktRequest
    {
        [Key]
        public int MeldingId { get; set; }
        public int BrukerIdFra { get; set; }
        public int BrukerIdTil { get; set; }
        public int VaktId { get; set; }
        public string Melding { get; set; }
        public bool Akseptert { get; set; }
        public DateTime Sendt { get; set; }
        public int ProsjektId { get; set; }

        public virtual Prosjekt Prosjekt { get; set; }
        public virtual Vakt Vakt { get; set; }
        public virtual dbBruker dbBruker { get; set; }

    }
    
    public class VaktRequestOverforing
    {
        public int BrukerId { get; set; }
        public int VaktId { get; set; }
    }
    public class VaktRequestMelding
    {
        public int MeldingId { get; set; }
        public DateTime Tid { get; set; }
        public int ProsjektId { get; set; }
        public int VaktId { get; set; }
        public string TilBruker { get; set; }
        public string FraBruker { get; set; }
        public string Prosjektnavn { get; set; }
        public string Melding { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string title { get; set; }    
        public string Beskrivelse { get; set; }
    }
}