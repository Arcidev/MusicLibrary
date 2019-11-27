﻿using AutoMapper;
using BusinessLayer.DTO;
using BusinessLayer.Repositories;
using BusinessLayer.Resources;
using DataLayer.Entities;
using DotVVM.Framework.Storage;
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

        public StorageFileFacade(IMapper mapper, Func<StorageFileRepository> storageFileRepositoryFunc, Func<IUnitOfWorkProvider> uowProviderFunc) : base(mapper, uowProviderFunc)
        {
            uploadPath = Directory.GetCurrentDirectory() + "\\wwwroot\\SavedFiles";
            this.storageFileRepositoryFunc = storageFileRepositoryFunc;
        }

        public async Task<StorageFileDTO> AddFileAsync(UploadedFile file, IUploadedFileStorage storage)
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
            await uow.CommitAsync();

            return mapper.Map<StorageFileDTO>(entity);
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

        public string SaveFile(UploadedFile file, IUploadedFileStorage storage)
        {
            var fileName = $"{file.FileId}{Path.GetExtension(file.FileName)}";
            var targetPath = Path.Combine(GetUploadPath(), fileName);
            storage.SaveAs(file.FileId, targetPath);
            storage.DeleteFile(file.FileId);

            return fileName;
        }

        public async Task<StorageFileDTO> GetFileAsync(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = storageFileRepositoryFunc();
            var storageFile = await repo.GetByIdAsync(id);
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