using BusinessLayer.DTO;
using BusinessLayer.Repositories;
using BusinessLayer.Resources;
using DataLayer.Entities;
using DotVVM.Core.Storage;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BusinessLayer.Facades
{
    public class StorageFileFacade : BaseFacade
    {
        private readonly string uploadPath;
        private readonly Func<StorageFileRepository> storageFileRepositoryFunc;

        public StorageFileFacade(Func<StorageFileRepository> storageFileRepositoryFunc, Func<IUnitOfWorkProvider> uowProviderFunc) : base(uowProviderFunc)
        {
            uploadPath = Directory.GetCurrentDirectory() + "\\wwwroot\\SavedFiles";
            this.storageFileRepositoryFunc = storageFileRepositoryFunc;
        }

        public async Task<StorageFileDTO> AddFileAsync(UploadedFile file, IUploadedFileStorage storage)
        {
            using var uow = uowProviderFunc().Create();
            var fileName = await SaveFileAsync(file, storage);
            var entity = new StorageFile()
            {
                DisplayName = file.FileName,
                FileName = fileName
            };

            var repo = storageFileRepositoryFunc();
            repo.Insert(entity);
            await uow.CommitAsync();

            return entity.Adapt<StorageFileDTO>();
        }

        public async Task DeleteFileAsync(int fileId)
        {
            using var uow = uowProviderFunc().Create();
            var repo = storageFileRepositoryFunc();
            var storageFile = await repo.GetByIdAsync(fileId);
            IsNotNull(storageFile, ErrorMessages.FileNotExist);

            var targetPath = Path.Combine(GetUploadPath(), storageFile.FileName);
            if (File.Exists(targetPath))
                File.Delete(targetPath);

            repo.Delete(storageFile);
            await uow.CommitAsync();
        }

        public async Task<string> SaveFileAsync(UploadedFile file, IUploadedFileStorage storage)
        {
            var fileName = $"{file.FileId}{Path.GetExtension(file.FileName)}";
            var targetPath = Path.Combine(GetUploadPath(), fileName);
            await storage.SaveAsAsync(file.FileId, targetPath);
            await storage.DeleteFileAsync(file.FileId);

            return fileName;
        }

        public async Task<StorageFileDTO> GetFileAsync(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = storageFileRepositoryFunc();
            var storageFile = await repo.GetByIdAsync(id);
            IsNotNull(storageFile, ErrorMessages.FileNotExist);

            return storageFile.Adapt<StorageFileDTO>();
        }

        public string GetUploadPath()
        {
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            return uploadPath;
        }
    }
}
