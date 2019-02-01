using AutoMapper;
using BL.DTO;
using BL.Queries;
using BL.Repositories;
using BL.Resources;
using DAL.Entities;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Storage;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Facades
{
    public class AlbumFacade : ImageStorableFacade
    {
        public Func<RecentAlbumsQuery> RecentlyAddedAlbumsQueryFunc { get; set; }

        public Func<FeaturedAlbumsQuery> FeaturedAlbumsQueryFunc { get; set; }

        public Func<AlbumRepository> AlbumRepositoryFunc { get; set; }

        public Func<AlbumSongsQuery<SongDTO>> AlbumSongsQuerySongFunc { get; set; }

        public Func<AlbumSongsQuery<SongInfoDTO>> AlbumSongsQuerySongInfoFunc { get; set; }

        public Func<AlbumsQuery<AlbumDTO>> AlbumsQueryAlbumFunc { get; set; }

        public Func<AlbumsQuery<AlbumInfoDTO>> AlbumsQueryAlbumInfoFunc { get; set; }

        public Func<AlbumsQuery<AlbumBandInfoDTO>> AlbumsQueryAlbumBandInfoFunc { get; set; }

        public Func<AlbumReviewRepository> AlbumReviewRepositoryFunc { get; set; }

        public Func<AlbumReviewsQuery> AlbumReviewsQueryFunc { get; set; }

        public Func<UserAlbumReviewsQuery> UserAlbumReviewsQueryFunc { get; set; }

        public Func<AlbumSongRepository> AlbumSongRepositoryFunc { get; set; }

        public Func<UserAlbumRepository> UserAlbumRepositoryFunc { get; set; }

        public Func<UserAlbumsQuery> UserAlbumsQueryFunc { get; set; }

        public Func<IsInUserAlbumCollectionQuery> IsInUserAlbumCollectionQueryFunc { get; set; }

        public AlbumDTO AddAlbum(AlbumCreateDTO album, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<Album>(album);
                entity.CreateDate = DateTime.Now;
                SetImageFile(entity, file, storage);

                if (album.AddedSongs != null)
                {
                    entity.AlbumSongs = album.AddedSongs.Select(songId => new AlbumSong()
                    {
                        SongId = songId
                    }).ToList();
                }

                var repo = AlbumRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();
                return Mapper.Map<AlbumDTO>(entity);
            }
        }

        public void ApproveAlbums(IEnumerable<int> albumIds, bool approved)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = AlbumRepositoryFunc();
                var albums = repo.GetByIds(albumIds);
                foreach (var album in albums)
                    album.Approved = approved;

                uow.Commit();
            }
        }

        public void EditAlbum(AlbumEditDTO album, UploadedFile imageFile = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = AlbumRepositoryFunc();
                var entity = repo.GetById(album.Id);
                IsNotNull(entity, ErrorMessages.AlbumNotExist);

                Mapper.Map(album, entity);
                SetImageFile(entity, imageFile, storage);
                
                if (album.RemovedSongs != null)
                {
                    var toRemove = entity.AlbumSongs.Where(x => album.RemovedSongs.Contains(x.SongId)).ToList();
                    var albumSongRepo = AlbumSongRepositoryFunc();
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
        }

        public IEnumerable<SongDTO> GetAlbumSongs(int albumId)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = AlbumSongsQuerySongFunc();
                query.AlbumId = albumId;
                query.Approved = true;

                return query.Execute();
            }
        }

        public IEnumerable<AlbumBandInfoDTO> GetAlbumBandInfoes(int? songId = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = AlbumsQueryAlbumBandInfoFunc();
                query.Approved = true;
                query.SongId = songId;

                return query.Execute();
            }
        }

        public IEnumerable<SongInfoDTO> GetAlbumSongInfoes(int albumId)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = AlbumSongsQuerySongInfoFunc();
                query.AlbumId = albumId;
                query.Approved = true;

                return query.Execute();
            }
        }

        public AlbumDTO GetAlbum(int id, bool includeBandInfo = true, bool includeSongs = true)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = AlbumRepositoryFunc();
                var entity = repo.GetById(id);
                IsNotNull(entity, ErrorMessages.AlbumNotExist);

                var dto = Mapper.Map<AlbumDTO>(entity);
                if (includeBandInfo)
                    dto.Band = Mapper.Map<BandDTO>(entity.Band);

                if (includeSongs)
                    dto.Songs = GetAlbumSongs(entity.Id);

                return dto;
            }
        }

        public IEnumerable<AlbumDTO> GetAlbums(int? categoryId = null, string filter = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = AlbumsQueryAlbumFunc();
                query.CategoryId = categoryId;
                query.Filter = filter;
                query.IncludeBandFilter = true;
                query.Approved = true;

                return query.Execute();
            }
        }

        public void LoadAlbums(GridViewDataSet<AlbumInfoDTO> dataSet, string filter = null, bool? approved = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = AlbumsQueryAlbumInfoFunc();
                query.Filter = filter;
                query.Approved = approved;

                FillDataSet(dataSet, query);
            }
        }

        public void LoadUserAlbumsCollection(int userId, GridViewDataSet<AlbumInfoDTO> dataSet, string filter = null)
        {
            LoadAlbums(dataSet, filter, true);
            using (var uow = UowProviderFunc().Create())
            {
                var collectionQuery = IsInUserAlbumCollectionQueryFunc();
                collectionQuery.UserId = userId;
                collectionQuery.AlbumIds = dataSet.Items.Select(x => x.AlbumId);
                var collection = collectionQuery.Execute();

                foreach(var album in dataSet.Items)
                {
                    if (collection.Contains(album.AlbumId))
                        album.HasInCollection = true;
                }
            }
        }

        public void AddReview(AlbumReviewCreateDTO review)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<AlbumReview>(review);
                entity.CreateDate = DateTime.Now;
                entity.EditDate = DateTime.Now;

                var repo = AlbumReviewRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();

                UpdateAlbumQuality(review.AlbumId);
                uow.Commit();
            }
        }

        public void LoadReviews(int albumId, GridViewDataSet<ReviewDTO> dataSet)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = AlbumReviewsQueryFunc();
                query.AlbumId = albumId;

                FillDataSet(dataSet, query);
            }
        }

        public void EditUserReview(int reviewId, ReviewEditDTO editedReview)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = AlbumReviewRepositoryFunc();
                var entity = repo.GetById(reviewId);
                IsNotNull(entity, ErrorMessages.ReviewNotExist);

                if (entity.CreatedById != editedReview.CreatedById)
                    throw new UIException(ErrorMessages.NotUserReview);

                var qualityUpdated = editedReview.Quality != entity.Quality;
                Mapper.Map(editedReview, entity);
                entity.EditDate = DateTime.Now;
                uow.Commit();

                if (!qualityUpdated)
                    return;

                UpdateAlbumQuality(entity.AlbumId);
                uow.Commit();
            }
        }

        public void LoadUserReviews(int userId, GridViewDataSet<UserAlbumReviewDTO> dataSet)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = UserAlbumReviewsQueryFunc();
                query.UserId = userId;

                FillDataSet(dataSet, query);
            }
        }

        public void AddSongToAlbum(int albumId, int songId)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = AlbumSongRepositoryFunc();
                repo.Insert(new AlbumSong()
                {
                    AlbumId = albumId,
                    SongId = songId
                });

                uow.Commit();
            }
        }

        public IEnumerable<AlbumDTO> GetRecentAlbums(int count)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = RecentlyAddedAlbumsQueryFunc();
                query.Take = count;

                return query.Execute();
            }
        }

        public IEnumerable<AlbumDTO> GetFeaturedAlbums(int count)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = FeaturedAlbumsQueryFunc();
                query.Take = count;

                return query.Execute();
            }
        }

        public async Task AddAlbumToUserCollection(UserAlbumCreateDTO userAlbum)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = UserAlbumRepositoryFunc();
                if (await repo.GetUserAlbum(userAlbum.UserId, userAlbum.AlbumId) != null)
                    return;

                var entity = Mapper.Map<UserAlbum>(userAlbum);
                repo.Insert(entity);

                uow.Commit();
            }
        }

        public async Task RemoveAlbumFromUserCollection(int userId, int albumId)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = UserAlbumRepositoryFunc();
                var entity = await repo.GetUserAlbum(userId, albumId);
                if (entity == null)
                    return;

                repo.Delete(entity);
                uow.Commit();
            }
        }

        public async Task<bool> HasUserInCollection(int userId, int albumId)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = UserAlbumRepositoryFunc();
                var entity = await repo.GetUserAlbum(userId, albumId);

                return entity != null;
            }
        }

        public IEnumerable<AlbumDTO> GetUserAlbums(int userId)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = UserAlbumsQueryFunc();
                query.UserId = userId;

                return query.Execute();
            }
        }

        private void UpdateAlbumQuality(int albumId)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = AlbumReviewRepositoryFunc();
                var averageQuality = repo.GetAlbumAverageQuality(albumId);

                var albumRepo = AlbumRepositoryFunc();
                var album = albumRepo.GetById(albumId);
                album.AverageQuality = Convert.ToDecimal(averageQuality);

                uow.Commit();
            }
        }
    }
}
