using Riganti.Utils.Infrastructure.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class AlbumSong : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public int AlbumId { get; set; }

        public int SongId { get; set; }

        [ForeignKey(nameof(AlbumId))]
        public virtual Album Album { get; set; }

        [ForeignKey(nameof(SongId))]
        public virtual Song Song { get; set; }
    }
}
