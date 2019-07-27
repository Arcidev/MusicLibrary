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
    public class BandFacade : ImageStorableFacade
    {
        private readonly Func<BandRepository> bandRepositoryFunc;
        private readonly Func<BandReviewRepository> bandReviewRepositoryFunc;
        private readonly Func<BandsQuery<BandDTO>> bandsQueryBandFunc;
        private readonly Func<BandsQuery<BandInfoDTO>> bandsQueryBandInfoFunc;
        private readonly Func<BandAlbumsQuery> bandAlbumsQueryFunc;
        private readonly Func<BandReviewsQuery> bandReviewsQueryFunc;
        private readonly Func<UserBandReviewsQuery> userBandReviewsQueryFunc;
        private readonly Func<BandMembersQuery> bandMembersQueryFunc;
        private readonly Func<BandInfoesQuery> bandInfoesQueryFunc;

        public BandFacade(Func<BandRepository> bandRepositoryFunc,
            Func<BandReviewRepository> bandReviewRepositoryFunc,
            Func<BandsQuery<BandDTO>> bandsQueryBandFunc,
            Func<BandsQuery<BandInfoDTO>> bandsQueryBandInfoFunc,
            Func<BandAlbumsQuery> bandAlbumsQueryFunc,
            Func<BandReviewsQuery> bandReviewsQueryFunc,
            Func<UserBandReviewsQuery> userBandReviewsQueryFunc,
            Func<BandMembersQuery> bandMembersQueryFunc,
            Func<BandInfoesQuery> bandInfoesQueryFunc,
            IMapper mapper,
            Lazy<StorageFileFacade> storageFileFacade,
            Func<IUnitOfWorkProvider> uowProvider) : base(mapper, storageFileFacade, uowProvider)
        {
            this.bandRepositoryFunc = bandRepositoryFunc;
            this.bandReviewRepositoryFunc = bandReviewRepositoryFunc;
            this.bandsQueryBandFunc = bandsQueryBandFunc;
            this.bandsQueryBandInfoFunc = bandsQueryBandInfoFunc;
            this.bandAlbumsQueryFunc = bandAlbumsQueryFunc;
            this.bandReviewsQueryFunc = bandReviewsQueryFunc;
            this.userBandReviewsQueryFunc = userBandReviewsQueryFunc;
            this.bandMembersQueryFunc = bandMembersQueryFunc;
            this.bandInfoesQueryFunc = bandInfoesQueryFunc;
        }

        public IEnumerable<BandInfoDTO> GetBandInfoes()
        {
            using var uow = uowProviderFunc().Create();
            var query = bandInfoesQueryFunc();
            return query.Execute();
        }

        public void ApproveBands(IEnumerable<int> bandIds, bool approved)
        {
            using var uow = uowProviderFunc().Create();
            var repo = bandRepositoryFunc();
            var bands = repo.GetByIds(bandIds);
            foreach (var band in bands)
                band.Approved = approved;

            uow.Commit();
        }

        public BandDTO AddBand(BandBaseDTO band, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var entity = mapper.Map<Band>(band);
            entity.CreateDate = DateTime.Now;
            SetImageFile(entity, file, storage);

            var repo = bandRepositoryFunc();
            repo.Insert(entity);

            uow.Commit();
            return mapper.Map<BandDTO>(entity);
        }

        public void EditBand(BandEditDTO band, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var repo = bandRepositoryFunc();
            var entity = repo.GetById(band.Id);
            mapper.Map(band, entity);
            SetImageFile(entity, file, storage);

            uow.Commit();
        }

        public void LoadBands(GridViewDataSet<BandInfoDTO> dataSet, string filter = null)
        {
            using var uow = uowProviderFunc().Create();
            var query = bandsQueryBandInfoFunc();
            query.Filter = filter;

            FillDataSet(dataSet, query);
        }

        public IEnumerable<AlbumDTO> GetBandAlbums(int bandId, int? excludeAlbumId = null, int? count = null, bool? approved = null)
        {
            using var uow = uowProviderFunc().Create();
            var query = bandAlbumsQueryFunc();
            query.BandId = bandId;
            query.ExcludeAlbumId = excludeAlbumId;
            query.Approved = approved;
            query.Take = count;

            return query.Execute();
        }

        public IEnumerable<BandDTO> GetBands()
        {
            using var uow = uowProviderFunc().Create();
            var query = bandsQueryBandFunc();
            query.Approved = true;
            return query.Execute();
        }

        public BandDTO GetBand(int id, bool includeAlbums = true, bool includeMembers = true)
        {
            using var uow = uowProviderFunc().Create();
            var repo = bandRepositoryFunc();
            var entity = repo.GetById(id);
            IsNotNull(entity, ErrorMessages.BandNotExist);

            var dto = mapper.Map<BandDTO>(entity);
            if (includeAlbums)
                dto.Albums = mapper.Map<IEnumerable<AlbumDTO>>(entity.Albums.Where(x => x.Approved));

            if (includeMembers)
                dto.Members = GetBandMembers(entity.Id);

            return dto;
        }

        public IEnumerable<ArtistDTO> GetBandMembers(int bandId)
        {
            using var uow = uowProviderFunc().Create();
            var query = bandMembersQueryFunc();
            query.BandId = bandId;
            query.Approved = true;

            return query.Execute();
        }

        public void LoadReviews(int bandId, GridViewDataSet<ReviewDTO> dataSet)
        {
            using var uow = uowProviderFunc().Create();
            var query = bandReviewsQueryFunc();
            query.BandId = bandId;

            FillDataSet(dataSet, query);
        }

        public void LoadUserReviews(int userId, GridViewDataSet<UserBandReviewDTO> dataSet)
        {
            using var uow = uowProviderFunc().Create();
            var query = userBandReviewsQueryFunc();
            query.UserId = userId;

            FillDataSet(dataSet, query);
        }

        public void EditUserReview(int reviewId, ReviewEditDTO editedReview)
        {
            using var uow = uowProviderFunc().Create();
            var repo = bandReviewRepositoryFunc();
            var entity = repo.GetById(reviewId);
            IsNotNull(entity, ErrorMessages.ReviewNotExist);

            if (entity.CreatedById != editedReview.CreatedById)
                throw new UIException(ErrorMessages.NotUserReview);

            mapper.Map(editedReview, entity);
            entity.EditDate = DateTime.Now;

            uow.Commit();
        }

        public void AddReview(BandReviewCreateDTO review)
        {
            using var uow = uowProviderFunc().Create();
            var entity = mapper.Map<BandReview>(review);
            entity.CreateDate = DateTime.Now;
            entity.EditDate = DateTime.Now;

            var repo = bandReviewRepositoryFunc();
            repo.Insert(entity);

            uow.Commit();
        }
    }
}
