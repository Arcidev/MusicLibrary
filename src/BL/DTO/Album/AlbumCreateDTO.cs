
namespace BL.DTO
{
    public class AlbumCreateDTO
    {
        public string Name { get; set; }

        public int BandId { get; set; }

        public bool Approved { get; set; }

        public int CategoryId { get; set; }
    }
}
