namespace Jobbplan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                "dbo.Prosjekt",
                c => new
                    {
                        ProsjektId = c.Int(nullable: false, identity: true),
                        EierId = c.Int(nullable: false),
                        Arbeidsplass = c.String(),
                    })
                .PrimaryKey(t => t.ProsjektId);
            
            CreateTable(
                "dbo.Vakt",
                c => new
                    {
                        VaktId = c.Int(nullable: false, identity: true),
                        start = c.String(),
                        end = c.String(),
                        title = c.String(),
                        color = c.String(),
                        Ledig = c.Boolean(nullable: false),
                        Beskrivelse = c.String(),
                        BrukerId = c.Int(nullable: false),
                        ProsjektId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VaktId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vakt");
            DropTable("dbo.Prosjekt");
            DropTable("dbo.Poststed");
            DropTable("dbo.dbBruker");
        }
    }
}
