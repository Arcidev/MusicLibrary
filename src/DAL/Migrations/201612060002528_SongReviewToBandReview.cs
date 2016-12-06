namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SongReviewToBandReview : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SongReviews", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SongReviews", "SongId", "dbo.Songs");
            DropIndex("dbo.SongReviews", new[] { "SongId" });
            DropIndex("dbo.SongReviews", new[] { "CreatedById" });
            CreateTable(
                "dbo.BandReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BandId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(nullable: false),
                        Quality = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bands", t => t.BandId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatedById, cascadeDelete: true)
                .Index(t => t.BandId)
                .Index(t => t.CreatedById);
            
            DropTable("dbo.SongReviews");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SongReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SongId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(nullable: false),
                        Quality = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.BandReviews", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.BandReviews", "BandId", "dbo.Bands");
            DropIndex("dbo.BandReviews", new[] { "CreatedById" });
            DropIndex("dbo.BandReviews", new[] { "BandId" });
            DropTable("dbo.BandReviews");
            CreateIndex("dbo.SongReviews", "CreatedById");
            CreateIndex("dbo.SongReviews", "SongId");
            AddForeignKey("dbo.SongReviews", "SongId", "dbo.Songs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SongReviews", "CreatedById", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
