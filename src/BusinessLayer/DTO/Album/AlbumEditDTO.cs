using System.Collections.Generic;

namespace BusinessLayer.DTO
{
    public class AlbumEditDTO : AlbumCreateDTO
    {
        public int Id { get; set; }

        public IEnumerable<int> RemovedSongs { get; set; }
    }
}
