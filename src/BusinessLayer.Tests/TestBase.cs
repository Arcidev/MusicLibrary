using BusinessLayer.Installers;
using DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BL.Tests
{
    public abstract class TestBase
    {
        protected static readonly IServiceProvider services;

        static TestBase()
        {
            services = new ServiceCollection()
                .AddDbContext<MusicLibraryDbContext>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MusicLibraryTests"), ServiceLifetime.Transient, ServiceLifetime.Transient)
                .AddTransient<Func<DbContext>>(provider => () => provider.GetService<MusicLibraryDbContext>())
                .ConfigureAutoMapper()
                .ConfigureServices()
                .ConfigureFacades()
                .BuildServiceProvider();

            InitTestDatabase();
        }

        private static void InitTestDatabase()
        {
            using var context = services.GetRequiredService<MusicLibraryDbContext>();
            context.Database.EnsureCreated();

            context.Categories.RemoveRange(context.Categories);
            context.Bands.RemoveRange(context.Bands);
            context.Songs.RemoveRange(context.Songs);
            context.Users.RemoveRange(context.Users);

            context.SaveChanges();
        }
    }
}
