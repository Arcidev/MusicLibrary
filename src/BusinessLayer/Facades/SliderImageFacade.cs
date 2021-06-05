using BusinessLayer.DTO;
using BusinessLayer.Queries;
using BusinessLayer.Repositories;
using DataLayer.Entities;
using DotVVM.Core.Storage;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Facades
{
    public class SliderImageFacade : ImageStorableFacade
    {
        private readonly Func<SliderImageRepository> sliderImageRepositoryFunc;
        private readonly Func<SliderImagesQuery> sliderImageQueryFunc;

        public SliderImageFacade(Func<SliderImageRepository> sliderImageRepositoryFunc,
            Func<SliderImagesQuery> sliderImageQueryFunc,
            Lazy<StorageFileFacade> storageFileFacade,
            Func<IUnitOfWorkProvider> uowProvider) : base(storageFileFacade, uowProvider)
        {
            this.sliderImageRepositoryFunc = sliderImageRepositoryFunc;
            this.sliderImageQueryFunc = sliderImageQueryFunc;
        }

        public async Task<IList<SliderImageDTO>> GetImages()
        {
            using var uow = uowProviderFunc().Create();
            var query = sliderImageQueryFunc();
            return await query.ExecuteAsync();
        }

        public async Task<SliderImageDTO> AddSliderImage(SliderImageEditDTO sliderImage, UploadedFile file, IUploadedFileStorage storage)
        {
            using var uow = uowProviderFunc().Create();
            var entity = sliderImage.Adapt<SliderImage>();
            await SetImageFileAsync(entity, file, storage);

            var repo = sliderImageRepositoryFunc();
            repo.Insert(entity);

            await uow.CommitAsync();
            return entity.Adapt<SliderImageDTO>();
        }
    }
}
