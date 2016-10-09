using System;

namespace BL.DTO
{
    public class SongDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Approved { get; set; }

        public DateTime CreateDate { get; set; }

        public int? AudioStorageFileId { get; set; }

        public string YoutubeUrlParam { get; set; }

        public StorageFileDTO AudioStorageFile { get; set; }
    }
}
