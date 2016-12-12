using DAL.Entities;
using DotVVM.Framework.Storage;
using System;

namespace BL.Facades
{
    public abstract class ImageStorableFacade : BaseFacade
    {
        public Lazy<StorageFileFacade> StorageFileFacade { get; set; }

        protected void SetImageFile(IImageFileEntity entity, UploadedFile file, IUploadedFileStorage storage)
        {
            if (file != null && storage != null)
            {
                if (entity.ImageStorageFile != null)
                    StorageFileFacade.Value.DeleteFile(entity.ImageStorageFile.Id);

                var fileName = StorageFileFacade.Value.SaveFile(file, storage);
                entity.ImageStorageFile = new StorageFile()
                {
                    DisplayName = file.FileName,
                    FileName = fileName
                };
            }
        }
    }
}
