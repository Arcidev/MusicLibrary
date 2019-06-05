using BL.Installers;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MusicLibrary.Installers;
using Owin;

namespace MusicLibrary.AppStart
{
    internal static class WindsorBootstrap
    {
        internal static IWindsorContainer Container { get; private set; }

        internal static void SetupContainer(IAppBuilder app)
        {
            Container = new WindsorContainer();
            Container.AddFacility<TypedFactoryFacility>();
            Container.Install(FromAssembly.InThisApplication());
            Container.Register(Component.For<IAppBuilder>().Instance(app));

            Container.Install(new AutoMapperInstaller());
            Container.Install(new ServicesInstaller());
            Container.Install(new FacadesInstaller());
            Container.Install(new WebApiInstaller());
        }

        internal static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
