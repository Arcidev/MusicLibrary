using Shared.Enums;

namespace BL.DTO
{
    public class ReviewEditDTO
    {
        public string Text { get; set; }

        public int CreatedById { get; set; }

        public Quality Quality { get; set; }
    }
}
