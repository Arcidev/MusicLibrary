
namespace BusinessLayer.DTO
{
    public class AlbumBandInfoDTO
    {
        public int AlbumId { get; set; }

        public string AlbumName { get; set; }

        public string BandName { get; set; }

        public bool Removed { get; set; }

        public string BandAlbumName => $"{BandName} - {AlbumName}";

        public override bool Equals(object obj)
        {
            if (!(obj is AlbumBandInfoDTO info))
                return false;

            return AlbumId == info.AlbumId;
        }

        public override int GetHashCode() => AlbumId.GetHashCode();
    }
}
