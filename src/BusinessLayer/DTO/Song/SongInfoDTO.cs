
namespace BusinessLayer.DTO
{
    public class SongInfoDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Approved { get; set; }

        public bool Removed { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is SongInfoDTO songInfo))
                return false;

            return Id == songInfo.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
