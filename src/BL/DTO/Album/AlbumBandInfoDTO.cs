
namespace BL.DTO
{
    public class AlbumBandInfoDTO
    {
        public int AlbumId { get; set; }

        public string AlbumName { get; set; }

        public string BandName { get; set; }

        public bool Removed { get; set; }

        public string BandAlbumName { get { return $"{BandName} - {AlbumName}"; } }

        public override bool Equals(object obj)
        {
            var info = obj as AlbumBandInfoDTO;
            if (info == null)
                return false;

            return AlbumId == info.AlbumId;
        }

        public override int GetHashCode()
        {
            return AlbumId.GetHashCode();
        }
    }
}
