using DAL.Enums;
using Riganti.Utils.Infrastructure.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class User : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100), Index("IX_User_Email", IsUnique = true)]
        public string Email { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(100)]
        public string PasswordSalt { get; set; }

        [Required, StringLength(100)]
        public string PasswordHash { get; set; }

        public UserRole UserRole { get; set; }

        public int? ImageStorageFileId { get; set; }

        [ForeignKey(nameof(ImageStorageFileId))]
        public StorageFile ImageStorageFile { get; set; }
    }
}
