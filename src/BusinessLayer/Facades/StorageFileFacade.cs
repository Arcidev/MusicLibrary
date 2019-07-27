using AutoMapper;
using BusinessLayer.DTO;
using BusinessLayer.Repositories;
using BusinessLayer.Resources;
using DataLayer.Entities;
using DotVVM.Framework.Storage;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.IO;

namespace BusinessLayer.Facades
{
    public class StorageFileFacade : BaseFacade
    {
        private readonly string uploadPath;
        private readonly Func<StorageFileRepository> storageFileRepositoryFunc;

        public StorageFileFacade(IMapper mapper, Func<StorageFileRepository> storageFileRepositoryFunc, Func<IUnitOfWorkProvider> uowProviderFunc) : base(mapper, uowProviderFunc)
        {
            uploadPath = Directory.GetCurrentDirectory() + "\\wwwroot\\SavedFiles";
            this.storageFileRepositoryFunc = storageFileRepositoryFunc;
        }

        public StorageFileDTO AddFile(UploadedFile file, IUploadedFileStorage storage)
        {
            using var uow = uowProviderFunc().Create();
            var fileName = SaveFile(file, storage);
            var entity = new StorageFile()
            {
                DisplayName = file.FileName,
                FileName = fileName
            };

            var repo = storageFileRepositoryFunc();
            repo.Insert(entity);
            uow.Commit();

            return mapper.Map<StorageFileDTO>(entity);
        }

        public void DeleteFile(int fileId)
        {
            using var uow = uowProviderFunc().Create();
            var repo = storageFileRepositoryFunc();
            var storageFile = repo.GetById(fileId);
            IsNotNull(storageFile, ErrorMessages.FileNotExist);

            var targetPath = Path.Combine(GetUploadPath(), storageFile.FileName);
            if (File.Exists(targetPath))
                File.Delete(targetPath);

            repo.Delete(storageFile);
            uow.Commit();
        }

        public string SaveFile(UploadedFile file, IUploadedFileStorage storage)
        {
            var fileName = $"{file.FileId}{Path.GetExtension(file.FileName)}";
            var targetPath = Path.Combine(GetUploadPath(), fileName);
            storage.SaveAs(file.FileId, targetPath);
            storage.DeleteFile(file.FileId);

            return fileName;
        }

        public StorageFileDTO GetFile(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = storageFileRepositoryFunc();
            var storageFile = repo.GetById(id);
            IsNotNull(storageFile, ErrorMessages.FileNotExist);

            return mapper.Map<StorageFileDTO>(storageFile);
        }

        public string GetUploadPath()
        {
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            return uploadPath;
        }
    }
}
