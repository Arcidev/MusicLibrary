using Riganti.Utils.Infrastructure.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Band : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int? ImageStorageFileId { get; set; }

        [ForeignKey(nameof(ImageStorageFileId))]
        public StorageFile ImageStorageFile { get; set; }
    }
}
