
namespace BL.DTO
{
    public class AlbumCreateDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ImageStorageFileId { get; set; }

        public int BandId { get; set; }

        public bool Approved { get; set; }

        public int CategoryId { get; set; }
    }
}
