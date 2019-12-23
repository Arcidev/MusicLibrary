using DataLayer.Context;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;

namespace BusinessLayer.Queries
{
    public abstract class AppQuery<T> : EntityFrameworkQuery<T>
    {
        public new MusicLibraryDbContext Context => (MusicLibraryDbContext)base.Context;

        public AppQuery(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
