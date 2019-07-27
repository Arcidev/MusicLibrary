using System.Collections.Generic;

namespace BusinessLayer.DTO
{
    public class SongEditDTO : SongCreateDTO
    {
        public int Id { get; set; }

        public IEnumerable<int> RemovedAlbums { get; set; }
    }
}
