using AutoMapper;
using BusinessLayer.DTO;
using BusinessLayer.Queries;
using BusinessLayer.Repositories;
using BusinessLayer.Resources;
using DataLayer.Entities;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Storage;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;

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
            IMapper mapper,
            Lazy<StorageFileFacade> storageFileFacade,
            Func<IUnitOfWorkProvider> uowProvider) : base(mapper, storageFileFacade, uowProvider)
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

        public AlbumDTO AddAlbum(AlbumCreateDTO album, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var entity = mapper.Map<Album>(album);
            entity.CreateDate = DateTime.Now;
            SetImageFile(entity, file, storage);

            if (album.AddedSongs != null)
            {
                entity.AlbumSongs = album.AddedSongs.Select(songId => new AlbumSong()
                {
                    SongId = songId
                }).ToList();
            }

            var repo = albumRepositoryFunc();
            repo.Insert(entity);

            uow.Commit();
            return mapper.Map<AlbumDTO>(entity);
        }

        public void ApproveAlbums(IEnumerable<int> albumIds, bool approved)
        {
            using var uow = uowProviderFunc().Create();
            var repo = albumRepositoryFunc();
            var albums = repo.GetByIds(albumIds);
            foreach (var album in albums)
                album.Approved = approved;

            uow.Commit();
        }

        public void EditAlbum(AlbumEditDTO album, UploadedFile imageFile = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var repo = albumRepositoryFunc();
            var entity = repo.GetById(album.Id);
            IsNotNull(entity, ErrorMessages.AlbumNotExist);

            mapper.Map(album, entity);
            SetImageFile(entity, imageFile, storage);

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

            uow.Commit();
        }

        public IEnumerable<SongDTO> GetAlbumSongs(int albumId)
        {
            using var uow = uowProviderFunc().Create();
            var query = albumSongsQuerySongFunc();
            query.AlbumId = albumId;
            query.Approved = true;

            return query.Execute();
        }

        public IEnumerable<AlbumBandInfoDTO> GetAlbumBandInfoes(int? songId = null)
        {
            using var uow = uowProviderFunc().Create();
            var query = albumsQueryAlbumBandInfoFunc();
            query.Approved = true;
            query.SongId = songId;

            return query.Execute();
        }

        public IEnumerable<SongInfoDTO> GetAlbumSongInfoes(int albumId)
        {
            using var uow = uowProviderFunc().Create();
            var query = albumSongsQuerySongInfoFunc();
            query.AlbumId = albumId;
            query.Approved = true;

            return query.Execute();
        }

        public AlbumDTO GetAlbum(int id, bool includeBandInfo = true, bool includeSongs = true)
        {
            using var uow = uowProviderFunc().Create();
            var repo = albumRepositoryFunc();
            var entity = repo.GetById(id);
            IsNotNull(entity, ErrorMessages.AlbumNotExist);

            var dto = mapper.Map<AlbumDTO>(entity);
            if (includeBandInfo)
                dto.Band = mapper.Map<BandDTO>(entity.Band);

            if (includeSongs)
                dto.Songs = GetAlbumSongs(entity.Id);

            return dto;
        }

        public IEnumerable<AlbumDTO> GetAlbums(int? categoryId = null, string filter = null)
        {
            using var uow = uowProviderFunc().Create();
            var query = albumsQueryAlbumFunc();
            query.CategoryId = categoryId;
            query.Filter = filter;
            query.IncludeBandFilter = true;
            query.Approved = true;

            return query.Execute();
        }

        public void LoadAlbums(GridViewDataSet<AlbumInfoDTO> dataSet, string filter = null, bool? approved = null)
        {
            using var uow = uowProviderFunc().Create();
            var query = albumsQueryAlbumInfoFunc();
            query.Filter = filter;
            query.Approved = approved;

            FillDataSet(dataSet, query);
        }

        public void LoadUserAlbumsCollection(int userId, GridViewDataSet<AlbumInfoDTO> dataSet, string filter = null)
        {
            LoadAlbums(dataSet, filter, true);
            using var uow = uowProviderFunc().Create();
            var collectionQuery = isInUserAlbumCollectionQueryFunc();
            collectionQuery.UserId = userId;
            collectionQuery.AlbumIds = dataSet.Items.Select(x => x.AlbumId);
            var collection = collectionQuery.Execute();

            foreach (var album in dataSet.Items)
            {
                if (collection.Contains(album.AlbumId))
                    album.HasInCollection = true;
            }
        }

        public void AddReview(AlbumReviewCreateDTO review)
        {
            using var uow = uowProviderFunc().Create();
            var entity = mapper.Map<AlbumReview>(review);
            entity.CreateDate = DateTime.Now;
            entity.EditDate = DateTime.Now;

            var repo = albumReviewRepositoryFunc();
            repo.Insert(entity);

            uow.Commit();

            UpdateAlbumQuality(review.AlbumId);
            uow.Commit();
        }

        public void LoadReviews(int albumId, GridViewDataSet<ReviewDTO> dataSet)
        {
            using var uow = uowProviderFunc().Create();
            var query = albumReviewsQueryFunc();
            query.AlbumId = albumId;

            FillDataSet(dataSet, query);
        }

        public void EditUserReview(int reviewId, ReviewEditDTO editedReview)
        {
            using var uow = uowProviderFunc().Create();
            var repo = albumReviewRepositoryFunc();
            var entity = repo.GetById(reviewId);
            IsNotNull(entity, ErrorMessages.ReviewNotExist);

            if (entity.CreatedById != editedReview.CreatedById)
                throw new UIException(ErrorMessages.NotUserReview);

            var qualityUpdated = editedReview.Quality != entity.Quality;
            mapper.Map(editedReview, entity);
            entity.EditDate = DateTime.Now;
            uow.Commit();

            if (!qualityUpdated)
                return;

            UpdateAlbumQuality(entity.AlbumId);
            uow.Commit();
        }

        public void LoadUserReviews(int userId, GridViewDataSet<UserAlbumReviewDTO> dataSet)
        {
            using var uow = uowProviderFunc().Create();
            var query = userAlbumReviewsQueryFunc();
            query.UserId = userId;

            FillDataSet(dataSet, query);
        }

        public void AddSongToAlbum(int albumId, int songId)
        {
            using var uow = uowProviderFunc().Create();
            var repo = albumSongRepositoryFunc();
            repo.Insert(new AlbumSong()
            {
                AlbumId = albumId,
                SongId = songId
            });

            uow.Commit();
        }

        public IEnumerable<AlbumDTO> GetRecentAlbums(int count)
        {
            using var uow = uowProviderFunc().Create();
            var query = recentlyAddedAlbumsQueryFunc();
            query.Take = count;

            return query.Execute();
        }

        public IEnumerable<AlbumDTO> GetFeaturedAlbums(int count)
        {
            using var uow = uowProviderFunc().Create();
            var query = featuredAlbumsQueryFunc();
            query.Take = count;

            return query.Execute();
        }

        public void AddAlbumToUserCollection(UserAlbumCreateDTO userAlbum)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userAlbumRepositoryFunc();
            if (repo.GetUserAlbum(userAlbum.UserId, userAlbum.AlbumId) != null)
                return;

            var entity = mapper.Map<UserAlbum>(userAlbum);
            repo.Insert(entity);

            uow.Commit();
        }

        public void RemoveAlbumFromUserCollection(int userId, int albumId)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userAlbumRepositoryFunc();
            var entity = repo.GetUserAlbum(userId, albumId);
            if (entity == null)
                return;

            repo.Delete(entity);
            uow.Commit();
        }

        public bool HasUserInCollection(int userId, int albumId)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userAlbumRepositoryFunc();
            var entity = repo.GetUserAlbum(userId, albumId);

            return entity != null;
        }

        public IEnumerable<AlbumDTO> GetUserAlbums(int userId)
        {
            using var uow = uowProviderFunc().Create();
            var query = userAlbumsQueryFunc();
            query.UserId = userId;

            return query.Execute();
        }

        private void UpdateAlbumQuality(int albumId)
        {
            using var uow = uowProviderFunc().Create();
            var repo = albumReviewRepositoryFunc();
            var averageQuality = repo.GetAlbumAverageQuality(albumId);

            var albumRepo = albumRepositoryFunc();
            var album = albumRepo.GetById(albumId);
            album.AverageQuality = Convert.ToDecimal(averageQuality);

            uow.Commit();
        }
    }
}
