using AutoMapper;
using BusinessLayer.DTO;
using BusinessLayer.Queries;
using BusinessLayer.Repositories;
using DataLayer.Entities;
using DotVVM.Framework.Storage;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Facades
{
    public class SliderImageFacade : ImageStorableFacade
    {
        private readonly Func<SliderImageRepository> sliderImageRepositoryFunc;
        private readonly Func<SliderImagesQuery> sliderImageQueryFunc;

        public SliderImageFacade(Func<SliderImageRepository> sliderImageRepositoryFunc,
            Func<SliderImagesQuery> sliderImageQueryFunc,
            IMapper mapper,
            Lazy<StorageFileFacade> storageFileFacade,
            Func<IUnitOfWorkProvider> uowProvider) : base(mapper, storageFileFacade, uowProvider)
        {
            this.sliderImageRepositoryFunc = sliderImageRepositoryFunc;
            this.sliderImageQueryFunc = sliderImageQueryFunc;
        }

        public IEnumerable<SliderImageDTO> GetImages()
        {
            using var uow = uowProviderFunc().Create();
            var query = sliderImageQueryFunc();
            return query.Execute();
        }

        public SliderImageDTO AddSliderImage(SliderImageEditDTO sliderImage, UploadedFile file, IUploadedFileStorage storage)
        {
            using var uow = uowProviderFunc().Create();
            var entity = mapper.Map<SliderImage>(sliderImage);
            SetImageFile(entity, file, storage);

            var repo = sliderImageRepositoryFunc();
            repo.Insert(entity);

            uow.Commit();
            return mapper.Map<SliderImageDTO>(entity);
        }
    }
}
