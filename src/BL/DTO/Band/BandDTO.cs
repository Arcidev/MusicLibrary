using System;
using System.Collections.Generic;

namespace BL.DTO
{
    public class BandDTO : BandCreateDTO
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public int? ImageStorageFileId { get; set; }

        public StorageFileDTO ImageStorageFile { get; set; }

        public IEnumerable<AlbumDTO> Albums { get; set; }
    }
}
