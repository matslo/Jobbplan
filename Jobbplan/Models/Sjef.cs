using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Jobbplan.Models
{
    public class Sjef
    {
        [Key]
        public int SjefId { get; set; }
        public int ProsjektDeltakerId { get; set; }
        public string Rettigheter { get; set; }
        public string Tittel { get; set; }

       // public virtual Prosjektdeltakelse Prosjektdeltakelse { get; set; }
    }
}