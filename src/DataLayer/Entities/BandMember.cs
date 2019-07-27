using Riganti.Utils.Infrastructure.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class BandMember : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public int ArtistId { get; set; }

        public int BandId { get; set; }

        [ForeignKey(nameof(ArtistId))]
        public virtual Artist Artist { get; set; }

        [ForeignKey(nameof(BandId))]
        public virtual Band Band { get; set; }
    }
}
