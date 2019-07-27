using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class Album : IEntity<int>, IImageFileEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public int? ImageStorageFileId { get; set; }

        public int BandId { get; set; }

        public bool Approved { get; set; }

        public DateTime CreateDate { get; set; }

        public int CategoryId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AverageQuality { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        [ForeignKey(nameof(BandId))]
        public virtual Band Band { get; set; }

        [ForeignKey(nameof(ImageStorageFileId))]
        public virtual StorageFile ImageStorageFile { get; set; }

        public virtual ICollection<AlbumReview> Reviews { get; set; }

        public virtual ICollection<AlbumSong> AlbumSongs { get; set; }
    }
}
