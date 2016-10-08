using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class SongReview : Review
    {
        public int SongId { get; set; }

        [ForeignKey(nameof(SongId))]
        public virtual Song Song { get; set; }
    }
}
