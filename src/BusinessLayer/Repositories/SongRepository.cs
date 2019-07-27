using DataLayer.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BusinessLayer.Repositories
{
    public class SongRepository : BaseRepository<Song, int>
    {
        public SongRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }
    }
}
