using Riganti.Utils.Infrastructure.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class UserAlbum : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public int AlbumId { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(AlbumId))]
        public virtual Album Album { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
