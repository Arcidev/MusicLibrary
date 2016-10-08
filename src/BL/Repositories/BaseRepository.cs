using DAL.Context;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class BaseRepository<TEntity, TKey> : EntityFrameworkRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        protected new MusicLibraryDbContext Context => (MusicLibraryDbContext)base.Context;

        public BaseRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
