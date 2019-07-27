using Riganti.Utils.Infrastructure.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class Song : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public bool Approved { get; set; }

        public DateTime CreateDate { get; set; }

        public int? AudioStorageFileId { get; set; }

        [MaxLength(50)]
        public string YoutubeUrlParam { get; set; }

        [ForeignKey(nameof(AudioStorageFileId))]
        public virtual StorageFile AudioStorageFile { get; set; }
    }
}
