using AutoMapper;
using BusinessLayer.DTO;
using BusinessLayer.Repositories;
using DataLayer.Entities;
using DotVVM.Framework.Storage;
using BusinessLayer.Queries;
using System;
using System.Collections.Generic;
using BusinessLayer.Resources;
using DotVVM.Framework.Controls;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;

namespace BusinessLayer.Facades
{
    public class SongFacade : ImageStorableFacade
    {
        private readonly Func<SongRepository> songRepositoryFunc;
        private readonly Func<AlbumSongRepository> albumSongRepositoryFunc;
        private readonly Func<SongsQuery<SongDTO>> songsQuerySongFunc;
        private readonly Func<SongsQuery<SongInfoDTO>> songsQuerySongInfoFunc;

        public SongFacade(Func<SongRepository> songRepositoryFunc,
            Func<AlbumSongRepository> albumSongRepositoryFunc,
            Func<SongsQuery<SongDTO>> songsQuerySongFunc,
            Func<SongsQuery<SongInfoDTO>> songsQuerySongInfoFunc,
            IMapper mapper,
            Lazy<StorageFileFacade> storageFileFacase,
            Func<IUnitOfWorkProvider> uowProvider
            ) : base(mapper, storageFileFacase, uowProvider)
        {
            this.songRepositoryFunc = songRepositoryFunc;
            this.albumSongRepositoryFunc = albumSongRepositoryFunc;
            this.songsQuerySongFunc = songsQuerySongFunc;
            this.songsQuerySongInfoFunc = songsQuerySongInfoFunc;
        }

        public SongDTO AddSong(SongCreateDTO song, UploadedFile audioFile = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var entity = mapper.Map<Song>(song);
            entity.CreateDate = DateTime.Now;

            SetAudioFile(entity, audioFile, storage);

            if (song.AddedAlbums != null && song.AddedAlbums.Any())
            {
                var albumSongsRepo = albumSongRepositoryFunc();
                albumSongsRepo.Insert(song.AddedAlbums.Select(albumId => new AlbumSong()
                {
                    AlbumId = albumId,
                    Song = entity
                }));
            }
            else
            {
                var repo = songRepositoryFunc();
                repo.Insert(entity);
            }

            uow.Commit();

            return mapper.Map<SongDTO>(entity);
        }

        public void EditSong(SongEditDTO song, UploadedFile audioFile = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var repo = songRepositoryFunc();
            var entity = repo.GetById(song.Id);
            IsNotNull(entity, ErrorMessages.SongNotExist);

            mapper.Map(song, entity);
            SetAudioFile(entity, audioFile, storage);

            var albumSongRepo = albumSongRepositoryFunc();
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

        public void DeleteSong(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = songRepositoryFunc();
            repo.Delete(id);

            uow.Commit();
        }

        public IEnumerable<SongDTO> GetSongs()
        {
            using var uow = uowProviderFunc().Create();
            var query = songsQuerySongFunc();
            query.Approved = true;
            return query.Execute();
        }

        public IEnumerable<SongInfoDTO> GetSongInfoes()
        {
            using var uow = uowProviderFunc().Create();
            var query = songsQuerySongInfoFunc();
            query.Approved = true;
            return query.Execute();
        }

        public void LoadSongInfoes(GridViewDataSet<SongInfoDTO> dataSet, string filter = null)
        {
            using var uow = uowProviderFunc().Create();
            var query = songsQuerySongInfoFunc();
            query.Filter = filter;

            FillDataSet(dataSet, query);
        }

        public SongDTO GetSong(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = songRepositoryFunc();
            var entity = repo.GetById(id);
            IsNotNull(entity, ErrorMessages.SongNotExist);
            return mapper.Map<SongDTO>(entity);
        }

        public void ApproveSongs(IEnumerable<int> songIds, bool approved)
        {
            using var uow = uowProviderFunc().Create();
            var repo = songRepositoryFunc();
            var songs = repo.GetByIds(songIds);
            foreach (var song in songs)
                song.Approved = approved;

            uow.Commit();
        }

        private void SetAudioFile(Song entity, UploadedFile file, IUploadedFileStorage storage)
        {
            if (file != null && storage != null)
            {
                if (entity.AudioStorageFileId.HasValue)
                    storageFileFacade.Value.DeleteFile(entity.AudioStorageFileId.Value);

                var fileName = storageFileFacade.Value.SaveFile(file, storage);
                entity.AudioStorageFile = new StorageFile()
                {
                    DisplayName = file.FileName,
                    FileName = fileName
                };
            }
        }
    }
}
