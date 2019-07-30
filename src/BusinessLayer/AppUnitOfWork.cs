using Microsoft.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using DbContextOptions = Riganti.Utils.Infrastructure.EntityFrameworkCore.DbContextOptions;

namespace BusinessLayer
{
    /// <summary>
    /// Application's implementation of UoW
    /// </summary>
    internal class AppUnitOfWork : EntityFrameworkUnitOfWork
    {
        /// <summary>
        /// Creates new instance of UoW
        /// </summary>
        /// <param name="provider">UoW provider</param>
        /// <param name="dbContextFactory">Functor resolving <see cref="DbContext"/></param>
        /// <param name="options">Options for database context</param>
        public AppUnitOfWork(IUnitOfWorkProvider provider, Func<DbContext> dbContextFactory, DbContextOptions options) : base(provider, dbContextFactory, options) { }
    }
}
