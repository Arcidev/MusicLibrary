namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewAddedEditDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AlbumReviews", "EditDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SongReviews", "EditDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SongReviews", "EditDate");
            DropColumn("dbo.AlbumReviews", "EditDate");
        }
    }
}
