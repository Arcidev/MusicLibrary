namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MultipleEntitiesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlbumId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(nullable: false),
                        Quality = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatedById, cascadeDelete: true)
                .Index(t => t.AlbumId)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Name_skSK = c.String(maxLength: 100),
                        Name_csCZ = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_Category_Name");
            
            CreateTable(
                "dbo.SongReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SongId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(nullable: false),
                        Quality = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById, cascadeDelete: true)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: true)
                .Index(t => t.SongId)
                .Index(t => t.CreatedById);
            
            AddColumn("dbo.Albums", "Approved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Albums", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Albums", "CateogoryId", c => c.Int(nullable: false));
            AddColumn("dbo.Bands", "Approved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bands", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Songs", "Approved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Songs", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Artists", "Approved", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Albums", "CateogoryId");
            AddForeignKey("dbo.Albums", "CateogoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SongReviews", "SongId", "dbo.Songs");
            DropForeignKey("dbo.SongReviews", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.AlbumReviews", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.AlbumReviews", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.Albums", "CateogoryId", "dbo.Categories");
            DropIndex("dbo.SongReviews", new[] { "CreatedById" });
            DropIndex("dbo.SongReviews", new[] { "SongId" });
            DropIndex("dbo.Categories", "IX_Category_Name");
            DropIndex("dbo.Albums", new[] { "CateogoryId" });
            DropIndex("dbo.AlbumReviews", new[] { "CreatedById" });
            DropIndex("dbo.AlbumReviews", new[] { "AlbumId" });
            DropColumn("dbo.Artists", "Approved");
            DropColumn("dbo.Songs", "CreateDate");
            DropColumn("dbo.Songs", "Approved");
            DropColumn("dbo.Bands", "CreateDate");
            DropColumn("dbo.Bands", "Approved");
            DropColumn("dbo.Albums", "CateogoryId");
            DropColumn("dbo.Albums", "CreateDate");
            DropColumn("dbo.Albums", "Approved");
            DropTable("dbo.SongReviews");
            DropTable("dbo.Categories");
            DropTable("dbo.AlbumReviews");
        }
    }
}
