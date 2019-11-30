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
using System.Threading.Tasks;

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

        public async Task<IList<BandInfoDTO>> GetBandInfoesAsync()
        {
            using var uow = uowProviderFunc().Create();
            var query = bandInfoesQueryFunc();
            return await query.ExecuteAsync();
        }

        public async Task ApproveBandsAsync(IEnumerable<int> bandIds, bool approved)
        {
            using var uow = uowProviderFunc().Create();
            var repo = bandRepositoryFunc();
            var bands = await repo.GetByIdsAsync(bandIds);
            foreach (var band in bands)
                band.Approved = approved;

            await uow.CommitAsync();
        }

        public async Task<BandDTO> AddBandAsync(BandBaseDTO band, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var entity = mapper.Map<Band>(band);
            entity.CreateDate = DateTime.Now;
            await SetImageFileAsync(entity, file, storage);

            var repo = bandRepositoryFunc();
            repo.Insert(entity);

            await uow.CommitAsync();
            return mapper.Map<BandDTO>(entity);
        }

        public async Task EditBandAsync(BandEditDTO band, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var repo = bandRepositoryFunc();
            var entity = await repo.GetByIdAsync(band.Id);
            mapper.Map(band, entity);
            await SetImageFileAsync(entity, file, storage);

            await uow.CommitAsync();
        }

        public async Task LoadBandsAsync(GridViewDataSet<BandInfoDTO> dataSet, string filter = null)
        {
            using var uow = uowProviderFunc().Create();
            var query = bandsQueryBandInfoFunc();
            query.Filter = filter;

            await FillDataSetAsync(dataSet, query);
        }

        public async Task<IList<AlbumDTO>> GetBandAlbumsAsync(int bandId, int? excludeAlbumId = null, int? count = null, bool? approved = null)
        {
            using var uow = uowProviderFunc().Create();
            var query = bandAlbumsQueryFunc();
            query.BandId = bandId;
            query.ExcludeAlbumId = excludeAlbumId;
            query.Approved = approved;
            query.Take = count;

            return await query.ExecuteAsync();
        }

        public async Task<IList<BandDTO>> GetBandsAsync()
        {
            using var uow = uowProviderFunc().Create();
            var query = bandsQueryBandFunc();
            query.Approved = true;

            return await query.ExecuteAsync();
        }

        public async Task<BandDTO> GetBandAsync(int id, bool includeAlbums = true, bool includeMembers = true)
        {
            using var uow = uowProviderFunc().Create();
            var repo = bandRepositoryFunc();
            var entity = await repo.GetByIdAsync(id);
            IsNotNull(entity, ErrorMessages.BandNotExist);

            var dto = mapper.Map<BandDTO>(entity);
            if (includeAlbums)
                dto.Albums = mapper.Map<IEnumerable<AlbumDTO>>(entity.Albums.Where(x => x.Approved));

            if (includeMembers)
                dto.Members = await GetBandMembersAsync(entity.Id);

            return dto;
        }

        public async Task<IList<ArtistDTO>> GetBandMembersAsync(int bandId)
        {
            using var uow = uowProviderFunc().Create();
            var query = bandMembersQueryFunc();
            query.BandId = bandId;
            query.Approved = true;

            return await query.ExecuteAsync();
        }

        public async Task LoadReviewsAsync(int bandId, GridViewDataSet<ReviewDTO> dataSet)
        {
            using var uow = uowProviderFunc().Create();
            var query = bandReviewsQueryFunc();
            query.BandId = bandId;

            await FillDataSetAsync(dataSet, query);
        }

        public async Task LoadUserReviewsAsync(int userId, GridViewDataSet<UserBandReviewDTO> dataSet)
        {
            using var uow = uowProviderFunc().Create();
            var query = userBandReviewsQueryFunc();
            query.UserId = userId;

            await FillDataSetAsync(dataSet, query);
        }

        public async Task EditUserReviewAsync(int reviewId, ReviewEditDTO editedReview)
        {
            using var uow = uowProviderFunc().Create();
            var repo = bandReviewRepositoryFunc();
            var entity = await repo.GetByIdAsync(reviewId);
            IsNotNull(entity, ErrorMessages.ReviewNotExist);

            if (entity.CreatedById != editedReview.CreatedById)
                throw new UIException(ErrorMessages.NotUserReview);

            mapper.Map(editedReview, entity);
            entity.EditDate = DateTime.Now;

            await uow.CommitAsync();
        }

        public async Task AddReviewAsync(BandReviewCreateDTO review)
        {
            using var uow = uowProviderFunc().Create();
            var entity = mapper.Map<BandReview>(review);
            entity.CreateDate = DateTime.Now;
            entity.EditDate = DateTime.Now;

            var repo = bandReviewRepositoryFunc();
            repo.Insert(entity);

            await uow.CommitAsync();
        }
    }
}
