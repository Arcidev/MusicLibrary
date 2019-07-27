using DataLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Repositories
{
    public class AlbumSongRepository : BaseRepository<AlbumSong, int>
    {
        public AlbumSongRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }

        public void DeleteByAlbumIds(IEnumerable<int> albumIds)
        {
            Context.AlbumSongs.RemoveRange(Context.AlbumSongs.Where(x => albumIds.Contains(x.AlbumId)));
        }
    }
}
