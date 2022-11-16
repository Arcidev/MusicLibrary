using BusinessLayer.DTO;
using BusinessLayer.Queries;
using BusinessLayer.Repositories;
using BusinessLayer.Resources;
using DataLayer.Entities;
using DotVVM.Core.Storage;
using DotVVM.Framework.Controls;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            Lazy<StorageFileFacade> storageFileFacase,
            Func<IUnitOfWorkProvider> uowProvider) : base(storageFileFacase, uowProvider)
        {
            this.songRepositoryFunc = songRepositoryFunc;
            this.albumSongRepositoryFunc = albumSongRepositoryFunc;
            this.songsQuerySongFunc = songsQuerySongFunc;
            this.songsQuerySongInfoFunc = songsQuerySongInfoFunc;
        }

        public async Task<SongDTO> AddSongAsync(SongCreateDTO song, UploadedFile audioFile = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var entity = song.Adapt<Song>();
            entity.CreateDate = DateTime.Now;

            await SetAudioFile(entity, audioFile, storage);

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

            await uow.CommitAsync();
            return entity.Adapt<SongDTO>();
        }

        public async Task EditSongAsync(SongEditDTO song, UploadedFile audioFile = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var repo = songRepositoryFunc();
            var entity = await repo.GetByIdAsync(song.Id);
            IsNotNull(entity, ErrorMessages.SongNotExist);

            song.Adapt(entity);
            await SetAudioFile(entity, audioFile, storage);

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

            await uow.CommitAsync();
        }

        public async Task DeleteSongAsync(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = songRepositoryFunc();
            repo.Delete(id);

            await uow.CommitAsync();
        }

        public async Task<IList<SongDTO>> GetSongsAsync()
        {
            using var uow = uowProviderFunc().Create();
            var query = songsQuerySongFunc();
            query.Approved = true;

            return await query.ExecuteAsync();
        }

        public async Task<IList<SongInfoDTO>> GetSongInfoesAsync()
        {
            using var uow = uowProviderFunc().Create();
            var query = songsQuerySongInfoFunc();
            query.Approved = true;

            return await query.ExecuteAsync();
        }

        public async Task LoadSongInfoesAsync(GridViewDataSet<SongInfoDTO> dataSet, string filter = null)
        {
            using var uow = uowProviderFunc().Create();
            var query = songsQuerySongInfoFunc();
            query.Filter = filter;

            await FillDataSetAsync(dataSet, query);
        }

        public async Task<SongDTO> GetSongAsync(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = songRepositoryFunc();
            var entity = await repo.GetByIdAsync(id);
            IsNotNull(entity, ErrorMessages.SongNotExist);

            return entity.Adapt<SongDTO>();
        }

        public async Task ApproveSongsAsync(IEnumerable<int> songIds, bool approved)
        {
            using var uow = uowProviderFunc().Create();
            var repo = songRepositoryFunc();
            var songs = await repo.GetByIdsAsync(songIds);
            foreach (var song in songs)
                song.Approved = approved;

            await uow.CommitAsync();
        }

        private async Task SetAudioFile(Song entity, UploadedFile file, IUploadedFileStorage storage)
        {
            if (file != null && storage != null)
            {
                if (entity.AudioStorageFileId.HasValue)
                    await storageFileFacade.Value.DeleteFileAsync(entity.AudioStorageFileId.Value);

                var fileName = await storageFileFacade.Value.SaveFileAsync(file, storage);
                entity.AudioStorageFile = new StorageFile()
                {
                    DisplayName = file.FileName,
                    FileName = fileName
                };
            }
        }
    }
}
