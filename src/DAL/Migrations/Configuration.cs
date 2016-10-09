using DAL.Context;
using DAL.Entities;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MusicLibraryDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MusicLibraryDbContext context)
        {
            if (context.Bands.Any())
                context.Bands.RemoveRange(context.Bands);

            if (context.Artists.Any())
                context.Artists.RemoveRange(context.Artists);

            if (context.StorageFiles.Any())
                context.StorageFiles.RemoveRange(context.StorageFiles);

            if (context.Categories.Any())
                context.Categories.RemoveRange(context.Categories);

            var rock = new Category()
            {
                Name = "Rock",
                Name_csCZ = "Rock",
                Name_skSK = "Rock"
            };

            var metal = new Category()
            {
                Name = "Metal",
                Name_csCZ = "Metal",
                Name_skSK = "Metal"
            };
            context.Categories.AddOrUpdate(new[] { rock, metal });

            var lindemann = new Artist()
            {
                FirstName = "Till",
                LastName = "Lindemann",
                Approved = true
            };

            context.Bands.AddRange(new []
            {
                new Band()
                {
                    Name = "Rammstein",
                    ImageStorageFile = new StorageFile()
                    {
                        DisplayName = "rammstein_logo.jpg",
                        FileName = "rammstein_logo.jpg"
                    },
                    SliderImages = new []
                    {
                        new SliderImage()
                        {
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "rammstein_guitar.jpg",
                                FileName = "rammstein_guitar.jpg"
                            }
                        },
                        new SliderImage()
                        {
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "Rammstein-Till-Lindemann-fire.jpg",
                                FileName = "Rammstein-Till-Lindemann-fire.jpg"
                            }
                        }
                    },
                    Approved = true,
                    Albums = new []
                    {
                        new Album()
                        {
                            Approved = true,
                            CreateDate = DateTime.Now,
                            Name = "Mutter",
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "Mutter.jpg",
                                FileName = "Mutter.jpg"
                            },
                            Category = metal,
                            AlbumSongs = new []
                            {
                                new AlbumSong()
                                {
                                    Song = new Song()
                                    {
                                        Approved = true,
                                        CreateDate = DateTime.Now,
                                        Name = "Ich Will",
                                        YoutubeUrlParam = "EOnSh3QlpbQ"
                                    }
                                },
                                new AlbumSong()
                                {
                                    Song = new Song()
                                    {
                                        Approved = true,
                                        CreateDate = DateTime.Now,
                                        Name = "Feuer Frei!",
                                        YoutubeUrlParam = "ZkW-K5RQdzo"
                                    }
                                }
                            }
                        },
                        new Album()
                        {
                            Approved = true,
                            CreateDate = DateTime.Now,
                            Name = "Liebe ist f�r alle da",
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "Cover_lifad.png",
                                FileName = "Cover_lifad.png"
                            },
                            Category = metal,
                            AlbumSongs = new []
                            {
                                new AlbumSong()
                                {
                                    Song = new Song()
                                    {
                                        Approved = true,
                                        CreateDate = DateTime.Now,
                                        Name = "Haifisch",
                                        YoutubeUrlParam = "GukNjYQZW8s"
                                    }
                                },
                                new AlbumSong()
                                {
                                    Song = new Song()
                                    {
                                        Approved = true,
                                        CreateDate = DateTime.Now,
                                        Name = "Ich Tu Dir Weh",
                                        YoutubeUrlParam = "IxuEtL7gxoM"
                                    }
                                }
                            }
                        }
                    },
                    CreateDate = DateTime.Now,
                    BandMembers = new []
                    {
                        new BandMember() { Artist = lindemann },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Richard",
                                LastName = "Kruspe",
                                Approved = true
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Paul",
                                LastName = "Landers",
                                Approved = true
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Christian",
                                LastName = "Lorenz",
                                Approved = true
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Oliver",
                                LastName = "Riedel",
                                Approved = true
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Christoph",
                                LastName = "Schneider",
                                Approved = true
                            }
                        }
                    }
                },
                new Band()
                {
                    Name = "Lindemann",
                    Approved = true,
                    CreateDate = DateTime.Now,
                    Albums = new []
                    {
                        new Album()
                        {
                            Name = "Skills in Pills",
                            Approved = true,
                            CreateDate = DateTime.Now,
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "Lindemann_Skills_in_Pills.jpg",
                                FileName = "Lindemann_Skills_in_Pills.jpg"
                            },
                            Category = metal,
                            AlbumSongs = new []
                            {
                                new AlbumSong()
                                {
                                    Song = new Song()
                                    {
                                        Approved = true,
                                        CreateDate = DateTime.Now,
                                        Name = "Fish On",
                                        YoutubeUrlParam = "eciZWNdkGqs"
                                    }
                                },
                                new AlbumSong()
                                {
                                    Song = new Song()
                                    {
                                        Approved = true,
                                        CreateDate = DateTime.Now,
                                        Name = "Praise Abort",
                                        YoutubeUrlParam = "QWE_M0CX9So"
                                    }
                                }
                            }
                        }
                    },
                    ImageStorageFile = new StorageFile()
                    {
                        DisplayName = "Lindemann_logo.jpg",
                        FileName = "Lindemann_logo.jpg"
                    },
                    BandMembers = new []
                    {
                        new BandMember() { Artist = lindemann },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                Approved = true,
                                FirstName = "Peter",
                                LastName = "T�gtgren"
                            }
                        }
                    }
                },
                new Band()
                {
                    Name = "Slipknot",
                    ImageStorageFile = new StorageFile()
                    {
                        DisplayName = "slipknot_logo.jpg",
                        FileName = "slipknot_logo.jpg"
                    },
                    SliderImages = new []
                    {
                        new SliderImage()
                        {
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "Slipknot.jpg",
                                FileName = "Slipknot.jpg"
                            }
                        },
                        new SliderImage()
                        {
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "Slipknot_Taylor.jpg",
                                FileName = "Slipknot_Taylor.jpg"
                            }
                        }
                    },
                    Approved = true,
                    CreateDate = DateTime.Now,
                    Albums = new []
                    {
                        new Album()
                        {
                            Approved = true,
                            CreateDate = DateTime.Now,
                            Name = "All hope is gone",
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "All_Hope_is_Gone.jpg",
                                FileName = "All_Hope_is_Gone.jpg"
                            },
                            Category = metal,
                            AlbumSongs = new []
                            {
                                new AlbumSong()
                                {
                                    Song = new Song()
                                    {
                                        Approved = true,
                                        CreateDate = DateTime.Now,
                                        Name = "Dead Memories",
                                        YoutubeUrlParam = "9gsAz6S_zSw"
                                    }
                                },
                                new AlbumSong()
                                {
                                    Song = new Song()
                                    {
                                        Approved = true,
                                        CreateDate = DateTime.Now,
                                        Name = "Psychosocial",
                                        YoutubeUrlParam = "eIf3b6meriM"
                                    }
                                }
                            }
                        },
                        new Album()
                        {
                            Approved = true,
                            CreateDate = DateTime.Now,
                            Name = "Iowa",
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "Slipknot_Iowa.jpg",
                                FileName = "Slipknot_Iowa.jpg"
                            },
                            Category = metal,
                            AlbumSongs = new []
                            {
                                new AlbumSong()
                                {
                                    Song = new Song()
                                    {
                                        Approved = true,
                                        CreateDate = DateTime.Now,
                                        Name = "Left Behind",
                                        YoutubeUrlParam = "D1jQKpse7Yw"
                                    }
                                },
                                new AlbumSong()
                                {
                                    Song = new Song()
                                    {
                                        Approved = true,
                                        CreateDate = DateTime.Now,
                                        Name = "Metabolic",
                                        YoutubeUrlParam = "b3FpfQOxiKA"
                                    }
                                }
                            }
                        }
                    },
                    BandMembers = new []
                    {
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Corey",
                                LastName = "Taylor",
                                Approved = true
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Mick",
                                LastName = "Thomson",
                                Approved = true
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Sid",
                                LastName = "Wilson",
                                Approved = true
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Jim",
                                LastName = "Root",
                                Approved = true
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Craig",
                                LastName = "Jones",
                                Approved = true
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Shawn",
                                LastName = "Crahan",
                                Approved = true
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Alessandro",
                                LastName = "Venturella",
                                Approved = true
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                FirstName = "Jay",
                                LastName = "Weinberg",
                                Approved = true
                            }
                        }
                    }
                },
                new Band()
                {
                    Name = "Korn",
                    Approved = true,
                    CreateDate = DateTime.Now,
                    Albums = new []
                    {
                        new Album()
                        {
                            Name = "Issues",
                            Approved = true,
                            CreateDate = DateTime.Now,
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "KoRnIssues.jpg",
                                FileName = "KoRnIssues.jpg"
                            },
                            Category = metal,
                            AlbumSongs = new []
                            {
                                new AlbumSong()
                                {
                                    Song = new Song()
                                    {
                                        Approved = true,
                                        CreateDate = DateTime.Now,
                                        Name = "Falling Away from Me",
                                        YoutubeUrlParam = "2s3iGpDqQpQ"
                                    }
                                },
                                new AlbumSong()
                                {
                                    Song = new Song()
                                    {
                                        Approved = true,
                                        CreateDate = DateTime.Now,
                                        Name = "Somebody Someone",
                                        YoutubeUrlParam = "FBEE-t-uyI0"
                                    }
                                }
                            }
                        }
                    },
                    ImageStorageFile = new StorageFile()
                    {
                        DisplayName = "Korn_logo.jpg",
                        FileName = "Korn_logo.jpg"
                    },
                    BandMembers = new []
                    {
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                Approved = true,
                                FirstName = "Jonathan",
                                LastName = "Davis"
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                Approved = true,
                                FirstName = "James",
                                LastName = "Shaffer"
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                Approved = true,
                                FirstName = "Reginald",
                                LastName = "Arvizu"
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                Approved = true,
                                FirstName = "Brian",
                                LastName = "Welch"
                            }
                        },
                        new BandMember()
                        {
                            Artist = new Artist()
                            {
                                Approved = true,
                                FirstName = "Ray",
                                LastName = "Luzier"
                            }
                        }
                    },
                    SliderImages = new []
                    {
                        new SliderImage()
                        {
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "korn-5820-1078x516-1471045959.jpg",
                                FileName = "korn-5820-1078x516-1471045959.jpg"
                            }
                        },
                        new SliderImage()
                        {
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "OB-UL405_ikorn_G_20120906052811.jpg",
                                FileName = "OB-UL405_ikorn_G_20120906052811.jpg"
                            }
                        }
                    }
                }
            });
        }
    }
}
