using BL.Facades;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace BL.Installers
{
    public class FacadesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>(),
                Classes.FromThisAssembly().BasedOn<BaseFacade>().LifestyleTransient()
            );
        }
    }
}
