using Microsoft.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using DbContextOptions = Riganti.Utils.Infrastructure.EntityFrameworkCore.DbContextOptions;

namespace BusinessLayer
{
    /// <summary>
    /// Application's implementation of UoW provider
    /// </summary>
    internal class AppUnitOfWorkProvider : EntityFrameworkUnitOfWorkProvider
    {
        /// <summary>
        /// Creates new instance of UoW provider
        /// </summary>
        /// <param name="registry">UoW registry</param>
        /// <param name="dbContextFactory">Functor resolving <see cref="DbContext"/></param>
        public AppUnitOfWorkProvider(IUnitOfWorkRegistry registry, Func<DbContext> dbContextFactory) : base(registry, dbContextFactory) { }

        /// <inheritdoc />
        protected override EntityFrameworkUnitOfWork<DbContext> CreateUnitOfWork(Func<DbContext> dbContextFactory, DbContextOptions options)
        {
            return new AppUnitOfWork(this, dbContextFactory, options);
        }
    }
}
