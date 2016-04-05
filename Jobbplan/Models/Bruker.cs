using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jobbplan.Models
{

    public class dbBruker
    {
        [Key]
        public int BrukerId { get; set; }
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public string Email { get; set; }
        public string Telefonnummer { get; set; }
        public string Adresse { get; set; }
        public string Postnr { get; set; }
        public byte[] Passord { get; set; }

         public virtual Poststed Poststed { get; set; }
         public virtual List<Prosjektdeltakelse> Prosjektdeltakelse { get; set; }
    }
   
        public class Registrer
        {    
            public int id { get; set; }

            [Required]
            [Display(Name = "Fornavn")]
            public string Fornavn { get; set; }

            [Required]
            [Display(Name = "Etternavn")]
            public string Etternavn { get; set; }

            [Required]
            [DataType(DataType.EmailAddress)]
            [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Email er ikke gyldig")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Telefonnummer")]
            public string Telefonnummer { get; set; }

            [Required]
            [Display(Name = "Adresse")]
            public string Adresse { get; set; }

            [Required]
            [Display(Name = "Postnummer")]
            public string Postnummer { get; set; }

            [Required]
            [Display(Name = "Poststed")]
            public string Poststed { get; set; }

        
            [Required]
            [DataType(DataType.Password)]
            [StringLength(100, ErrorMessage = "Passordet må være minst seks tegn", MinimumLength = 6)]
            [Display(Name = "Bekreft Passord")]
            public string BekreftPassord { get; set; }
        }
        public class LogInn
        {
            [Required(ErrorMessage = "Du må skrive brukernavn")]
            [Display(Name = "Brukernavn")]
            public string Brukernavn { get; set; }

            [Required(ErrorMessage = "Du må skrive inn passord!")]
            [DataType(DataType.Password)]
            [Display(Name = "Passord")]
            public string Passord { get; set; }
        }
    public class BrukerListe
    {
        public int ProsjektDeltakerId { get; set; }
        public string Navn { get; set; }
        public string Brukernavn { get; set; }
        public int BrukerId { get; set; }
        public bool Admin { get; set; }
    }
    public class Profil
    {
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public string Email { get; set; }
        public string Telefonnummer { get; set; }
        public string Adresse { get; set; }
        public string Postnummer { get; set; }
        public string Poststed { get; set; }
       
    }

}
