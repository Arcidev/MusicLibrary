namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MinorEntityChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Artists", "BandId", "dbo.Bands");
            DropForeignKey("dbo.Artists", "ImageStorageFileId", "dbo.StorageFiles");
            DropIndex("dbo.Artists", new[] { "ImageStorageFileId" });
            DropIndex("dbo.Artists", new[] { "BandId" });
            CreateTable(
                "dbo.BandMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArtistId = c.Int(nullable: false),
                        BandId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artists", t => t.ArtistId, cascadeDelete: true)
                .ForeignKey("dbo.Bands", t => t.BandId, cascadeDelete: true)
                .Index(t => t.ArtistId)
                .Index(t => t.BandId);
            
            AddColumn("dbo.Songs", "AudioStorageFileId", c => c.Int());
            AddColumn("dbo.Songs", "YoutubeUrlParam", c => c.String(maxLength: 50));
            CreateIndex("dbo.Songs", "AudioStorageFileId");
            AddForeignKey("dbo.Songs", "AudioStorageFileId", "dbo.StorageFiles", "Id");
            DropColumn("dbo.Artists", "ImageStorageFileId");
            DropColumn("dbo.Artists", "BandId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Artists", "BandId", c => c.Int(nullable: false));
            AddColumn("dbo.Artists", "ImageStorageFileId", c => c.Int());
            DropForeignKey("dbo.BandMembers", "BandId", "dbo.Bands");
            DropForeignKey("dbo.BandMembers", "ArtistId", "dbo.Artists");
            DropForeignKey("dbo.Songs", "AudioStorageFileId", "dbo.StorageFiles");
            DropIndex("dbo.BandMembers", new[] { "BandId" });
            DropIndex("dbo.BandMembers", new[] { "ArtistId" });
            DropIndex("dbo.Songs", new[] { "AudioStorageFileId" });
            DropColumn("dbo.Songs", "YoutubeUrlParam");
            DropColumn("dbo.Songs", "AudioStorageFileId");
            DropTable("dbo.BandMembers");
            CreateIndex("dbo.Artists", "BandId");
            CreateIndex("dbo.Artists", "ImageStorageFileId");
            AddForeignKey("dbo.Artists", "ImageStorageFileId", "dbo.StorageFiles", "Id");
            AddForeignKey("dbo.Artists", "BandId", "dbo.Bands", "Id", cascadeDelete: true);
        }
    }
}
