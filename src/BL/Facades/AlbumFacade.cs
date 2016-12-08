﻿using AutoMapper;
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
using System.Threading.Tasks;

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

        public Func<UserAlbumReviewsQuery> UserAlbumReviewsQueryFunc { get; set; }

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

        public IEnumerable<SongDTO> GetAlbumSongs(int albumId)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = AlbumSongsQueryFunc();
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
                var query = AlbumsQueryFunc();
                query.CategoryId = categoryId;
                query.Filter = filter;

                return query.Execute();
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
            }
        }

        public IEnumerable<AlbumReviewDTO> GetReviews(int albumId)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = AlbumReviewsQueryFunc();
                query.AlbumId = albumId;

                return query.Execute();
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

                Mapper.Map(editedReview, entity);
                entity.EditDate = DateTime.Now;

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

        public async Task RemoveAlbumFromUserCollection(int userId, int albumId)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = UserAlbumRepositoryFunc();
                var entity = await repo.GetUserAlbum(userId, albumId);
                IsNotNull(entity, ErrorMessages.UserAlbumNotExist);

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
    }
}
