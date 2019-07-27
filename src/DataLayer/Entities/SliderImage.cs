using Riganti.Utils.Infrastructure.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class SliderImage : IEntity<int>, IImageFileEntity
    {
        [Key]
        public int Id { get; set; }

        public int BandId { get; set; }

        public int ImageStorageFileId { get; set; }

        [ForeignKey(nameof(BandId))]
        public virtual Band Band { get; set; }

        [ForeignKey(nameof(ImageStorageFileId))]
        public virtual StorageFile ImageStorageFile { get; set; }
    }
}
