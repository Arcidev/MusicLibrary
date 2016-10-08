using System;

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
    }
}
