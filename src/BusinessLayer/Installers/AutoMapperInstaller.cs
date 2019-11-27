﻿using AutoMapper;
using BusinessLayer.DTO;
using DataLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BusinessLayer.Installers
{
    /// <summary>
    /// AutoMapper install helper
    /// </summary>
    public static class AutoMapperInstaller
    {
        /// <summary>
        /// Extends <see cref="IServiceCollection"/> to allow chained automapper installation
        /// </summary>
        /// <param name="services">DI container</param>
        /// <returns>Passed DI container to allow chaining</returns>
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var autoMapperConfig = new MapperConfiguration(config => {
                config.CreateMap<User, UserDTO>();
                config.CreateMap<User, UserInfoDTO>();
                config.CreateMap<UserCreateDTO, User>();
                config.CreateMap<UserEditDTO, User>();

                config.CreateMap<StorageFile, StorageFileDTO>();

                config.CreateMap<Band, BandInfoDTO>();
                config.CreateMap<Band, BandDTO>()
                    .ForMember(target => target.Albums, action => action.Ignore());
                config.CreateMap<BandBaseDTO, Band>();

                config.CreateMap<Artist, ArtistDTO>();

                config.CreateMap<SliderImage, SliderImageDTO>();
                config.CreateMap<SliderImageEditDTO, SliderImage>();

                config.CreateMap<Album, AlbumBandInfoDTO>()
                    .ForMember(target => target.AlbumId, action => action.MapFrom(source => source.Id))
                    .ForMember(target => target.AlbumName, action => action.MapFrom(source => source.Name))
                    .ForMember(target => target.BandName, action => action.MapFrom(source => source.Band.Name));
                config.CreateMap<Album, AlbumInfoDTO>()
                    .ForMember(target => target.AlbumId, action => action.MapFrom(source => source.Id))
                    .ForMember(target => target.BandName, action => action.MapFrom(source => source.Band.Name))
                    .ForMember(target => target.Category, action => action.MapFrom(source => source.Category))
                    .ForMember(target => target.AlbumName, action => action.MapFrom(source => source.Name));
                config.CreateMap<Album, AlbumDTO>()
                    .ForMember(target => target.Band, action => action.Ignore());
                config.CreateMap<AlbumCreateDTO, Album>();
                config.CreateMap<AlbumEditDTO, Album>();

                config.CreateMap<Category, CategoryDTO>();
                config.CreateMap<CategoryDTO, Category>();

                config.CreateMap<Song, SongDTO>();
                config.CreateMap<Song, SongInfoDTO>();
                config.CreateMap<SongCreateDTO, Song>();
                config.CreateMap<SongEditDTO, Song>();

                config.CreateMap<AlbumReview, ReviewDTO>()
                    .ForMember(target => target.CreatedByFirstName, action => action.MapFrom(source => source.CreatedBy.FirstName))
                    .ForMember(target => target.CreatedByLastName, action => action.MapFrom(source => source.CreatedBy.LastName))
                    .ForMember(target => target.CreatedByImageStorageFileName, action => action.MapFrom(source => source.CreatedBy.ImageStorageFile.FileName));
                config.CreateMap<AlbumReviewCreateDTO, AlbumReview>();

                config.CreateMap<BandReview, ReviewDTO>()
                    .ForMember(target => target.CreatedByFirstName, action => action.MapFrom(source => source.CreatedBy.FirstName))
                    .ForMember(target => target.CreatedByLastName, action => action.MapFrom(source => source.CreatedBy.LastName))
                    .ForMember(target => target.CreatedByImageStorageFileName, action => action.MapFrom(source => source.CreatedBy.ImageStorageFile.FileName));
                config.CreateMap<BandReviewCreateDTO, BandReview>();

                config.CreateMap<UserAlbumCreateDTO, UserAlbum>();

                config.CreateMap<AlbumReview, UserAlbumReviewDTO>();
                config.CreateMap<BandReview, UserBandReviewDTO>();

                config.CreateMap<ReviewEditDTO, BandReview>();
                config.CreateMap<ReviewEditDTO, AlbumReview>();
            });

            return services.AddSingleton<IConfigurationProvider>(autoMapperConfig)
                .AddSingleton<IMapper, Mapper>();
        }
    }
}