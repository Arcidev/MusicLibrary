
namespace BL.DTO
{
    public class BandDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ImageStorageFileId { get; set; }

        public bool Approved { get; set; }

        public StorageFileDTO ImageStorageFile { get; set; }
    }
}
