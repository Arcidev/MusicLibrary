using Shared.Enums;
using System;

namespace BL.DTO
{
    public abstract class ReviewDTO
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime EditDate { get; set; }

        public int CreatedById { get; set; }

        public string CreatedByFirstName { get; set; }

        public string CreatedByLastName { get; set; }

        public string CreatedByFullName { get { return $"{CreatedByFirstName} {CreatedByLastName}"; } }

        public string CreatedByImageStorageFileName { get; set; }

        public Quality Quality { get; set; }

        public int QualityInt { get { return (int)Quality; } }
    }
}
