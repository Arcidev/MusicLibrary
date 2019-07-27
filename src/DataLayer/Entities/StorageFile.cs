using Riganti.Utils.Infrastructure.Core;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class StorageFile : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string DisplayName { get; set; }

        [Required, MaxLength(100)]
        public string FileName { get; set; }
    }
}
