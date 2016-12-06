using Shared.Enums;

namespace BL.DTO
{
    public abstract class ReviewCreateDTO
    {
        public string Text { get; set; }

        public int CreatedById { get; set; }

        public Quality Quality { get; set; }
    }
}
