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
    public class AlbumFacade : BaseFacade
    {
        public Func<RecentAlbumsQuery> RecentlyAddedAlbumsQueryFunc { get; set; }

        public Func<FeaturedAlbumsQuery> FeaturedAlbumsQueryFunc { get; set; }

        public Func<AlbumRepository> AlbumRepositoryFunc { get; set; }

        public Func<AlbumSongsQuery> AlbumSongsQueryFunc { get; set; }

        public Func<AlbumReviewRepository> AlbumReviewRepositoryFunc { get; set; }

        public StorageFileFacade StorageFileFacade { get; set; }

        public AlbumDTO AddAlbum(AlbumCreateDTO album, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<Album>(album);
                entity.CreateDate = DateTime.Now;

                if (file != null && storage != null)
                {
                    var fileName = StorageFileFacade.SaveFile(file, storage);
                    entity.ImageStorageFile = new StorageFile()
                    {
                        DisplayName = file.FileName,
                        FileName = fileName
                    };
                }

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
    }
}
