namespace Jobbplan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prosjektdeltakelse", "Admin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prosjektdeltakelse", "Admin");
        }
    }
}
