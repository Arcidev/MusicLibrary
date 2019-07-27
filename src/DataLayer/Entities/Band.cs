using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class Band : IEntity<int>, IImageFileEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string Description { get; set; }

        public int? ImageStorageFileId { get; set; }

        public bool Approved { get; set; }

        public DateTime CreateDate { get; set; }

        [ForeignKey(nameof(ImageStorageFileId))]
        public virtual StorageFile ImageStorageFile { get; set; }

        public virtual ICollection<SliderImage> SliderImages { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

        public virtual ICollection<BandMember> BandMembers { get; set; }
    }
}
