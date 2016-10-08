
namespace BL.DTO
{
    public class SliderImageDTO
    {
        public int Id { get; set; }

        public int BandId { get; set; }

        public int ImageStorageFileId { get; set; }

        public StorageFileDTO ImageStorageFile { get; set; }
    }
}
