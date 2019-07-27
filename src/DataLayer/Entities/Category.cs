using Riganti.Utils.Infrastructure.Core;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Category : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Name_skSK { get; set; }

        [MaxLength(100)]
        public string Name_csCZ { get; set; }

        [MaxLength(100)]
        public string Name_esES { get; set; }
    }
}
