using DAL.Context;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Queries
{
    public abstract class AppQuery<T> : EntityFrameworkQuery<T>
    {
        public new MusicLibraryDbContext Context => (MusicLibraryDbContext)base.Context;

        public AppQuery(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
