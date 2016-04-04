namespace Jobbplan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstUpdate : DbMigration
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
                        Poststed_Postnummer = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BrukerId)
                .ForeignKey("dbo.Poststed", t => t.Poststed_Postnummer)
                .Index(t => t.Poststed_Postnummer);
            
            CreateTable(
                "dbo.Poststed",
                c => new
                    {
                        Postnummer = c.String(nullable: false, maxLength: 128),
                        PostSted = c.String(),
                    })
                .PrimaryKey(t => t.Postnummer);
            
            CreateTable(
                "dbo.Prosjektdeltakelse",
                c => new
                    {
                        ProsjektDeltakerId = c.Int(nullable: false, identity: true),
                        BrukerId = c.Int(nullable: false),
                        ProsjektId = c.Int(nullable: false),
                        Medlemsdato = c.DateTime(nullable: false),
                        Admin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProsjektDeltakerId)
                .ForeignKey("dbo.dbBruker", t => t.BrukerId, cascadeDelete: true)
                .ForeignKey("dbo.Prosjekt", t => t.ProsjektId, cascadeDelete: true)
                .Index(t => t.BrukerId)
                .Index(t => t.ProsjektId);
            
            CreateTable(
                "dbo.Prosjekt",
                c => new
                    {
                        ProsjektId = c.Int(nullable: false, identity: true),
                        EierId = c.Int(nullable: false),
                        Arbeidsplass = c.String(),
                        dbBruker_BrukerId = c.Int(),
                    })
                .PrimaryKey(t => t.ProsjektId)
                .ForeignKey("dbo.dbBruker", t => t.dbBruker_BrukerId)
                .Index(t => t.dbBruker_BrukerId);
            
            CreateTable(
                "dbo.Sjef",
                c => new
                    {
                        SjefId = c.Int(nullable: false, identity: true),
                        BrukerId = c.Int(nullable: false),
                        ProsjektId = c.Int(nullable: false),
                        Tittel = c.String(),
                    })
                .PrimaryKey(t => t.SjefId)
                .ForeignKey("dbo.dbBruker", t => t.BrukerId, cascadeDelete: true)
                .ForeignKey("dbo.Prosjekt", t => t.ProsjektId, cascadeDelete: true)
                .Index(t => t.BrukerId)
                .Index(t => t.ProsjektId);
            
            CreateTable(
                "dbo.Prosjektrequest",
                c => new
                    {
                        MeldingId = c.Int(nullable: false, identity: true),
                        BrukerIdFra = c.Int(nullable: false),
                        BrukerIdTil = c.Int(nullable: false),
                        ProsjektId = c.Int(nullable: false),
                        Akseptert = c.Boolean(nullable: false),
                        Sendt = c.DateTime(nullable: false),
                        dbBruker_BrukerId = c.Int(),
                    })
                .PrimaryKey(t => t.MeldingId)
                .ForeignKey("dbo.dbBruker", t => t.dbBruker_BrukerId)
                .ForeignKey("dbo.Prosjekt", t => t.ProsjektId, cascadeDelete: true)
                .Index(t => t.ProsjektId)
                .Index(t => t.dbBruker_BrukerId);
            
            CreateTable(
                "dbo.Vakt",
                c => new
                    {
                        VaktId = c.Int(nullable: false, identity: true),
                        start = c.DateTime(nullable: false),
                        end = c.DateTime(nullable: false),
                        title = c.String(),
                        color = c.String(),
                        Ledig = c.Boolean(nullable: false),
                        Beskrivelse = c.String(),
                        BrukerId = c.Int(nullable: false),
                        ProsjektId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VaktId);
            
            CreateTable(
                "dbo.VaktRequest",
                c => new
                    {
                        MeldingId = c.Int(nullable: false, identity: true),
                        BrukerIdFra = c.Int(nullable: false),
                        BrukerIdTil = c.Int(nullable: false),
                        VaktId = c.Int(nullable: false),
                        Melding = c.String(),
                        Akseptert = c.Boolean(nullable: false),
                        Sendt = c.DateTime(nullable: false),
                        ProsjektId = c.Int(nullable: false),
                        dbBruker_BrukerId = c.Int(),
                    })
                .PrimaryKey(t => t.MeldingId)
                .ForeignKey("dbo.dbBruker", t => t.dbBruker_BrukerId)
                .ForeignKey("dbo.Prosjekt", t => t.ProsjektId, cascadeDelete: true)
                .ForeignKey("dbo.Vakt", t => t.VaktId, cascadeDelete: true)
                .Index(t => t.VaktId)
                .Index(t => t.ProsjektId)
                .Index(t => t.dbBruker_BrukerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VaktRequest", "VaktId", "dbo.Vakt");
            DropForeignKey("dbo.VaktRequest", "ProsjektId", "dbo.Prosjekt");
            DropForeignKey("dbo.VaktRequest", "dbBruker_BrukerId", "dbo.dbBruker");
            DropForeignKey("dbo.Prosjektrequest", "ProsjektId", "dbo.Prosjekt");
            DropForeignKey("dbo.Prosjektrequest", "dbBruker_BrukerId", "dbo.dbBruker");
            DropForeignKey("dbo.Sjef", "ProsjektId", "dbo.Prosjekt");
            DropForeignKey("dbo.Sjef", "BrukerId", "dbo.dbBruker");
            DropForeignKey("dbo.Prosjektdeltakelse", "ProsjektId", "dbo.Prosjekt");
            DropForeignKey("dbo.Prosjekt", "dbBruker_BrukerId", "dbo.dbBruker");
            DropForeignKey("dbo.Prosjektdeltakelse", "BrukerId", "dbo.dbBruker");
            DropForeignKey("dbo.dbBruker", "Poststed_Postnummer", "dbo.Poststed");
            DropIndex("dbo.VaktRequest", new[] { "dbBruker_BrukerId" });
            DropIndex("dbo.VaktRequest", new[] { "ProsjektId" });
            DropIndex("dbo.VaktRequest", new[] { "VaktId" });
            DropIndex("dbo.Prosjektrequest", new[] { "dbBruker_BrukerId" });
            DropIndex("dbo.Prosjektrequest", new[] { "ProsjektId" });
            DropIndex("dbo.Sjef", new[] { "ProsjektId" });
            DropIndex("dbo.Sjef", new[] { "BrukerId" });
            DropIndex("dbo.Prosjekt", new[] { "dbBruker_BrukerId" });
            DropIndex("dbo.Prosjektdeltakelse", new[] { "ProsjektId" });
            DropIndex("dbo.Prosjektdeltakelse", new[] { "BrukerId" });
            DropIndex("dbo.dbBruker", new[] { "Poststed_Postnummer" });
            DropTable("dbo.VaktRequest");
            DropTable("dbo.Vakt");
            DropTable("dbo.Prosjektrequest");
            DropTable("dbo.Sjef");
            DropTable("dbo.Prosjekt");
            DropTable("dbo.Prosjektdeltakelse");
            DropTable("dbo.Poststed");
            DropTable("dbo.dbBruker");
        }
    }
}
