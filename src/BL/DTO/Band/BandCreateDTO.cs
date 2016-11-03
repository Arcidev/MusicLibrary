
namespace BL.DTO
{
    public class BandCreateDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int? ImageStorageFileId { get; set; }

        public bool Approved { get; set; }
    }
}
