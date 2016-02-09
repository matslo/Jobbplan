using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jobbplan.Models
{
    public class Vakt
    {
        [Key]
        public int VaktId { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public bool Ledig { get; set; }
        public string Beskrivelse { get; set; }

        public int BrukerId { get; set; }
        public int ProsjektId { get; set; } 

    }
    public class Vaktskjema
    {
        [ScaffoldColumn(false)]
        public int id { get; set; }

        [Required(ErrorMessage = "Dato må oppgis")]
        [Display(Name = "Fra Dato")]
        public DateTime start { get; set; }

        [Required(ErrorMessage = "Dato må oppgis")]
        [Display(Name = "Til Dato")]
        public DateTime end { get; set; }

        [Display(Name = "Tittel")]
        [StringLength(160)]
        public string title { get; set; }

        [Display(Name = "Beskrivelse")]
        [StringLength(400)]
        public string Beskrivelse { get; set; }

        [Display(Name = "Bruker")]
        public int BrukerId { get; set; }
        
    }
}