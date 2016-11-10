using DAL.Entities;
using DotVVM.Framework.Storage;
using Riganti.Utils.Infrastructure.Core;
using System;

namespace BL.Facades
{
    public class BaseFacade
    {
        public Func<IUnitOfWorkProvider> UowProviderFunc { get; set; }

        public Lazy<StorageFileFacade> StorageFileFacade { get; set; }

        public void IsNotNull(object obj, string errorMessage)
        {
            if (obj == null)
                throw new UIException(errorMessage);
        }

        protected void SetFile(IImageFileEntity entity, UploadedFile file, IUploadedFileStorage storage)
        {
            if (file != null && storage != null)
            {
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
