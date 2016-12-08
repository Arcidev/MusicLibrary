using Shared.Enums;

namespace BL.DTO
{
    public class UserBaseDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public UserRole UserRole { get; set; }
    }
}
