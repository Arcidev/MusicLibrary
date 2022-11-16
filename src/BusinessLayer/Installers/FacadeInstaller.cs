using BusinessLayer.Facades;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace BusinessLayer.Installers
{
    /// <summary>
    /// Facade install helper
    /// </summary>
    public static class FacadeInstaller
    {
        /// <summary>
        /// Extends <see cref="IServiceCollection"/> to allow chained facade installation
        /// </summary>
        /// <param name="services">DI container</param>
        /// <returns>Passed DI container to allow chaining</returns>
        public static IServiceCollection ConfigureFacades(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var baseFacade = typeof(BaseFacade);
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract && baseFacade.IsAssignableFrom(t)))
                services.AddScoped(type);

            services.AddScoped(provider => new Lazy<StorageFileFacade>(() => provider.GetRequiredService<StorageFileFacade>()));
            return services;
        }
    }
}
