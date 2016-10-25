using BL.Queries;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DAL.Context;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System;
using System.Data.Entity;

namespace BL.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Func<DbContext>>().Instance(() => new MusicLibraryDbContext()).LifestyleTransient(),
                Component.For<IUnitOfWorkProvider>().ImplementedBy<AppUnitOfWorkProvider>().LifestyleSingleton(),
                Component.For<IUnitOfWorkRegistry>().Instance(new HttpContextUnitOfWorkRegistry(new AsyncLocalUnitOfWorkRegistry())).LifestyleSingleton(),

                Classes.FromAssemblyContaining<AppUnitOfWorkProvider>().BasedOn(typeof(AppQuery<>)).LifestyleTransient(),
                Classes.FromAssemblyContaining<AppUnitOfWorkProvider>().BasedOn(typeof(IRepository<,>)).LifestyleTransient(),

                Component.For(typeof(IRepository<,>)).ImplementedBy(typeof(EntityFrameworkRepository<,>)).LifestyleTransient()
          );
        }
    }
}