using System;

namespace WebApi.Models
{
    public class BandViewModel
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? ImageStorageFileId { get; set; }
    }
}
