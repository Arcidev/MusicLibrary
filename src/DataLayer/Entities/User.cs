using Riganti.Utils.Infrastructure.Core;
using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class User : IEntity<int>, IImageFileEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, EmailAddress, StringLength(100)]
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
        public virtual StorageFile ImageStorageFile { get; set; }
    }
}
