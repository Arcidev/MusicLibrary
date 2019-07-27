
namespace BusinessLayer.DTO
{
    public class SliderImageDTO : SliderImageEditDTO
    {
        public int Id { get; set; }

        public int ImageStorageFileId { get; set; }

        public StorageFileDTO ImageStorageFile { get; set; }
    }
}
