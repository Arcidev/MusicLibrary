using System;

namespace BL.DTO
{
    public class BandDTO : BandCreateDTO
    {
        public DateTime CreateDate { get; set; }

        public StorageFileDTO ImageStorageFile { get; set; }
    }
}
