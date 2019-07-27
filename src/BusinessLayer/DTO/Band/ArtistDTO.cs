
namespace BusinessLayer.DTO
{
    public class ArtistDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public bool Approved { get; set; }
    }
}
