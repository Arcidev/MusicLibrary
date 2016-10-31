namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlbumFixedName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Albums", name: "CateogoryId", newName: "CategoryId");
            RenameIndex(table: "dbo.Albums", name: "IX_CateogoryId", newName: "IX_CategoryId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Albums", name: "IX_CategoryId", newName: "IX_CateogoryId");
            RenameColumn(table: "dbo.Albums", name: "CategoryId", newName: "CateogoryId");
        }
    }
}
