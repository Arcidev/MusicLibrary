using Riganti.Utils.Infrastructure.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Artist : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        public bool Approved { get; set; }

        public virtual ICollection<BandMember> BandsMember { get; set; }
    }
}
