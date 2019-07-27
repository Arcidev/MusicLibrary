using System.Collections.Generic;

namespace BusinessLayer.DTO
{
    public class AlbumCreateDTO : AlbumBaseDTO
    {
        public IEnumerable<int> AddedSongs { get; set; }
    }
}
