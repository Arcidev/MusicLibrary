using Riganti.Utils.Infrastructure.Core;
using Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public abstract class Review : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime EditDate { get; set; }

        public int CreatedById { get; set; }

        public Quality Quality { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public virtual User CreatedBy { get; set; }
    }
}
