using AutoMapper;
using BL.DTO;
using BL.Queries;
using BL.Repositories;
using BL.Resources;
using DAL.Entities;
using DotVVM.Framework.Storage;
using System;
using System.Collections.Generic;

namespace BL.Facades
{
    public class AlbumFacade : ImageStorableFacade
    {
        public Func<RecentAlbumsQuery> RecentlyAddedAlbumsQueryFunc { get; set; }

        public Func<FeaturedAlbumsQuery> FeaturedAlbumsQueryFunc { get; set; }

        public Func<AlbumRepository> AlbumRepositoryFunc { get; set; }

        public Func<AlbumSongsQuery> AlbumSongsQueryFunc { get; set; }

        public Func<AlbumsQuery> AlbumsQueryFunc { get; set; }

        public Func<AlbumReviewRepository> AlbumReviewRepositoryFunc { get; set; }

        public Func<AlbumReviewsQuery> AlbumReviewsQueryFunc { get; set; }

        public Func<AlbumSongRepository> AlbumSongRepositoryFunc { get; set; }

        public Func<UserAlbumRepository> UserAlbumRepositoryFunc { get; set; }

        public Func<UserAlbumsQuery> UserAlbumsQueryFunc { get; set; }

        public AlbumDTO AddAlbum(AlbumCreateDTO album, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<Album>(album);
                entity.CreateDate = DateTime.Now;
                SetImageFile(entity, file, storage);

                var repo = AlbumRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();
                return Mapper.Map<AlbumDTO>(entity);
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
                {
                    var query = AlbumSongsQueryFunc();
                    query.AlbumId = entity.Id;
                    query.Approved = true;
                    dto.Songs = query.Execute();
                }

                return dto;
            }
        }

        public IEnumerable<AlbumDTO> GetAlbums()
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = AlbumsQueryFunc();
                return query.Execute();
            }
        }

        public void AddReview(AlbumReviewDTO review)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<AlbumReview>(review);
                entity.CreateDate = DateTime.Now;

                var repo = AlbumReviewRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();
            }
        }

        public IEnumerable<AlbumReviewDTO> GetReviews(int? albumId = null, int? userId = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = AlbumReviewsQueryFunc();
                query.AlbumId = albumId;
                query.UserId = userId;

                return query.Execute();
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

        public void AddAlbumToUserCollection(UserAlbumCreateDTO userAlbum)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = UserAlbumRepositoryFunc();
                var entity = Mapper.Map<UserAlbum>(userAlbum);
                repo.Insert(entity);

                uow.Commit();
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
    }
}
