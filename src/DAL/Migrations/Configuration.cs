using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
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
            {
                var toRemove = context.Bands.Where(x => x.Name == "Rammstein" || x.Name == "Slipknot");
                context.Bands.RemoveRange(toRemove);
            }

            if (context.StorageFiles.Any())
            {
                var toRemove = context.StorageFiles
                    .Where(x => x.FileName == "rammstein_logo.jpg" ||
                                x.FileName == "rammstein_guitar.jpg" ||
                                x.FileName == "Rammstein-Till-Lindemann-fire.jpg" ||
                                x.FileName == "Mutter.jpg" ||
                                x.FileName == "Cover_lifad.png" ||
                                x.FileName == "slipknot_logo.jpg" ||
                                x.FileName == "Slipknot.jpg" ||
                                x.FileName == "Slipknot_Taylor.jpg" ||
                                x.FileName == "All_Hope_is_Gone.jpg" ||
                                x.FileName == "Slipknot_Iowa.jpg");
                context.StorageFiles.RemoveRange(toRemove);
            }

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
                            Category = metal
                        },
                        new Album()
                        {
                            Approved = true,
                            CreateDate = DateTime.Now,
                            Name = "Liebe ist fur alle da",
                            ImageStorageFile = new StorageFile()
                            {
                                DisplayName = "Cover_lifad.png",
                                FileName = "Cover_lifad.png"
                            },
                            Category = metal
                        }
                    },
                    CreateDate = DateTime.Now
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
                            Category = metal
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
                            Category = metal
                        }
                    }
                }
            });
        }
    }
}
