using System;
using System.Collections.Generic;

namespace BL.DTO
{
    public class AlbumDTO : AlbumCreateDTO
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public CategoryDTO Category { get; set; }

        public BandDTO Band { get; set; }

        public StorageFileDTO ImageStorageFile { get; set; }

        public IList<SongDTO> Songs { get; set; }

        public AlbumDTO()
        {
            Songs = new List<SongDTO>();
        }
    }
}
