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

namespace BL.Facades
{
    public class BandFacade : ImageStorableFacade
    {
        public Func<BandRepository> BandRepositoryFunc { get; set; }

        public Func<BandReviewRepository> BandReviewRepositoryFunc { get; set; }

        public Func<BandsQuery<BandDTO>> BandsQueryBandFunc { get; set; }

        public Func<BandsQuery<BandInfoDTO>> BandsQueryBandInfoFunc { get; set; }

        public Func<BandAlbumsQuery> BandAlbumsQueryFunc { get; set; }

        public Func<BandReviewsQuery> BandReviewsQueryFunc { get; set; }

        public Func<UserBandReviewsQuery> UserBandReviewsQueryFunc { get; set; }

        public Func<BandMembersQuery> BandMembersQueryFunc { get; set; }

        public Func<BandInfoesQuery> BandInfoesQueryFunc { get; set; }

        public IEnumerable<BandInfoDTO> GetBandInfoes()
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = BandInfoesQueryFunc();
                return query.Execute();
            }
        }

        public void ApproveBands(IEnumerable<int> bandIds, bool approved)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = BandRepositoryFunc();
                var bands = repo.GetByIds(bandIds);
                foreach (var band in bands)
                    band.Approved = approved;

                uow.Commit();
            }
        }

        public BandDTO AddBand(BandBaseDTO band, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<Band>(band);
                entity.CreateDate = DateTime.Now;
                SetImageFile(entity, file, storage);

                var repo = BandRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();
                return Mapper.Map<BandDTO>(entity);
            }
        }

        public void EditBand(BandEditDTO band, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = BandRepositoryFunc();
                var entity = repo.GetById(band.Id);
                Mapper.Map(band, entity);
                SetImageFile(entity, file, storage);

                uow.Commit();
            }
        }

        public void LoadBands(GridViewDataSet<BandInfoDTO> dataSet, string filter = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = BandsQueryBandInfoFunc();
                query.Filter = filter;

                FillDataSet(dataSet, query);
            }
        }

        public IEnumerable<AlbumDTO> GetBandAlbums(int bandId, int? excludeAlbumId = null, int? count = null, bool? approved = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = BandAlbumsQueryFunc();
                query.BandId = bandId;
                query.ExcludeAlbumId = excludeAlbumId;
                query.Approved = approved;
                query.Take = count;

                return query.Execute();
            }
        }

        public IEnumerable<BandDTO> GetBands()
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = BandsQueryBandFunc();
                query.Approved = true;
                return query.Execute();
            }
        }

        public BandDTO GetBand(int id, bool includeAlbums = true, bool includeMembers = true)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = BandRepositoryFunc();
                var entity = repo.GetById(id);
                IsNotNull(entity, ErrorMessages.BandNotExist);

                var dto = Mapper.Map<BandDTO>(entity);
                if (includeAlbums)
                    dto.Albums = Mapper.Map<IEnumerable<AlbumDTO>>(entity.Albums.Where(x => x.Approved));

                if (includeMembers)
                    dto.Members = GetBandMembers(entity.Id);

                return dto;
            }
        }

        public IEnumerable<ArtistDTO> GetBandMembers(int bandId)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = BandMembersQueryFunc();
                query.BandId = bandId;
                query.Approved = true;

                return query.Execute();
            }
        }

        public void LoadReviews(int bandId, GridViewDataSet<ReviewDTO> dataSet)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = BandReviewsQueryFunc();
                query.BandId = bandId;

                FillDataSet(dataSet, query);
            }
        }

        public void LoadUserReviews(int userId, GridViewDataSet<UserBandReviewDTO> dataSet)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = UserBandReviewsQueryFunc();
                query.UserId = userId;

                FillDataSet(dataSet, query);
            }
        }

        public void EditUserReview(int reviewId, ReviewEditDTO editedReview)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = BandReviewRepositoryFunc();
                var entity = repo.GetById(reviewId);
                IsNotNull(entity, ErrorMessages.ReviewNotExist);

                if (entity.CreatedById != editedReview.CreatedById)
                    throw new UIException(ErrorMessages.NotUserReview);

                Mapper.Map(editedReview, entity);
                entity.EditDate = DateTime.Now;

                uow.Commit();
            }
        }

        public void AddReview(BandReviewCreateDTO review)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<BandReview>(review);
                entity.CreateDate = DateTime.Now;
                entity.EditDate = DateTime.Now;

                var repo = BandReviewRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();
            }
        }
    }
}
