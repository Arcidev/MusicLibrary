using Shared.Enums;
using System;

namespace BL.DTO
{
    public abstract class ReviewDTO
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedById { get; set; }

        public Quality Quality { get; set; }
    }
}
