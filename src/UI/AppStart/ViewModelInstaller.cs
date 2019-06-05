using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;

namespace MusicLibrary.AppStart
{
    public class ViewModelInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<IDotvvmViewModel>()
                    .LifestyleTransient(),

                Classes.FromThisAssembly()
                    .BasedOn<IDotvvmPresenter>()
                    .LifestyleTransient()
            );
        }
    }
}
