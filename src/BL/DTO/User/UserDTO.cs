
namespace BL.DTO
{
    public class UserDTO : UserEditDTO
    {
        public string Email { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }

        public int? ImageStorageFileId { get; set; }

        public StorageFileDTO ImageStorageFile { get; set; }

        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}
