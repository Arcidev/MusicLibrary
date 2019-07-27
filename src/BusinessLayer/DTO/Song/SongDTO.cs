using System;

namespace BusinessLayer.DTO
{
    public class SongDTO : SongBaseDTO
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public int? AudioStorageFileId { get; set; }

        public StorageFileDTO AudioStorageFile { get; set; }
    }
}
