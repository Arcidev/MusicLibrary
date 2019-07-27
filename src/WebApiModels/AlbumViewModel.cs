using System;

namespace WebApiModels
{
    public class AlbumViewModel
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Name { get; set; }

        public int? ImageStorageFileId { get; set; }

        public int BandId { get; set; }

        public int CategoryId { get; set; }
    }
}
