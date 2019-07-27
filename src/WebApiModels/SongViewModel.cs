using System;

namespace WebApiModels
{
    public class SongViewModel
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public int? AudioStorageFileId { get; set; }

        public string Name { get; set; }

        public string YoutubeUrlParam { get; set; }
    }
}
