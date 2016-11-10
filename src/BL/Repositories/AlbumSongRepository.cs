using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Repositories
{
    public class AlbumSongRepository : BaseRepository<AlbumSong, int>
    {
        public AlbumSongRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
