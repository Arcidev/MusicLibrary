namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BandAddedDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bands", "Description", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bands", "Description");
        }
    }
}
