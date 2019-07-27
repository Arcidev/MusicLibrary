using System.Collections.Generic;

namespace BusinessLayer.DTO
{
    public class SongCreateDTO : SongBaseDTO
    {
        public IEnumerable<int> AddedAlbums { get; set; }
    }
}
