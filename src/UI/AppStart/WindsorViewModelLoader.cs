using System;
using Castle.Windsor;
using DotVVM.Framework.ViewModel.Serialization;

namespace MusicLibrary.AppStart
{
    public class WindsorViewModelLoader : DefaultViewModelLoader
    {
        private readonly WindsorContainer container;

        public WindsorViewModelLoader(WindsorContainer container, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.container = container;
        }

        protected override object CreateViewModelInstance(Type viewModelType)
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