using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;

namespace Jobbplan.Models
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public partial class Dbkontekst : DbContext
    {
        public Dbkontekst()
            : base("name=Jobbplan")
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