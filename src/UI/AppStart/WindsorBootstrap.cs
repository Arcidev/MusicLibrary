using BL.Installers;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Owin;

namespace MusicLibrary.AppStart
{
    internal static class WindsorBootstrap
    {
        internal static WindsorContainer container;

        internal static void SetupContainer(IAppBuilder app)
        {
            container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();
            container.Install(FromAssembly.InThisApplication());
            container.Register(Component.For<IAppBuilder>().Instance(app));

            container.Install(new FacadesInstaller());
            container.Install(new ServicesInstaller());
        }

        internal static T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
