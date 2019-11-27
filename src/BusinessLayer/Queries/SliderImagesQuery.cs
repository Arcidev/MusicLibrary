﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class SliderImagesQuery : AppQuery<SliderImageDTO>
    {
        public SliderImagesQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<SliderImageDTO> GetQueryable()
        {
            return Context.SliderImages.ProjectTo<SliderImageDTO>(mapperConfig);
        }
    }
}