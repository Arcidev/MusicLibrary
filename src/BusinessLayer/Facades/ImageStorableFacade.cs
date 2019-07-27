using AutoMapper;
using DataLayer.Entities;
using DotVVM.Framework.Storage;
using Riganti.Utils.Infrastructure.Core;
using System;

namespace BusinessLayer.Facades
{
    public abstract class ImageStorableFacade : BaseFacade
    {
        protected readonly Lazy<StorageFileFacade> storageFileFacade;

        protected ImageStorableFacade(IMapper mapper, Lazy<StorageFileFacade> storageFileFacade, Func<IUnitOfWorkProvider> uowProviderFunc) : base(mapper, uowProviderFunc)
        {
            this.storageFileFacade = storageFileFacade;
        }

        protected void SetImageFile(IImageFileEntity entity, UploadedFile file, IUploadedFileStorage storage)
        {
            if (file != null && storage != null)
            {
                if (entity.ImageStorageFile != null)
                    storageFileFacade.Value.DeleteFile(entity.ImageStorageFile.Id);

                var fileName = storageFileFacade.Value.SaveFile(file, storage);
                entity.ImageStorageFile = new StorageFile()
                {
                    DisplayName = file.FileName,
                    FileName = fileName
                };
            }
        }
    }
}
