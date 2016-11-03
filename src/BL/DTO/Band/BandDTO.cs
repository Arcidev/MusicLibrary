using System;

namespace BL.DTO
{
    public class BandDTO : BandCreateDTO
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public StorageFileDTO ImageStorageFile { get; set; }
    }
}
