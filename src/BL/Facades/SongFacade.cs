using AutoMapper;
using BL.DTO;
using BL.Repositories;
using DAL.Entities;
using DotVVM.Framework.Storage;
using System;

namespace BL.Facades
{
    public class SongFacade : ImageStorableFacade
    {
        public Func<SongRepository> SongRepositoryFunc { get; set; }

        public SongDTO AddSong(SongCreateDTO song, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<Song>(song);
                entity.CreateDate = DateTime.Now;
                if (file != null && storage != null)
                {
                    var fileName = StorageFileFacade.Value.SaveFile(file, storage);
                    entity.AudioStorageFile = new StorageFile()
                    {
                        DisplayName = file.FileName,
                        FileName = fileName
                    };
                }

                var repo = SongRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();

                return Mapper.Map<SongDTO>(entity);
            }
        }

        public void DeleteSong(int id)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = SongRepositoryFunc();
                repo.Delete(id);

                uow.Commit();
            }
        }
    }
}
