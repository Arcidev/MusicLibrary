using AutoMapper;
using BL.DTO;
using BL.Repositories;
using DAL.Entities;
using DotVVM.Framework.Storage;
using BL.Queries;
using System;
using System.Collections.Generic;
using BL.Resources;
using DotVVM.Framework.Controls;
using System.Linq;

namespace BL.Facades
{
    public class SongFacade : ImageStorableFacade
    {
        public Func<SongRepository> SongRepositoryFunc { get; set; }

        public Func<SongsQuery<SongDTO>> SongsQuerySongFunc { get; set; }

        public Func<SongsQuery<SongInfoDTO>> SongsQuerySongInfoFunc { get; set; }

        public Func<AlbumSongRepository> AlbumSongRepositoryFunc { get; set; }

        public IMapper Mapper { get; set; }

        public SongDTO AddSong(SongCreateDTO song, UploadedFile audioFile = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<Song>(song);
                entity.CreateDate = DateTime.Now;

                SetAudioFile(entity, audioFile, storage);

                if (song.AddedAlbums != null && song.AddedAlbums.Any())
                {
                    var albumSongsRepo = AlbumSongRepositoryFunc();
                    albumSongsRepo.Insert(song.AddedAlbums.Select(albumId => new AlbumSong()
                    {
                        AlbumId = albumId,
                        Song = entity
                    }));
                }
                else
                {
                    var repo = SongRepositoryFunc();
                    repo.Insert(entity);
                }

                uow.Commit();

                return Mapper.Map<SongDTO>(entity);
            }
        }

        public void EditSong(SongEditDTO song, UploadedFile audioFile = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = SongRepositoryFunc();
                var entity = repo.GetById(song.Id);
                IsNotNull(entity, ErrorMessages.SongNotExist);

                Mapper.Map(song, entity);
                SetAudioFile(entity, audioFile, storage);

                var albumSongRepo = AlbumSongRepositoryFunc();
                if (song.RemovedAlbums != null)
                    albumSongRepo.DeleteByAlbumIds(song.RemovedAlbums);

                if (song.AddedAlbums != null)
                {
                    albumSongRepo.Insert(song.AddedAlbums.Select(albumId => new AlbumSong()
                    {
                        AlbumId = albumId,
                        SongId = entity.Id
                    }));
                }

                uow.Commit();
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
                query.Approved = true;
                return query.Execute();
            }
        }

        public IEnumerable<SongInfoDTO> GetSongInfoes()
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = SongsQuerySongInfoFunc();
                query.Approved = true;
                return query.Execute();
            }
        }

        public void LoadSongInfoes(GridViewDataSet<SongInfoDTO> dataSet, string filter = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = SongsQuerySongInfoFunc();
                query.Filter = filter;

                FillDataSet(dataSet, query);
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

        public void ApproveSongs(IEnumerable<int> songIds, bool approved)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = SongRepositoryFunc();
                var songs = repo.GetByIds(songIds);
                foreach (var song in songs)
                    song.Approved = approved;

                uow.Commit();
            }
        }

        private void SetAudioFile(Song entity, UploadedFile file, IUploadedFileStorage storage)
        {
            if (file != null && storage != null)
            {
                if (entity.AudioStorageFileId.HasValue)
                    StorageFileFacade.Value.DeleteFile(entity.AudioStorageFileId.Value);

                var fileName = StorageFileFacade.Value.SaveFile(file, storage);
                entity.AudioStorageFile = new StorageFile()
                {
                    DisplayName = file.FileName,
                    FileName = fileName
                };
            }
        }
    }
}
