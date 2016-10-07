using System;
using System.Data.Entity;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using DAL.Context;

namespace BL
{
    public class AppUnitOfWork : EntityFrameworkUnitOfWork
    {
        public new MusicLibraryDbContext Context => (MusicLibraryDbContext)base.Context;

        public AppUnitOfWork(IUnitOfWorkProvider provider, Func<DbContext> dbContextFactory, DbContextOptions options) : base(provider, dbContextFactory, options) { }
    }
}
