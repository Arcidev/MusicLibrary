using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System;
using System.Data.Entity;

namespace BL
{
    public class AppUnitOfWorkProvider : EntityFrameworkUnitOfWorkProvider
    {
        public AppUnitOfWorkProvider(IUnitOfWorkRegistry registry, Func<DbContext> dbContextFactory) : base(registry, dbContextFactory) { }

        protected override EntityFrameworkUnitOfWork<DbContext> CreateUnitOfWork(Func<DbContext> dbContextFactory, DbContextOptions options)
        {
            return new AppUnitOfWork(this, dbContextFactory, options);
        }
    }
}
