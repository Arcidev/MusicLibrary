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
    public class AlbumFacade : ImageStorableFacade
    {
        private readonly Func<RecentAlbumsQuery> recentlyAddedAlbumsQueryFunc;
        private readonly Func<FeaturedAlbumsQuery> featuredAlbumsQueryFunc;
        private readonly Func<AlbumRepository> albumRepositoryFunc;
        private readonly Func<AlbumSongsQuery<SongDTO>> albumSongsQuerySongFunc;
        private readonly Func<AlbumSongsQuery<SongInfoDTO>> albumSongsQuerySongInfoFunc;
        private readonly Func<AlbumsQuery<AlbumDTO>> albumsQueryAlbumFunc;
        private readonly Func<AlbumsQuery<AlbumInfoDTO>> albumsQueryAlbumInfoFunc;
        private readonly Func<AlbumsQuery<AlbumBandInfoDTO>> albumsQueryAlbumBandInfoFunc;
        private readonly Func<AlbumReviewRepository> albumReviewRepositoryFunc;
        private readonly Func<AlbumReviewsQuery> albumReviewsQueryFunc;
        private readonly Func<UserAlbumReviewsQuery> userAlbumReviewsQueryFunc;
        private readonly Func<AlbumSongRepository> albumSongRepositoryFunc;
        private readonly Func<UserAlbumRepository> userAlbumRepositoryFunc;
        private readonly Func<UserAlbumsQuery> userAlbumsQueryFunc;
        private readonly Func<IsInUserAlbumCollectionQuery> isInUserAlbumCollectionQueryFunc;

        public AlbumFacade(Func<RecentAlbumsQuery> recentlyAddedAlbumsQueryFunc,
            Func<FeaturedAlbumsQuery> featuredAlbumsQueryFunc,
            Func<AlbumRepository> albumRepositoryFunc,
            Func<AlbumSongsQuery<SongDTO>> albumSongsQuerySongFunc,
            Func<AlbumSongsQuery<SongInfoDTO>> albumSongsQuerySongInfoFunc,
            Func<AlbumsQuery<AlbumDTO>> albumsQueryAlbumFunc,
            Func<AlbumsQuery<AlbumInfoDTO>> albumsQueryAlbumInfoFunc,
            Func<AlbumsQuery<AlbumBandInfoDTO>> albumsQueryAlbumBandInfoFunc,
            Func<AlbumReviewRepository> albumReviewRepositoryFunc,
            Func<AlbumReviewsQuery> albumReviewsQueryFunc,
            Func<UserAlbumReviewsQuery> userAlbumReviewsQueryFunc,
            Func<AlbumSongRepository> albumSongRepositoryFunc,
            Func<UserAlbumRepository> userAlbumRepositoryFunc,
            Func<UserAlbumsQuery> userAlbumsQueryFunc,
            Func<IsInUserAlbumCollectionQuery> isInUserAlbumCollectionQueryFunc,
            Lazy<StorageFileFacade> storageFileFacade,
            Func<IUnitOfWorkProvider> uowProvider) : base(storageFileFacade, uowProvider)
        {
            this.recentlyAddedAlbumsQueryFunc = recentlyAddedAlbumsQueryFunc;
            this.featuredAlbumsQueryFunc = featuredAlbumsQueryFunc;
            this.albumRepositoryFunc = albumRepositoryFunc;
            this.albumSongsQuerySongFunc = albumSongsQuerySongFunc;
            this.albumSongsQuerySongInfoFunc = albumSongsQuerySongInfoFunc;
            this.albumsQueryAlbumFunc = albumsQueryAlbumFunc;
            this.albumsQueryAlbumInfoFunc = albumsQueryAlbumInfoFunc;
            this.albumsQueryAlbumBandInfoFunc = albumsQueryAlbumBandInfoFunc;
            this.albumReviewRepositoryFunc = albumReviewRepositoryFunc;
            this.albumReviewsQueryFunc = albumReviewsQueryFunc;
            this.userAlbumReviewsQueryFunc = userAlbumReviewsQueryFunc;
            this.albumSongRepositoryFunc = albumSongRepositoryFunc;
            this.userAlbumRepositoryFunc = userAlbumRepositoryFunc;
            this.userAlbumsQueryFunc = userAlbumsQueryFunc;
            this.isInUserAlbumCollectionQueryFunc = isInUserAlbumCollectionQueryFunc;
        }

        public async Task<AlbumDTO> AddAlbumAsync(AlbumCreateDTO album, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var entity = album.Adapt<Album>();
            entity.CreateDate = DateTime.Now;
            await SetImageFileAsync(entity, file, storage);

            if (album.AddedSongs != null)
            {
                entity.AlbumSongs = album.AddedSongs.Select(songId => new AlbumSong()
                {
                    SongId = songId
                }).ToList();
            }

            var repo = albumRepositoryFunc();
            repo.Insert(entity);

            await uow.CommitAsync();
            return entity.Adapt<AlbumDTO>();
        }

        public async Task ApproveAlbumsAsync(IEnumerable<int> albumIds, bool approved)
        {
            using var uow = uowProviderFunc().Create();
            var repo = albumRepositoryFunc();
            var albums = await repo.GetByIdsAsync(albumIds);
            foreach (var album in albums)
                album.Approved = approved;

            await uow.CommitAsync();
        }

        public async Task EditAlbumAsync(AlbumEditDTO album, UploadedFile imageFile = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var repo = albumRepositoryFunc();
            var entity = await repo.GetByIdAsync(album.Id);
            IsNotNull(entity, ErrorMessages.AlbumNotExist);

            album.Adapt(entity);
            await SetImageFileAsync(entity, imageFile, storage);

            if (album.RemovedSongs != null)
            {
                var toRemove = entity.AlbumSongs.Where(x => album.RemovedSongs.Contains(x.SongId)).ToList();
                var albumSongRepo = albumSongRepositoryFunc();
                foreach (var albumSong in toRemove)
                {
                    entity.AlbumSongs.Remove(albumSong);
                    albumSongRepo.Delete(albumSong);
                }
            }

            if (album.AddedSongs != null)
            {
                foreach (var addedSongId in album.AddedSongs)
                    entity.AlbumSongs.Add(new AlbumSong() { SongId = addedSongId });
            }

            await uow.CommitAsync();
        }

        public async Task<IList<SongDTO>> GetAlbumSongsAsync(int albumId)
        {
            using var uow = uowProviderFunc().Create();
            var query = albumSongsQuerySongFunc();
            query.AlbumId = albumId;
            query.Approved = true;

            return await query.ExecuteAsync();
        }

        public async Task<IList<AlbumBandInfoDTO>> GetAlbumBandInfoesAsync(int? songId = null)
        {
            using var uow = uowProviderFunc().Create();
            var query = albumsQueryAlbumBandInfoFunc();
            query.Approved = true;
            query.SongId = songId;

            return await query.ExecuteAsync();
        }

        public async Task<IList<SongInfoDTO>> GetAlbumSongInfoesAsync(int albumId)
        {
            using var uow = uowProviderFunc().Create();
            var query = albumSongsQuerySongInfoFunc();
            query.AlbumId = albumId;
            query.Approved = true;

            return await query.ExecuteAsync();
        }

        public async Task<AlbumDTO> GetAlbumAsync(int id, bool includeBandInfo = true, bool includeSongs = true)
        {
            using var uow = uowProviderFunc().Create();
            var repo = albumRepositoryFunc();
            var entity = await repo.GetByIdAsync(id);
            IsNotNull(entity, ErrorMessages.AlbumNotExist);

            var dto = entity.Adapt<AlbumDTO>();
            if (includeBandInfo)
                dto.Band = entity.Band.Adapt<BandDTO>();

            if (includeSongs)
                dto.Songs = await GetAlbumSongsAsync(entity.Id);

            return dto;
        }

        public async Task<IList<AlbumDTO>> GetAlbumsAsync(int? categoryId = null, string filter = null)
        {
            using var uow = uowProviderFunc().Create();
            var query = albumsQueryAlbumFunc();
            query.CategoryId = categoryId;
            query.Filter = filter;
            query.IncludeBandFilter = true;
            query.Approved = true;

            return await query.ExecuteAsync();
        }

        public async Task LoadAlbumsAsync(GridViewDataSet<AlbumInfoDTO> dataSet, string filter = null, bool? approved = null)
        {
            using var uow = uowProviderFunc().Create();
            var query = albumsQueryAlbumInfoFunc();
            query.Filter = filter;
            query.Approved = approved;

            await FillDataSetAsync(dataSet, query);
        }

        public async Task LoadUserAlbumsCollectionAsync(int userId, GridViewDataSet<AlbumInfoDTO> dataSet, string filter = null)
        {
            await LoadAlbumsAsync(dataSet, filter, true);
            using var uow = uowProviderFunc().Create();
            var collectionQuery = isInUserAlbumCollectionQueryFunc();
            collectionQuery.UserId = userId;
            collectionQuery.AlbumIds = dataSet.Items.Select(x => x.AlbumId);
            var collection = await collectionQuery.ExecuteAsync();

            foreach (var album in dataSet.Items)
            {
                if (collection.Contains(album.AlbumId))
                    album.HasInCollection = true;
            }
        }

        public async Task AddReviewAsync(AlbumReviewCreateDTO review)
        {
            using var uow = uowProviderFunc().Create();
            var entity = review.Adapt<AlbumReview>();
            entity.CreateDate = DateTime.Now;
            entity.EditDate = DateTime.Now;

            var repo = albumReviewRepositoryFunc();
            repo.Insert(entity);

            await uow.CommitAsync();

            await UpdateAlbumQualityAsync(review.AlbumId);
            await uow.CommitAsync();
        }

        public async Task LoadReviewsAsync(int albumId, GridViewDataSet<ReviewDTO> dataSet)
        {
            using var uow = uowProviderFunc().Create();
            var query = albumReviewsQueryFunc();
            query.AlbumId = albumId;

            await FillDataSetAsync(dataSet, query);
        }

        public async Task EditUserReviewAsync(int reviewId, ReviewEditDTO editedReview)
        {
            using var uow = uowProviderFunc().Create();
            var repo = albumReviewRepositoryFunc();
            var entity = await repo.GetByIdAsync(reviewId);
            IsNotNull(entity, ErrorMessages.ReviewNotExist);

            if (entity.CreatedById != editedReview.CreatedById)
                throw new UIException(ErrorMessages.NotUserReview);

            var qualityUpdated = editedReview.Quality != entity.Quality;
            editedReview.Adapt(entity);
            entity.EditDate = DateTime.Now;
            await uow.CommitAsync();

            if (!qualityUpdated)
                return;

            await UpdateAlbumQualityAsync(entity.AlbumId);
            await uow.CommitAsync();
        }

        public async Task LoadUserReviewsAsync(int userId, GridViewDataSet<UserAlbumReviewDTO> dataSet)
        {
            using var uow = uowProviderFunc().Create();
            var query = userAlbumReviewsQueryFunc();
            query.UserId = userId;

            await FillDataSetAsync(dataSet, query);
        }

        public async Task AddSongToAlbumAsync(int albumId, int songId)
        {
            using var uow = uowProviderFunc().Create();
            var repo = albumSongRepositoryFunc();
            repo.Insert(new AlbumSong()
            {
                AlbumId = albumId,
                SongId = songId
            });

            await uow.CommitAsync();
        }

        public async Task<IList<AlbumDTO>> GetRecentAlbumsAsync(int count)
        {
            using var uow = uowProviderFunc().Create();
            var query = recentlyAddedAlbumsQueryFunc();
            query.Take = count;

            return await query.ExecuteAsync();
        }

        public async Task<IList<AlbumDTO>> GetFeaturedAlbumsAsync(int count)
        {
            using var uow = uowProviderFunc().Create();
            var query = featuredAlbumsQueryFunc();
            query.Take = count;

            return await query.ExecuteAsync();
        }

        public async Task AddAlbumToUserCollectionAsync(UserAlbumCreateDTO userAlbum)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userAlbumRepositoryFunc();
            if (await repo.GetUserAlbumAsync(userAlbum.UserId, userAlbum.AlbumId) != null)
                return;

            var entity = userAlbum.Adapt<UserAlbum>();
            repo.Insert(entity);

            await uow.CommitAsync();
        }

        public async Task RemoveAlbumFromUserCollectionAsync(int userId, int albumId)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userAlbumRepositoryFunc();
            var entity = await repo.GetUserAlbumAsync(userId, albumId);
            if (entity == null)
                return;

            repo.Delete(entity);
            await uow.CommitAsync();
        }

        public async Task<bool> HasUserInCollectionAsync(int userId, int albumId)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userAlbumRepositoryFunc();
            var entity = await repo.GetUserAlbumAsync(userId, albumId);

            return entity != null;
        }

        public async Task<IList<AlbumDTO>> GetUserAlbumsAsync(int userId)
        {
            using var uow = uowProviderFunc().Create();
            var query = userAlbumsQueryFunc();
            query.UserId = userId;

            return await query.ExecuteAsync();
        }

        private async Task UpdateAlbumQualityAsync(int albumId)
        {
            using var uow = uowProviderFunc().Create();
            var repo = albumReviewRepositoryFunc();
            var averageQuality = await repo.GetAlbumAverageQualityAsync(albumId);

            var albumRepo = albumRepositoryFunc();
            var album = await albumRepo.GetByIdAsync(albumId);
            album.AverageQuality = Convert.ToDecimal(averageQuality);

            await uow.CommitAsync();
        }
    }
}
