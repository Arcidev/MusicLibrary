using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class AlbumReview : Review
    {
        public int AlbumId { get; set; }

        [ForeignKey(nameof(AlbumId))]
        public virtual Album Album { get; set; }
    }
}
