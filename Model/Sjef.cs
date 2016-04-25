using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Jobbplan.Model
{
    public class Sjef
    {
        public int BrukerId { get; set; }
        public int ProsjektId { get; set; }
        public string Tittel { get; set; }
    }
    public class AdminSjekk
    {
        public int BrukerId { get; set; }
        public int ProsjektId { get; set; }
    }
}