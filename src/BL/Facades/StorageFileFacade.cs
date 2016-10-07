using AutoMapper;
using BL.DTO;
using BL.Repositories;
using DAL.Entities;
using DotVVM.Framework.Storage;
using System;
using System.IO;
using System.Web.Hosting;

namespace BL.Facades
{
    public class StorageFileFacade : BaseFacade
    {
        public Func<StorageFileRepository> StorageFileRepositoryFunc { get; set; }

        public StorageFileDTO AddFile(UploadedFile file, IUploadedFileStorage storage)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var fileName = SaveFile(file, storage);
                var entity = new StorageFile()
                {
                    DisplayName = file.FileName,
                    FileName = fileName
                };

                var repo = StorageFileRepositoryFunc();
                repo.Insert(entity);
                uow.Commit();

                return Mapper.Map<StorageFileDTO>(entity);
            }
        }

        public string SaveFile(UploadedFile file, IUploadedFileStorage storage)
        {
            var fileName = $"{file.FileId}{Path.GetExtension(file.FileName)}";
            var targetPath = Path.Combine(GetUploadPath(), fileName);
            storage.SaveAs(file.FileId, targetPath);
            storage.DeleteFile(file.FileId);

            return fileName;
        }

        private string GetUploadPath()
        {
            var uploadPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "SavedFiles");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            return uploadPath;
        }
    }
}
