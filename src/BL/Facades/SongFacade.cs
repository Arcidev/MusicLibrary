using AutoMapper;
using BL.DTO;
using BL.Repositories;
using DAL.Entities;
using DotVVM.Framework.Storage;
using BL.Queries;
using System;
using System.Collections.Generic;
using BL.Resources;

namespace BL.Facades
{
    public class SongFacade : ImageStorableFacade
    {
        public Func<SongRepository> SongRepositoryFunc { get; set; }

        public Func<SongsQuery<SongDTO>> SongsQuerySongFunc { get; set; }

        public Func<SongsQuery<SongInfoDTO>> SongsQuerySongInfoFunc { get; set; }

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

        public IEnumerable<SongDTO> GetSongs()
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = SongsQuerySongFunc();
                return query.Execute();
            }
        }

        public IEnumerable<SongInfoDTO> GetSongInfoes()
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = SongsQuerySongInfoFunc();
                return query.Execute();
            }
        }

        public SongDTO GetSong(int id)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = SongRepositoryFunc();
                var entity = repo.GetById(id);
                IsNotNull(entity, ErrorMessages.SongNotExist);
                return Mapper.Map<SongDTO>(entity);
            }
        }
    }
}
