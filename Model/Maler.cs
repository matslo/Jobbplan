using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jobbplan.Model
{
    public class Maler
    {
        [Key]
        public int MalId { get; set; }
        [Required]
        public string Tittel { get; set; }
        [Required]
        public string startTid { get; set; }
        [Required]
        public string sluttTid { get; set; }
        [Required]
        public int ProsjektId { get; set; }

        public virtual Prosjekt Prosjekt { get; set; }

    }
    public class MalerSkjema
    {
        [Required]
        public string Tittel { get; set; }

        [Required]      
        public string startTid { get; set; }

        [Required]       
        public string sluttTid { get; set; }
        [Required]
        public int ProsjektId { get; set; }
    }

    public class VisMaler
    {
        public string Tittel { get; set; }
        public string startTid { get; set; }
        public string sluttTid { get; set; }
    }

}