using DataLayer.Context;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;

namespace BusinessLayer.Repositories
{
    public abstract class BaseRepository<TEntity, TKey> : EntityFrameworkRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        protected new MusicLibraryDbContext Context => (MusicLibraryDbContext)base.Context;

        public BaseRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }
    }
}
