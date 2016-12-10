using System.Collections.Generic;

namespace BL.DTO
{
    public class AlbumEditDTO : AlbumCreateDTO
    {
        public int Id { get; set; }

        public IEnumerable<int> RemovedSongs { get; set; }
    }
}
