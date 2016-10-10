namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAlbumAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAlbums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlbumId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.AlbumId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAlbums", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserAlbums", "AlbumId", "dbo.Albums");
            DropIndex("dbo.UserAlbums", new[] { "UserId" });
            DropIndex("dbo.UserAlbums", new[] { "AlbumId" });
            DropTable("dbo.UserAlbums");
        }
    }
}
