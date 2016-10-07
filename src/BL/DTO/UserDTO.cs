using DAL.Enums;

namespace BL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }

        public string Password { get; set; }

        public UserRole UserRole { get; set; }

        public int? ImageStorageFileId { get; set; }

        public StorageFileDTO ImageStorageFile { get; set; }
    }
}
