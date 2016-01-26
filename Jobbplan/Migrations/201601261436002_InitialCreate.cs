namespace Jobbplan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.dbBruker",
                c => new
                    {
                        BrukerId = c.Int(nullable: false, identity: true),
                        Fornavn = c.String(),
                        Etternavn = c.String(),
                        Email = c.String(),
                        Telefonnummer = c.String(),
                        Adresse = c.String(),
                        Postnr = c.String(),
                        Passord = c.Binary(),
                    })
                .PrimaryKey(t => t.BrukerId);
            
            CreateTable(
                "dbo.Poststed",
                c => new
                    {
                        Postnummer = c.String(nullable: false, maxLength: 128),
                        PostSted = c.String(),
                    })
                .PrimaryKey(t => t.Postnummer);
            
            CreateTable(
                "dbo.Vakt",
                c => new
                    {
                        VaktId = c.Int(nullable: false, identity: true),
                        start = c.String(),
                        end = c.String(),
                        title = c.String(),
                    })
                .PrimaryKey(t => t.VaktId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vakt");
            DropTable("dbo.Poststed");
            DropTable("dbo.dbBruker");
        }
    }
}
