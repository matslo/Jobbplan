using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jobbplan.Models
{
    public class Poststed
    {
        [Key]
        int Postnummer { get; set; }
        string PostSted { get; set; }       
    }
}