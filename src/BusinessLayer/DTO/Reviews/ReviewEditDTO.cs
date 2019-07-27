using Shared.Enums;

namespace BusinessLayer.DTO
{
    public class ReviewEditDTO
    {
        public string Text { get; set; }

        public int CreatedById { get; set; }

        public Quality Quality { get; set; }
    }
}
