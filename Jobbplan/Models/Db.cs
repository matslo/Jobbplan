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
            : base("name=Jpl")
        {
           Database.CreateIfNotExists();
        }
        public DbSet<Bruker.dbBruker> Brukere { get; set; }
        public DbSet<Poststed> Poststeder { get; set; }
        public DbSet<Vakt> Vakter { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        
    }
}