using Riganti.Utils.Infrastructure.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Song : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public bool Approved { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
