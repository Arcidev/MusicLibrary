using DataLayer.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BusinessLayer.Repositories
{
    public class AlbumRepository : BaseRepository<Album, int>
    {
        public AlbumRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }
    }
}
