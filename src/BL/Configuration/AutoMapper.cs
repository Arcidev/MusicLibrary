﻿using AutoMapper;
using BL.DTO;
using DAL.Entities;

namespace BL.Configuration
{
    public static class AutoMapper
    {
        public static void Init()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<User, UserDTO>();
                config.CreateMap<UserCreateDTO, User>();
                config.CreateMap<UserEditDTO, User>();

                config.CreateMap<StorageFile, StorageFileDTO>();

                config.CreateMap<Band, BandDTO>()
                    .ForMember(target => target.Albums, action => action.Ignore());
                config.CreateMap<BandCreateDTO, Band>();

                config.CreateMap<Artist, ArtistDTO>();

                config.CreateMap<SliderImage, SliderImageDTO>();
                config.CreateMap<SliderImageEditDTO, SliderImage>();

                config.CreateMap<Album, AlbumDTO>()
                    .ForMember(target => target.Band, action => action.Ignore());
                config.CreateMap<AlbumCreateDTO, Album>();

                config.CreateMap<Category, CategoryDTO>();
                config.CreateMap<CategoryDTO, Category>();

                config.CreateMap<Song, SongDTO>();
                config.CreateMap<SongCreateDTO, Song>();

                config.CreateMap<AlbumReview, AlbumReviewDTO>()
                    .ForMember(target => target.CreatedByFirstName, action => action.MapFrom(source => source.CreatedBy.FirstName))
                    .ForMember(target => target.CreatedByLastName, action => action.MapFrom(source => source.CreatedBy.LastName))
                    .ForMember(target => target.CreatedByImageStorageFileName, action => action.MapFrom(source => source.CreatedBy.ImageStorageFile != null ? source.CreatedBy.ImageStorageFile.FileName : null));
                config.CreateMap<AlbumReviewCreateDTO, AlbumReview>();

                config.CreateMap<BandReview, BandReviewDTO>()
                    .ForMember(target => target.CreatedByFirstName, action => action.MapFrom(source => source.CreatedBy.FirstName))
                    .ForMember(target => target.CreatedByLastName, action => action.MapFrom(source => source.CreatedBy.LastName))
                    .ForMember(target => target.CreatedByImageStorageFileName, action => action.MapFrom(source => source.CreatedBy.ImageStorageFile != null ? source.CreatedBy.ImageStorageFile.FileName : null));
                config.CreateMap<BandReviewCreateDTO, BandReview>();

                config.CreateMap<UserAlbumCreateDTO, UserAlbum>();
            });
        }
    }
}
