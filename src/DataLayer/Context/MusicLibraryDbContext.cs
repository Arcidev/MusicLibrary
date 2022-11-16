using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataLayer.Context
{
    public class MusicLibraryDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }

        public DbSet<AlbumReview> AlbumReviews { get; set; }

        public DbSet<AlbumSong> AlbumSongs { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Band> Bands { get; set; }

        public DbSet<BandReview> BandReviews { get; set; }

        public DbSet<BandMember> BandMembers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SliderImage> SliderImages { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<StorageFile> StorageFiles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserAlbum> UserAlbums { get; set; }

        public MusicLibraryDbContext(DbContextOptions<MusicLibraryDbContext> options) : base(options) { }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            builder.Entity<Category>().HasIndex(x => x.Name).IsUnique();

            SeedData(builder);
        }

        private static void SeedData(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(new Category()
            {
                Id = 1,
                Name = "Metal",
                Name_csCZ = "Metal",
                Name_skSK = "Metal",
                Name_esES = "Metal"
            },
            new Category()
            {
                Id = 2,
                Name = "Rock",
                Name_csCZ = "Rock",
                Name_skSK = "Rock",
                Name_esES = "Rock"
            });

            builder.Entity<Artist>().HasData(new Artist()
            {
                Id = 1,
                FirstName = "Till",
                LastName = "Lindemann",
                Approved = true
            }, new Artist()
            {
                Id = 2,
                FirstName = "Richard",
                LastName = "Kruspe",
                Approved = true
            }, new Artist()
            {
                Id = 3,
                FirstName = "Paul",
                LastName = "Landers",
                Approved = true
            }, new Artist()
            {
                Id = 4,
                FirstName = "Christian",
                LastName = "Lorenz",
                Approved = true
            }, new Artist()
            {
                Id = 5,
                FirstName = "Oliver",
                LastName = "Riedel",
                Approved = true
            }, new Artist()
            {
                Id = 6,
                FirstName = "Christoph",
                LastName = "Schneider",
                Approved = true
            }, new Artist()
            {
                Id = 7,
                Approved = true,
                FirstName = "Peter",
                LastName = "Tägtgren"
            }, new Artist()
            {
                Id = 8,
                FirstName = "Corey",
                LastName = "Taylor",
                Approved = true
            }, new Artist()
            {
                Id = 9,
                FirstName = "Mick",
                LastName = "Thomson",
                Approved = true
            }, new Artist()
            {
                Id = 10,
                FirstName = "Sid",
                LastName = "Wilson",
                Approved = true
            }, new Artist()
            {
                Id = 11,
                FirstName = "Jim",
                LastName = "Root",
                Approved = true
            }, new Artist()
            {
                Id = 12,
                FirstName = "Craig",
                LastName = "Jones",
                Approved = true
            }, new Artist()
            {
                Id = 13,
                FirstName = "Shawn",
                LastName = "Crahan",
                Approved = true
            }, new Artist()
            {
                Id = 14,
                FirstName = "Alessandro",
                LastName = "Venturella",
                Approved = true
            }, new Artist()
            {
                Id = 15,
                FirstName = "Jay",
                LastName = "Weinberg",
                Approved = true
            }, new Artist()
            {
                Id = 16,
                Approved = true,
                FirstName = "Jonathan",
                LastName = "Davis"
            },
            new Artist()
            {
                Id = 17,
                Approved = true,
                FirstName = "James",
                LastName = "Shaffer"
            },
            new Artist()
            {
                Id = 18,
                Approved = true,
                FirstName = "Reginald",
                LastName = "Arvizu"
            },
            new Artist()
            {
                Id = 19,
                Approved = true,
                FirstName = "Brian",
                LastName = "Welch"
            },
            new Artist()
            {
                Id = 20,
                Approved = true,
                FirstName = "Ray",
                LastName = "Luzier"
            });

            builder.Entity<StorageFile>().HasData(new StorageFile()
            {
                Id = 1,
                DisplayName = "rammstein_logo.jpg",
                FileName = "rammstein_logo.jpg"
            }, new StorageFile()
            {
                Id = 2,
                DisplayName = "rammstein_guitar.jpg",
                FileName = "rammstein_guitar.jpg"
            }, new StorageFile()
            {
                Id = 3,
                DisplayName = "Rammstein-Till-Lindemann-fire.jpg",
                FileName = "Rammstein-Till-Lindemann-fire.jpg"
            }, new StorageFile()
            {
                Id = 4,
                DisplayName = "Mutter.jpg",
                FileName = "Mutter.jpg"
            }, new StorageFile()
            {
                Id = 5,
                DisplayName = "Cover_lifad.png",
                FileName = "Cover_lifad.png"
            }, new StorageFile()
            {
                Id = 6,
                DisplayName = "finger_death_punch_logo_1.gif",
                FileName = "finger_death_punch_logo_1.gif"
            }, new StorageFile()
            {
                Id = 7,
                DisplayName = "finger_death_punch.jpg",
                FileName = "finger_death_punch.jpg"
            }, new StorageFile()
            {
                Id = 8,
                DisplayName = "FFDP-Got-Your-Six-Album-Cover.jpg",
                FileName = "FFDP-Got-Your-Six-Album-Cover.jpg"
            }, new StorageFile()
            {
                Id = 9,
                DisplayName = "Andragona.jpg",
                FileName = "Andragona.jpg"
            }, new StorageFile()
            {
                Id = 10,
                DisplayName = "EndOfTheProphesiedDawn.jpg",
                FileName = "EndOfTheProphesiedDawn.jpg"
            }, new StorageFile()
            {
                Id = 11,
                DisplayName = "Lindemann_logo.jpg",
                FileName = "Lindemann_logo.jpg"
            }, new StorageFile()
            {
                Id = 12,
                DisplayName = "Lindemann_Skills_in_Pills.jpg",
                FileName = "Lindemann_Skills_in_Pills.jpg"
            }, new StorageFile()
            {
                Id = 13,
                DisplayName = "slipknot_logo.jpg",
                FileName = "slipknot_logo.jpg"
            }, new StorageFile()
            {
                Id = 14,
                DisplayName = "Slipknot.jpg",
                FileName = "Slipknot.jpg"
            }, new StorageFile()
            {
                Id = 15,
                DisplayName = "Slipknot_Taylor.jpg",
                FileName = "Slipknot_Taylor.jpg"
            }, new StorageFile()
            {
                Id = 16,
                DisplayName = "All_Hope_is_Gone.jpg",
                FileName = "All_Hope_is_Gone.jpg"
            }, new StorageFile()
            {
                Id = 17,
                DisplayName = "Slipknot_Iowa.jpg",
                FileName = "Slipknot_Iowa.jpg"
            }, new StorageFile()
            {
                Id = 18,
                DisplayName = "KoRnIssues.jpg",
                FileName = "KoRnIssues.jpg"
            }, new StorageFile()
            {
                Id = 19,
                DisplayName = "Korn_logo.jpg",
                FileName = "Korn_logo.jpg"
            }, new StorageFile()
            {
                Id = 20,
                DisplayName = "korn-5820-1078x516-1471045959.jpg",
                FileName = "korn-5820-1078x516-1471045959.jpg"
            }, new StorageFile()
            {
                Id = 21,
                DisplayName = "OB-UL405_ikorn_G_20120906052811.jpg",
                FileName = "OB-UL405_ikorn_G_20120906052811.jpg"
            });

            builder.Entity<Band>().HasData(new Band()
            {
                Id = 1,
                Name = "Rammstein",
                ImageStorageFileId = 1,
                Approved = true,
                CreateDate = DateTime.Now,
                Description = "Industrial metal"
            }, new Band()
            {
                Id = 2,
                Name = "Five Finger Death Punch",
                ImageStorageFileId = 6,
                Approved = true,
                CreateDate = DateTime.Now,
                Description = "Heavy metal"
            }, new Band()
            {
                Id = 3,
                Name = "Andragona",
                Approved = true,
                CreateDate = DateTime.Now,
                ImageStorageFileId = 9,
                Description = "Power metal"
            }, new Band()
            {
                Id = 4,
                Name = "Lindemann",
                Approved = true,
                CreateDate = DateTime.Now,
                Description = "Industrial metal",
                ImageStorageFileId = 11
            }, new Band()
            {
                Id = 5,
                Name = "Slipknot",
                ImageStorageFileId = 13,
                Approved = true,
                CreateDate = DateTime.Now,
                Description = "Heavy metal"
            }, new Band()
            {
                Id = 6,
                Name = "Korn",
                Approved = true,
                CreateDate = DateTime.Now,
                ImageStorageFileId = 19,
                Description = "Nu Metal"
            });

            builder.Entity<SliderImage>().HasData(new SliderImage()
            {
                Id = 1,
                BandId = 1,
                ImageStorageFileId = 2
            }, new SliderImage()
            {
                Id = 2,
                BandId = 1,
                ImageStorageFileId = 3
            }, new SliderImage()
            {
                Id = 3,
                BandId = 2,
                ImageStorageFileId = 7
            }, new SliderImage()
            {
                Id = 4,
                BandId = 5,
                ImageStorageFileId = 14
            }, new SliderImage()
            {
                Id = 5,
                BandId = 5,
                ImageStorageFileId = 15
            }, new SliderImage()
            {
                Id = 6,
                BandId = 6,
                ImageStorageFileId = 20
            }, new SliderImage()
            {
                Id = 7,
                BandId = 6,
                ImageStorageFileId = 21
            });

            builder.Entity<Album>().HasData(new Album()
            {
                Id = 1,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Mutter",
                ImageStorageFileId = 4,
                CategoryId = 1,
                BandId = 1
            }, new Album()
            {
                Id = 2,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Liebe ist für alle da",
                ImageStorageFileId = 5,
                CategoryId = 1,
                BandId = 1
            }, new Album()
            {
                Id = 3,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "GOT YOUR SIX",
                ImageStorageFileId = 8,
                CategoryId = 1,
                BandId = 2
            }, new Album()
            {
                Id = 4,
                Name = "End Of The Prophesied Dawn",
                Approved = true,
                CreateDate = DateTime.Now,
                ImageStorageFileId = 10,
                CategoryId = 1,
                BandId = 3
            }, new Album()
            {
                Id = 5,
                Name = "Skills in Pills",
                Approved = true,
                CreateDate = DateTime.Now,
                ImageStorageFileId = 12,
                CategoryId = 1,
                BandId = 4
            }, new Album()
            {
                Id = 6,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "All hope is gone",
                ImageStorageFileId = 16,
                CategoryId = 1,
                BandId = 5
            },
            new Album()
            {
                Id = 7,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Iowa",
                ImageStorageFileId = 17,
                CategoryId = 1,
                BandId = 5
            }, new Album()
            {
                Id = 8,
                Name = "Issues",
                Approved = true,
                CreateDate = DateTime.Now,
                ImageStorageFileId = 18,
                CategoryId = 1,
                BandId = 6
            });

            builder.Entity<Song>().HasData(new Song()
            {
                Id = 1,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Ich Will",
                YoutubeUrlParam = "EOnSh3QlpbQ",
            },
            new Song()
            {
                Id = 2,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Feuer Frei!",
                YoutubeUrlParam = "ZkW-K5RQdzo"
            },
            new Song()
            {
                Id = 3,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Haifisch",
                YoutubeUrlParam = "GukNjYQZW8s"
            }, new Song()
            {
                Id = 4,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Ich Tu Dir Weh",
                YoutubeUrlParam = "IxuEtL7gxoM"
            }, new Song()
            {
                Id = 5,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Jekyll And Hyde",
                YoutubeUrlParam = "HCBPmxiVMKk"
            }, new Song()
            {
                Id = 6,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Fight for your praise",
                YoutubeUrlParam = "oy7_bkN5eMU"
            }, new Song()
            {
                Id = 7,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Miss You",
                YoutubeUrlParam = "b9-fzLvC-bY"
            }, new Song()
            {
                Id = 8,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Fish On",
                YoutubeUrlParam = "eciZWNdkGqs"
            }, new Song()
            {
                Id = 9,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Praise Abort",
                YoutubeUrlParam = "QWE_M0CX9So"
            },  new Song()
            {
                Id = 10,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Dead Memories",
                YoutubeUrlParam = "9gsAz6S_zSw"
            }, new Song()
            {
                Id = 11,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Psychosocial",
                YoutubeUrlParam = "eIf3b6meriM"
            }, new Song()
            {
                Id = 12,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Snuff",
                YoutubeUrlParam = "LXEKuttVRIo"
            }, new Song()
            {
                Id = 13,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Left Behind",
                YoutubeUrlParam = "D1jQKpse7Yw"
            },
            new Song()
            {
                Id = 14,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Metabolic",
                YoutubeUrlParam = "b3FpfQOxiKA"
            }, new Song()
            {
                Id = 15,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Falling Away from Me",
                YoutubeUrlParam = "2s3iGpDqQpQ"
            },
            new Song()
            {
                Id = 16,
                Approved = true,
                CreateDate = DateTime.Now,
                Name = "Somebody Someone",
                YoutubeUrlParam = "FBEE-t-uyI0"
            });

            builder.Entity<AlbumSong>().HasData(new AlbumSong()
            {
                Id = 1,
                AlbumId = 1,
                SongId = 1
            }, new AlbumSong()
            {
                Id = 2,
                AlbumId = 1,
                SongId = 2
            }, new AlbumSong()
            {
                Id = 3,
                AlbumId = 2,
                SongId = 3
            }, new AlbumSong()
            {
                Id = 4,
                AlbumId = 2,
                SongId = 4
            }, new AlbumSong()
            {
                Id = 5,
                AlbumId = 3,
                SongId = 5
            }, new AlbumSong()
            {
                Id = 6,
                AlbumId = 4,
                SongId = 6
            }, new AlbumSong()
            {
                Id = 7,
                AlbumId = 4,
                SongId = 7
            }, new AlbumSong()
            {
                Id = 8,
                AlbumId = 5,
                SongId = 8
            }, new AlbumSong()
            {
                Id = 9,
                AlbumId = 5,
                SongId = 9
            }, new AlbumSong()
            {
                Id = 10,
                AlbumId = 6,
                SongId = 10
            }, new AlbumSong()
            {
                Id = 11,
                AlbumId = 6,
                SongId = 11
            }, new AlbumSong()
            {
                Id = 12,
                AlbumId = 6,
                SongId = 12
            }, new AlbumSong()
            {
                Id = 13,
                AlbumId = 7,
                SongId = 13
            }, new AlbumSong()
            {
                Id = 14,
                AlbumId = 7,
                SongId = 14
            }, new AlbumSong()
            {
                Id = 15,
                AlbumId = 8,
                SongId = 15
            }, new AlbumSong()
            {
                Id = 16,
                AlbumId = 8,
                SongId = 16
            });

            builder.Entity<BandMember>().HasData(new BandMember()
            {
                Id = 1,
                BandId = 1,
                ArtistId = 1
            }, new BandMember()
            {
                Id = 2,
                BandId = 1,
                ArtistId = 2
            }, new BandMember()
            {
                Id = 3,
                BandId = 1,
                ArtistId = 3
            }, new BandMember()
            {
                Id = 4,
                BandId = 1,
                ArtistId = 4
            }, new BandMember()
            {
                Id = 5,
                BandId = 1,
                ArtistId = 5
            }, new BandMember()
            {
                Id = 6,
                BandId = 1,
                ArtistId = 6
            }, new BandMember()
            {
                Id = 7,
                BandId = 4,
                ArtistId = 1
            }, new BandMember()
            {
                Id = 8,
                BandId = 4,
                ArtistId = 7
            }, new BandMember()
            {
                Id = 9,
                BandId = 5,
                ArtistId = 8
            }, new BandMember()
            {
                Id = 10,
                BandId = 5,
                ArtistId = 9
            }, new BandMember()
            {
                Id = 11,
                BandId = 5,
                ArtistId = 10
            }, new BandMember()
            {
                Id = 12,
                BandId = 5,
                ArtistId = 11
            }, new BandMember()
            {
                Id = 13,
                BandId = 5,
                ArtistId = 12
            }, new BandMember()
            {
                Id = 14,
                BandId = 5,
                ArtistId = 13
            }, new BandMember()
            {
                Id = 15,
                BandId = 5,
                ArtistId = 14
            }, new BandMember()
            {
                Id = 16,
                BandId = 5,
                ArtistId = 15
            }, new BandMember()
            {
                Id = 17,
                BandId = 6,
                ArtistId = 16
            }, new BandMember()
            {
                Id = 18,
                BandId = 6,
                ArtistId = 17
            }, new BandMember()
            {
                Id = 19,
                BandId = 6,
                ArtistId = 18
            }, new BandMember()
            {
                Id = 20,
                BandId = 6,
                ArtistId = 19
            }, new BandMember()
            {
                Id = 21,
                BandId = 6,
                ArtistId = 20
            });

            builder.Entity<User>().HasData(new User()
            {
                Id = 1,
                Email = "user@admin.com",
                FirstName = "User",
                LastName = "Admin",
                PasswordHash = "3CehAkxz/wJvTTB2YA/XoE1DeIHWKviWJrC6RZUmLodcIkupRzW3SEOChHW7QchUDAgX1uVrUVEwAeoz3LMLoA==",
                PasswordSalt = "uLplTPeTOGuBPi4z2vRwhXo5cOaGLLG0NaoiI+fqwLI=",
                UserRole = Shared.Enums.UserRole.Admin
            }, new User()
            {
                Id = 2,
                Email = "user@superuser.com",
                FirstName = "Super",
                LastName = "User",
                PasswordHash = "3CehAkxz/wJvTTB2YA/XoE1DeIHWKviWJrC6RZUmLodcIkupRzW3SEOChHW7QchUDAgX1uVrUVEwAeoz3LMLoA==",
                PasswordSalt = "uLplTPeTOGuBPi4z2vRwhXo5cOaGLLG0NaoiI+fqwLI=",
                UserRole = Shared.Enums.UserRole.SuperUser
            });
        }
    }
}
