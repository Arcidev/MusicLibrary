namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSliderImage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ImageStorageFileId = c.Int(),
                        BandId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bands", t => t.BandId, cascadeDelete: true)
                .ForeignKey("dbo.StorageFiles", t => t.ImageStorageFileId)
                .Index(t => t.ImageStorageFileId)
                .Index(t => t.BandId);
            
            CreateTable(
                "dbo.Bands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ImageStorageFileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StorageFiles", t => t.ImageStorageFileId)
                .Index(t => t.ImageStorageFileId);
            
            CreateTable(
                "dbo.StorageFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(nullable: false, maxLength: 100),
                        FileName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.FileName, unique: true, name: "IX_StorageFile_FileName");
            
            CreateTable(
                "dbo.SliderImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BandId = c.Int(nullable: false),
                        ImageStorageFileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bands", t => t.BandId, cascadeDelete: true)
                .ForeignKey("dbo.StorageFiles", t => t.ImageStorageFileId, cascadeDelete: true)
                .Index(t => t.BandId)
                .Index(t => t.ImageStorageFileId);
            
            CreateTable(
                "dbo.AlbumSongs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlbumId = c.Int(nullable: false),
                        SongId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: true)
                .Index(t => t.AlbumId)
                .Index(t => t.SongId);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        ImageStorageFileId = c.Int(),
                        BandId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bands", t => t.BandId, cascadeDelete: true)
                .ForeignKey("dbo.StorageFiles", t => t.ImageStorageFileId)
                .Index(t => t.ImageStorageFileId)
                .Index(t => t.BandId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 100),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        PasswordSalt = c.String(nullable: false, maxLength: 100),
                        PasswordHash = c.String(nullable: false, maxLength: 100),
                        UserRole = c.Int(nullable: false),
                        ImageStorageFileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StorageFiles", t => t.ImageStorageFileId)
                .Index(t => t.Email, unique: true, name: "IX_User_Email")
                .Index(t => t.ImageStorageFileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "ImageStorageFileId", "dbo.StorageFiles");
            DropForeignKey("dbo.Artists", "ImageStorageFileId", "dbo.StorageFiles");
            DropForeignKey("dbo.Artists", "BandId", "dbo.Bands");
            DropForeignKey("dbo.AlbumSongs", "SongId", "dbo.Songs");
            DropForeignKey("dbo.AlbumSongs", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.Albums", "ImageStorageFileId", "dbo.StorageFiles");
            DropForeignKey("dbo.Albums", "BandId", "dbo.Bands");
            DropForeignKey("dbo.SliderImages", "ImageStorageFileId", "dbo.StorageFiles");
            DropForeignKey("dbo.SliderImages", "BandId", "dbo.Bands");
            DropForeignKey("dbo.Bands", "ImageStorageFileId", "dbo.StorageFiles");
            DropIndex("dbo.Users", new[] { "ImageStorageFileId" });
            DropIndex("dbo.Users", "IX_User_Email");
            DropIndex("dbo.Artists", new[] { "BandId" });
            DropIndex("dbo.Artists", new[] { "ImageStorageFileId" });
            DropIndex("dbo.AlbumSongs", new[] { "SongId" });
            DropIndex("dbo.AlbumSongs", new[] { "AlbumId" });
            DropIndex("dbo.SliderImages", new[] { "ImageStorageFileId" });
            DropIndex("dbo.SliderImages", new[] { "BandId" });
            DropIndex("dbo.StorageFiles", "IX_StorageFile_FileName");
            DropIndex("dbo.Bands", new[] { "ImageStorageFileId" });
            DropIndex("dbo.Albums", new[] { "BandId" });
            DropIndex("dbo.Albums", new[] { "ImageStorageFileId" });
            DropTable("dbo.Users");
            DropTable("dbo.Artists");
            DropTable("dbo.Songs");
            DropTable("dbo.AlbumSongs");
            DropTable("dbo.SliderImages");
            DropTable("dbo.StorageFiles");
            DropTable("dbo.Bands");
            DropTable("dbo.Albums");
        }
    }
}
