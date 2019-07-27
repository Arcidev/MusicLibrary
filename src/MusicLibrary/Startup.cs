using BusinessLayer.Installers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using DotVVM.Framework.Routing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DotVVM.Framework.Security;
using DotVVM.Framework.Runtime.Caching;
using DotVVM.Framework.Hosting.AspNetCore.Runtime.Caching;
using DotVVM.Framework.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Mvc;

namespace MusicLibrary
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt => opt.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddDataProtection();
            services.AddAuthorization().AddWebEncoders().ConfigureDatabase(configuration.GetConnectionString("MusicLibrary"));
            AddDotVVM(services);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // use DotVVM
            var dotvvmConfiguration = app.UseAuthentication().UseDotVVM<DotvvmStartup>(env.ContentRootPath);
            dotvvmConfiguration.AssertConfigurationIsValid();

            app.UseMvc();

            // use static files
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(env.WebRootPath)
            });
        }

        /// <summary>
        /// Hack used instead of AddDotvvm<DotvvmStartup> as it's throw errors on missing method to init dotvvm until it gets fixed
        /// </summary>
        /// <param name="services"></param>
        private static void AddDotVVM(IServiceCollection services)
        {
            services.AddAuthorization().AddMemoryCache();
            DotvvmServiceCollectionExtensions.RegisterDotVVMServices(services);

            services.TryAddSingleton<ICsrfProtector, DefaultCsrfProtector>();
            services.TryAddSingleton<ICookieManager, ChunkingCookieManager>();
            services.TryAddSingleton<IDotvvmCacheAdapter, AspNetCoreDotvvmCacheAdapter>();
            services.TryAddSingleton<IViewModelProtector, DefaultViewModelProtector>();
            services.TryAddSingleton<IEnvironmentNameProvider, DotvvmEnvironmentNameProvider>();

            var type = typeof(DefaultCsrfProtector).Assembly.GetType("DotVVM.Framework.Hosting.DotvvmRequestContextStorage");
            services.TryAddScoped(type, _ => Activator.CreateInstance(type));
            services.TryAddScoped(s => type.GetProperty("Context").GetValue(s.GetRequiredService(type), null));

            var configurator = new DotvvmStartup();
            var dotvvmServices = new DotvvmServiceCollection(services);
            configurator.ConfigureServices(dotvvmServices);
        }
    }
}
