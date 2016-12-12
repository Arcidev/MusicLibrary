using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public class AlbumSongRepository : BaseRepository<AlbumSong, int>
    {
        public AlbumSongRepository(IUnitOfWorkProvider provider) : base(provider) { }

        public void DeleteByAlbumIds(IEnumerable<int> albumIds)
        {
            Context.AlbumSongs.RemoveRange(Context.AlbumSongs.Where(x => albumIds.Contains(x.AlbumId)));
        }
    }
}
