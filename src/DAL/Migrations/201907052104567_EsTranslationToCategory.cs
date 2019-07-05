namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EsTranslationToCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Name_esES", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "Name_esES");
        }
    }
}
