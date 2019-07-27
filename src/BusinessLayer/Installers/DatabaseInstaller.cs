using DataLayer.Installers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BusinessLayer.Installers
{
    /// <summary>
    /// Database install helper
    /// </summary>
    public static class DatabaseInstaller
    {
        /// <summary>
        /// Extends <see cref="IServiceCollection"/> to allow chained database installation
        /// </summary>
        /// <param name="services">DI container</param>
        /// <param name="connectionString">Db connection string</param>
        /// <returns>Passed DI container to allow chaining</returns>
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, string connectionString)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            return DbInstaller.ConfigureDatabase(services, connectionString);
        }
    }
}
