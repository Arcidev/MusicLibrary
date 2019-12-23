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
            MapperInstaller.ConfigureMapper();

            services = new ServiceCollection()
                .AddDbContext<MusicLibraryDbContext>(options => options.UseInMemoryDatabase("TestDB"), ServiceLifetime.Transient, ServiceLifetime.Transient)
                .AddTransient<Func<DbContext>>(provider => () => provider.GetService<MusicLibraryDbContext>())
                .ConfigureServices()
                .ConfigureFacades()
                .BuildServiceProvider();
        }
    }
}
