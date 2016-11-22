using BL.Configuration;
using Castle.Windsor;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel.Serialization;
using DotVVM.Framework.Storage;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.StaticFiles;
using MusicLibrary.AppStart;
using Owin;
using System;
using System.IO;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Routing;

[assembly: OwinStartup(typeof(MusicLibrary.Startup))]
namespace MusicLibrary
{
    public class Startup
    {
        public string ApplicationPhysicalPath { get; set; } = HostingEnvironment.ApplicationPhysicalPath;

        public void Configuration(IAppBuilder app)
        {
            var applicationPhysicalPath = HostingEnvironment.ApplicationPhysicalPath;

            // automapper
            AutoMapper.Init();

            // IoC/DI
            WindsorBootstrap.SetupContainer(app);

            // register WebApi Factory
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorWebApiComposer(WindsorBootstrap.container));
            GlobalConfiguration.Configure(config =>
            {
                config.MapHttpAttributeRoutes();
                config.Formatters.Remove(config.Formatters.XmlFormatter);
            });

            // routes registration
            RouteTable.Routes.MapHttpRoute("WebApiControllers", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });

            // use cookie authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/login"),
                Provider = new CookieAuthenticationProvider()
                {
                    OnApplyRedirect = context =>
                    {
                        DotvvmAuthenticationHelper.ApplyRedirectResponse(context.OwinContext, context.RedirectUri);
                    }
                }
            });

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
