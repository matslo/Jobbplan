using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Jobbplan.Models
{
    public class Prosjektdeltakelse
    {
        [Key]
        public int ProsjektDeltakerId { get; set; }
        public int BrukerId { get; set; }
        public int ProsjektId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Slutt { get; set; }

    }
}