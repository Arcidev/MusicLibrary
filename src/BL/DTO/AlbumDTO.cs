using System;
using System.Collections.Generic;

namespace BL.DTO
{
    public class AlbumDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ImageStorageFileId { get; set; }

        public int BandId { get; set; }

        public bool Approved { get; set; }

        public DateTime CreateDate { get; set; }

        public int CateogoryId { get; set; }

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
