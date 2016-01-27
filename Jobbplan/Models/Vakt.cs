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
        public string Start { get; set; }
        public string End { get; set; }
        public string Title { get; set; }
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

    }
}