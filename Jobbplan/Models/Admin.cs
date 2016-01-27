using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Jobbplan.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        public int ProsjektDeltakerId { get; set; }
        public int ProsjektId { get; set; }
        public string Rettigheter { get; set; }
        public string Tittel { get; set; }
    }
}