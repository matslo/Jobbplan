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
            : base("Server = tcp:jobbplantest2016dbserver.database.windows.net, 1433; Database=JobbPlanDB;User ID = AdminAdmin@jobbplantest2016dbserver;Password=Gordo1414;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;")
        {
           Database.CreateIfNotExists();
        }
        public DbSet<dbBruker> Brukere { get; set; }
        public DbSet<Poststed> Poststeder { get; set; }
        public DbSet<Vakt> Vakter { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        
    }
}