
namespace BL.DTO
{
    public class SongInfoDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Removed { get; set; }

        public override bool Equals(object obj)
        {
            var songInfo = obj as SongInfoDTO;
            if (songInfo == null)
                return false;

            return Id == songInfo.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
