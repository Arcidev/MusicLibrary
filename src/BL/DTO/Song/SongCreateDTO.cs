using System.Collections.Generic;

namespace BL.DTO
{
    public class SongCreateDTO : SongBaseDTO
    {
        public IEnumerable<int> AddedAlbums { get; set; }
    }
}
