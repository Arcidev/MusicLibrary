using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameskSK = table.Column<string>(name: "Name_skSK", type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NamecsCZ = table.Column<string>(name: "Name_csCZ", type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NameesES = table.Column<string>(name: "Name_esES", type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ImageStorageFileId = table.Column<int>(type: "int", nullable: true),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bands_StorageFiles_ImageStorageFileId",
                        column: x => x.ImageStorageFileId,
                        principalTable: "StorageFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AudioStorageFileId = table.Column<int>(type: "int", nullable: true),
                    YoutubeUrlParam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Songs_StorageFiles_AudioStorageFileId",
                        column: x => x.AudioStorageFileId,
                        principalTable: "StorageFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false),
                    ImageStorageFileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_StorageFiles_ImageStorageFileId",
                        column: x => x.ImageStorageFileId,
                        principalTable: "StorageFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageStorageFileId = table.Column<int>(type: "int", nullable: true),
                    BandId = table.Column<int>(type: "int", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    AverageQuality = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_Bands_BandId",
                        column: x => x.BandId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Albums_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Albums_StorageFiles_ImageStorageFileId",
                        column: x => x.ImageStorageFileId,
                        principalTable: "StorageFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BandMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    BandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BandMembers_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandMembers_Bands_BandId",
                        column: x => x.BandId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SliderImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BandId = table.Column<int>(type: "int", nullable: false),
                    ImageStorageFileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SliderImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SliderImages_Bands_BandId",
                        column: x => x.BandId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SliderImages_StorageFiles_ImageStorageFileId",
                        column: x => x.ImageStorageFileId,
                        principalTable: "StorageFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BandReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BandId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    Quality = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BandReviews_Bands_BandId",
                        column: x => x.BandId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandReviews_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    Quality = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlbumReviews_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumReviews_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumSongs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    SongId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumSongs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlbumSongs_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumSongs_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAlbums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAlbums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAlbums_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAlbums_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "Approved", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, true, "Till", "Lindemann" },
                    { 2, true, "Richard", "Kruspe" },
                    { 3, true, "Paul", "Landers" },
                    { 4, true, "Christian", "Lorenz" },
                    { 5, true, "Oliver", "Riedel" },
                    { 6, true, "Christoph", "Schneider" },
                    { 7, true, "Peter", "Tägtgren" },
                    { 8, true, "Corey", "Taylor" },
                    { 9, true, "Mick", "Thomson" },
                    { 10, true, "Sid", "Wilson" },
                    { 11, true, "Jim", "Root" },
                    { 12, true, "Craig", "Jones" },
                    { 13, true, "Shawn", "Crahan" },
                    { 14, true, "Alessandro", "Venturella" },
                    { 15, true, "Jay", "Weinberg" },
                    { 16, true, "Jonathan", "Davis" },
                    { 17, true, "James", "Shaffer" },
                    { 18, true, "Reginald", "Arvizu" },
                    { 19, true, "Brian", "Welch" },
                    { 20, true, "Ray", "Luzier" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Name_csCZ", "Name_esES", "Name_skSK" },
                values: new object[,]
                {
                    { 1, "Metal", "Metal", "Metal", "Metal" },
                    { 2, "Rock", "Rock", "Rock", "Rock" }
                });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Approved", "AudioStorageFileId", "CreateDate", "Name", "YoutubeUrlParam" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8973), "Ich Will", "EOnSh3QlpbQ" },
                    { 2, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8977), "Feuer Frei!", "ZkW-K5RQdzo" },
                    { 3, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8979), "Haifisch", "GukNjYQZW8s" },
                    { 4, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8981), "Ich Tu Dir Weh", "IxuEtL7gxoM" },
                    { 5, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8983), "Jekyll And Hyde", "HCBPmxiVMKk" },
                    { 6, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8985), "Fight for your praise", "oy7_bkN5eMU" },
                    { 7, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8987), "Miss You", "b9-fzLvC-bY" },
                    { 8, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8989), "Fish On", "eciZWNdkGqs" },
                    { 9, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8991), "Praise Abort", "QWE_M0CX9So" },
                    { 10, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8993), "Dead Memories", "9gsAz6S_zSw" },
                    { 11, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8995), "Psychosocial", "eIf3b6meriM" },
                    { 12, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8997), "Snuff", "LXEKuttVRIo" },
                    { 13, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8999), "Left Behind", "D1jQKpse7Yw" },
                    { 14, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(9001), "Metabolic", "b3FpfQOxiKA" },
                    { 15, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(9003), "Falling Away from Me", "2s3iGpDqQpQ" },
                    { 16, true, null, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(9005), "Somebody Someone", "FBEE-t-uyI0" }
                });

            migrationBuilder.InsertData(
                table: "StorageFiles",
                columns: new[] { "Id", "DisplayName", "FileName" },
                values: new object[,]
                {
                    { 1, "rammstein_logo.jpg", "rammstein_logo.jpg" },
                    { 2, "rammstein_guitar.jpg", "rammstein_guitar.jpg" },
                    { 3, "Rammstein-Till-Lindemann-fire.jpg", "Rammstein-Till-Lindemann-fire.jpg" },
                    { 4, "Mutter.jpg", "Mutter.jpg" },
                    { 5, "Cover_lifad.png", "Cover_lifad.png" },
                    { 6, "finger_death_punch_logo_1.gif", "finger_death_punch_logo_1.gif" },
                    { 7, "finger_death_punch.jpg", "finger_death_punch.jpg" },
                    { 8, "FFDP-Got-Your-Six-Album-Cover.jpg", "FFDP-Got-Your-Six-Album-Cover.jpg" },
                    { 9, "Andragona.jpg", "Andragona.jpg" },
                    { 10, "EndOfTheProphesiedDawn.jpg", "EndOfTheProphesiedDawn.jpg" },
                    { 11, "Lindemann_logo.jpg", "Lindemann_logo.jpg" },
                    { 12, "Lindemann_Skills_in_Pills.jpg", "Lindemann_Skills_in_Pills.jpg" },
                    { 13, "slipknot_logo.jpg", "slipknot_logo.jpg" },
                    { 14, "Slipknot.jpg", "Slipknot.jpg" },
                    { 15, "Slipknot_Taylor.jpg", "Slipknot_Taylor.jpg" },
                    { 16, "All_Hope_is_Gone.jpg", "All_Hope_is_Gone.jpg" },
                    { 17, "Slipknot_Iowa.jpg", "Slipknot_Iowa.jpg" },
                    { 18, "KoRnIssues.jpg", "KoRnIssues.jpg" },
                    { 19, "Korn_logo.jpg", "Korn_logo.jpg" },
                    { 20, "korn-5820-1078x516-1471045959.jpg", "korn-5820-1078x516-1471045959.jpg" },
                    { 21, "OB-UL405_ikorn_G_20120906052811.jpg", "OB-UL405_ikorn_G_20120906052811.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "ImageStorageFileId", "LastName", "PasswordHash", "PasswordSalt", "UserRole" },
                values: new object[,]
                {
                    { 1, "user@admin.com", "User", null, "Admin", "3CehAkxz/wJvTTB2YA/XoE1DeIHWKviWJrC6RZUmLodcIkupRzW3SEOChHW7QchUDAgX1uVrUVEwAeoz3LMLoA==", "uLplTPeTOGuBPi4z2vRwhXo5cOaGLLG0NaoiI+fqwLI=", 2 },
                    { 2, "user@superuser.com", "Super", null, "User", "3CehAkxz/wJvTTB2YA/XoE1DeIHWKviWJrC6RZUmLodcIkupRzW3SEOChHW7QchUDAgX1uVrUVEwAeoz3LMLoA==", "uLplTPeTOGuBPi4z2vRwhXo5cOaGLLG0NaoiI+fqwLI=", 1 }
                });

            migrationBuilder.InsertData(
                table: "Bands",
                columns: new[] { "Id", "Approved", "CreateDate", "Description", "ImageStorageFileId", "Name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8820), "Industrial metal", 1, "Rammstein" },
                    { 2, true, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8885), "Heavy metal", 6, "Five Finger Death Punch" },
                    { 3, true, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8887), "Power metal", 9, "Andragona" },
                    { 4, true, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8890), "Industrial metal", 11, "Lindemann" },
                    { 5, true, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8892), "Heavy metal", 13, "Slipknot" },
                    { 6, true, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8894), "Nu Metal", 19, "Korn" }
                });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "Approved", "AverageQuality", "BandId", "CategoryId", "CreateDate", "ImageStorageFileId", "Name" },
                values: new object[,]
                {
                    { 1, true, 0m, 1, 1, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8936), 4, "Mutter" },
                    { 2, true, 0m, 1, 1, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8941), 5, "Liebe ist für alle da" },
                    { 3, true, 0m, 2, 1, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8944), 8, "GOT YOUR SIX" },
                    { 4, true, 0m, 3, 1, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8946), 10, "End Of The Prophesied Dawn" },
                    { 5, true, 0m, 4, 1, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8948), 12, "Skills in Pills" },
                    { 6, true, 0m, 5, 1, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8951), 16, "All hope is gone" },
                    { 7, true, 0m, 5, 1, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8953), 17, "Iowa" },
                    { 8, true, 0m, 6, 1, new DateTime(2022, 11, 16, 13, 54, 11, 842, DateTimeKind.Local).AddTicks(8955), 18, "Issues" }
                });

            migrationBuilder.InsertData(
                table: "BandMembers",
                columns: new[] { "Id", "ArtistId", "BandId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 1 },
                    { 4, 4, 1 },
                    { 5, 5, 1 },
                    { 6, 6, 1 },
                    { 7, 1, 4 },
                    { 8, 7, 4 },
                    { 9, 8, 5 },
                    { 10, 9, 5 },
                    { 11, 10, 5 },
                    { 12, 11, 5 },
                    { 13, 12, 5 },
                    { 14, 13, 5 },
                    { 15, 14, 5 },
                    { 16, 15, 5 },
                    { 17, 16, 6 },
                    { 18, 17, 6 },
                    { 19, 18, 6 },
                    { 20, 19, 6 },
                    { 21, 20, 6 }
                });

            migrationBuilder.InsertData(
                table: "SliderImages",
                columns: new[] { "Id", "BandId", "ImageStorageFileId" },
                values: new object[,]
                {
                    { 1, 1, 2 },
                    { 2, 1, 3 },
                    { 3, 2, 7 },
                    { 4, 5, 14 },
                    { 5, 5, 15 },
                    { 6, 6, 20 },
                    { 7, 6, 21 }
                });

            migrationBuilder.InsertData(
                table: "AlbumSongs",
                columns: new[] { "Id", "AlbumId", "SongId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 2, 3 },
                    { 4, 2, 4 },
                    { 5, 3, 5 },
                    { 6, 4, 6 },
                    { 7, 4, 7 },
                    { 8, 5, 8 },
                    { 9, 5, 9 },
                    { 10, 6, 10 },
                    { 11, 6, 11 },
                    { 12, 6, 12 },
                    { 13, 7, 13 },
                    { 14, 7, 14 },
                    { 15, 8, 15 },
                    { 16, 8, 16 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumReviews_AlbumId",
                table: "AlbumReviews",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumReviews_CreatedById",
                table: "AlbumReviews",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_BandId",
                table: "Albums",
                column: "BandId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_CategoryId",
                table: "Albums",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ImageStorageFileId",
                table: "Albums",
                column: "ImageStorageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumSongs_AlbumId",
                table: "AlbumSongs",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumSongs_SongId",
                table: "AlbumSongs",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_BandMembers_ArtistId",
                table: "BandMembers",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_BandMembers_BandId",
                table: "BandMembers",
                column: "BandId");

            migrationBuilder.CreateIndex(
                name: "IX_BandReviews_BandId",
                table: "BandReviews",
                column: "BandId");

            migrationBuilder.CreateIndex(
                name: "IX_BandReviews_CreatedById",
                table: "BandReviews",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Bands_ImageStorageFileId",
                table: "Bands",
                column: "ImageStorageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SliderImages_BandId",
                table: "SliderImages",
                column: "BandId");

            migrationBuilder.CreateIndex(
                name: "IX_SliderImages_ImageStorageFileId",
                table: "SliderImages",
                column: "ImageStorageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_AudioStorageFileId",
                table: "Songs",
                column: "AudioStorageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAlbums_AlbumId",
                table: "UserAlbums",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAlbums_UserId",
                table: "UserAlbums",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ImageStorageFileId",
                table: "Users",
                column: "ImageStorageFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumReviews");

            migrationBuilder.DropTable(
                name: "AlbumSongs");

            migrationBuilder.DropTable(
                name: "BandMembers");

            migrationBuilder.DropTable(
                name: "BandReviews");

            migrationBuilder.DropTable(
                name: "SliderImages");

            migrationBuilder.DropTable(
                name: "UserAlbums");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Bands");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "StorageFiles");
        }
    }
}
