using System.Collections.Generic;

namespace BL.DTO
{
    public class AlbumCreateDTO : AlbumBaseDTO
    {
        public IEnumerable<int> AddedSongs { get; set; }
    }
}
