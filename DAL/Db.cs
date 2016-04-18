using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using Jobbplan.Model;

namespace Jobbplan.DAL
{
    public class Dbkontekst : DbContext
    {

        public Dbkontekst()
            : base("JobbplanDb")
        {
            //Database.SetInitializer<Dbkontekst>(null);
            Database.CreateIfNotExists();
        }
        
        public virtual DbSet<dbBruker> Brukere { get; set; }
        public DbSet<Vakt> Vakter { get; set; }
        public DbSet<Prosjekt> Prosjekter { get; set; }
        public DbSet<Prosjektdeltakelse> Prosjektdeltakelser { get; set; }
        public DbSet<Prosjektrequest> Prosjektrequester { get; set; }
        public DbSet<VaktRequest> Vaktrequester { get; set; }
        public DbSet<Maler> Maler { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        
    }
}