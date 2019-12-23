using DataLayer.Entities;
using DotVVM.Framework.Storage;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Threading.Tasks;

namespace BusinessLayer.Facades
{
    public abstract class ImageStorableFacade : BaseFacade
    {
        protected readonly Lazy<StorageFileFacade> storageFileFacade;

        protected ImageStorableFacade(Lazy<StorageFileFacade> storageFileFacade, Func<IUnitOfWorkProvider> uowProviderFunc) : base(uowProviderFunc)
        {
            this.storageFileFacade = storageFileFacade;
        }

        protected async Task SetImageFileAsync(IImageFileEntity entity, UploadedFile file, IUploadedFileStorage storage)
        {
            if (file != null && storage != null)
            {
                if (entity.ImageStorageFile != null)
                    await storageFileFacade.Value.DeleteFileAsync(entity.ImageStorageFile.Id);

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
