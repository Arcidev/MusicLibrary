
namespace BL.DTO
{
    public class UserAlbumDTO
    {
        public int AlbumId { get; set; }

        public string AlbumName { get; set; }

        public CategoryDTO Category { get; set; }

        public string BandName { get; set; }

        public bool HasInCollection { get; set; }
    }
}
