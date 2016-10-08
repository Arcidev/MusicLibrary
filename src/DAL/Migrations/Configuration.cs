using DAL.Context;
using DAL.Entities;
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
            if (!context.Bands.Any())
            {
                var rammstein = new Band()
                {
                    Name = "Rammstein",
                    ImageStorageFile = new StorageFile()
                    {
                        DisplayName = "rammstein_logo.jpg",
                        FileName = "rammstein_logo.jpg"
                    },
                    SliderImages = new List<SliderImage>()
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
                    }
                };

                var slipknot = new Band()
                {
                    Name = "Slipknot",
                    ImageStorageFile = new StorageFile()
                    {
                        DisplayName = "slipknot_logo.jpg",
                        FileName = "slipknot_logo.jpg"
                    },
                    SliderImages = new List<SliderImage>()
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
                    }
                };

                context.Bands.Add(rammstein);
                context.Bands.Add(slipknot);
            }
        }
    }
}
