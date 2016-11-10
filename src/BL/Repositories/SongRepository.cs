using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Repositories
{
    public class SongRepository : BaseRepository<Song, int>
    {
        public SongRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
