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
        public int BrukerId { get; set; }
        public int ProsjektId { get; set; }
        public string Tittel { get; set; }

        public virtual Prosjekt Prosjekt { get; set; }
        public virtual dbBruker dbBruker { get; set; }
    }
    public class AdminSjekk
    {
        public int BrukerId { get; set; }
        public int ProsjektId { get; set; }
    }
}