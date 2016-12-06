﻿using System;

namespace BL.DTO
{
    public class SongDTO : SongCreateDTO
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public int? AudioStorageFileId { get; set; }

        public StorageFileDTO AudioStorageFile { get; set; }
    }
}