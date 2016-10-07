using DotVVM.Framework.Configuration;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel.Serialization;
using DotVVM.Framework.Storage;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using MusicLibrary.AppStart;
using Owin;
using System;
using System.IO;
using System.Web.Hosting;
using BL.Configuration;

[assembly: OwinStartup(typeof(MusicLibrary.Startup))]
namespace MusicLibrary
{
    public class Startup
    {
        public string ApplicationPhysicalPath { get; set; } = HostingEnvironment.ApplicationPhysicalPath;

        public void Configuration(IAppBuilder app)
        {
            var applicationPhysicalPath = HostingEnvironment.ApplicationPhysicalPath;

            // Automapper
            AutoMapper.Init();

            //IoC/DI
            WindsorBootstrap.SetupContainer(app);

            // use DotVVM
            var dotvvmConfiguration = InitDotvvm(app);
#if DEBUG
            dotvvmConfiguration.Debug = true;
#endif

            // use static files
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileSystem = new PhysicalFileSystem(ApplicationPhysicalPath)
            });
        }

        private DotvvmConfiguration InitDotvvm(IAppBuilder app)
        {
            var config = app.UseDotVVM<DotvvmStartup>(ApplicationPhysicalPath);

            config.ServiceLocator.RegisterSingleton<IViewModelLoader>(() => new WindsorViewModelLoader(WindsorBootstrap.container));
            config.ServiceLocator.RegisterSingleton<IUploadedFileStorage>(() => new FileSystemUploadedFileStorage(Path.Combine(ApplicationPhysicalPath, "Temp"), TimeSpan.FromMinutes(30)));

            return config;
        }

    }
}
