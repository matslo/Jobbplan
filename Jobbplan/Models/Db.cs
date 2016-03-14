using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;

namespace Jobbplan.Models
{
    public partial class Dbkontekst : DbContext
    {
        public Dbkontekst()
            : base("JobbplanD")
        {
            //Database.SetInitializer<Dbkontekst>(null);
            Database.CreateIfNotExists();
        }
        
        public DbSet<dbBruker> Brukere { get; set; }
        public DbSet<Poststed> Poststeder { get; set; }
        public DbSet<Vakt> Vakter { get; set; }
        public DbSet<Prosjekt> Prosjekter { get; set; }
        public DbSet<Prosjektdeltakelse> Prosjektdeltakelser { get; set; }
        public DbSet<Prosjektrequest> Prosjektrequester { get; set; }
        public DbSet<VaktRequest> Vaktrequester { get; set; }
        public DbSet<Sjef> Sjefer { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        
    }
}