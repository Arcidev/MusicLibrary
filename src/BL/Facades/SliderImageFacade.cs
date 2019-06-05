using AutoMapper;
using BL.DTO;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using DotVVM.Framework.Storage;
using System;
using System.Collections.Generic;

namespace BL.Facades
{
    public class SliderImageFacade : ImageStorableFacade
    {
        public Func<SliderImagesQuery> SliderImageQueryFunc { get; set; }

        public Func<SliderImageRepository> SliderImageRepositoryFunc { get; set; }

        public Func<IMapper> MapperInstance { get; set; }

        public IEnumerable<SliderImageDTO> GetImages()
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = SliderImageQueryFunc();
                return query.Execute();
            }
        }

        public SliderImageDTO AddSliderImage(SliderImageEditDTO sliderImage, UploadedFile file, IUploadedFileStorage storage)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = MapperInstance().Map<SliderImage>(sliderImage);
                SetImageFile(entity, file, storage);

                var repo = SliderImageRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();
                return MapperInstance().Map<SliderImageDTO>(entity);
            }
        }
    }
}
