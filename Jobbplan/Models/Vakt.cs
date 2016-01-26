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
    }
    public class Vaktskjema
    {
        [ScaffoldColumn(false)]
        public int id { get; set; }

        [Required(ErrorMessage = "Fornavn må oppgis")]
        [Display(Name = "Fra Dato")]
        public DateTime start { get; set; }

        [Required(ErrorMessage = "Fornavn må oppgis")]
        [Display(Name = "Til Dato")]
        public DateTime end { get; set; }

        [Required(ErrorMessage = "Fornavn må oppgis")]
        [Display(Name = "Title")]
        [StringLength(160)]
        public string title { get; set; }

    }
}