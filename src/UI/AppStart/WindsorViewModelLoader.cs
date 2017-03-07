using System;
using Castle.Windsor;
using DotVVM.Framework.ViewModel.Serialization;
using DotVVM.Framework.Hosting;

namespace MusicLibrary.AppStart
{
    public class WindsorViewModelLoader : DefaultViewModelLoader
    {
        private readonly WindsorContainer container;

        public WindsorViewModelLoader(WindsorContainer container)
        {
            this.container = container;
        }

        protected override object CreateViewModelInstance(Type viewModelType, IDotvvmRequestContext context)
        {
            return container.Resolve(viewModelType);
        }

        public override void DisposeViewModel(object instance)
        {
            container.Release(instance);
            base.DisposeViewModel(instance);
        }
    }
}