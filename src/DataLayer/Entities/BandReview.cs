using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class BandReview : Review
    {
        public int BandId { get; set; }

        [ForeignKey(nameof(BandId))]
        public virtual Band Band { get; set; }
    }
}
