using AutoMapper;
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
                config.CreateMap<User, UserDTO>()
                    .ForMember(target => target.Password, action => action.Ignore());
                config.CreateMap<UserDTO, User>()
                    .ForMember(target => target.ImageStorageFile, action => action.Ignore())
                    .ForMember(target => target.PasswordHash, action => action.Ignore())
                    .ForMember(target => target.PasswordSalt, action => action.Ignore());

                config.CreateMap<StorageFile, StorageFileDTO>();

                config.CreateMap<Band, BandDTO>();
                config.CreateMap<BandCreateDTO, Band>();

                config.CreateMap<SliderImage, SliderImageDTO>();

                config.CreateMap<Album, AlbumDTO>()
                    .ForMember(target => target.Band, action => action.Ignore());
                config.CreateMap<AlbumCreateDTO, Album>();

                config.CreateMap<Category, CategoryDTO>();
                config.CreateMap<CategoryDTO, Category>();

                config.CreateMap<Song, SongDTO>();

                config.CreateMap<AlbumReview, AlbumReviewDTO>();
                config.CreateMap<AlbumReviewDTO, AlbumReview>()
                    .ForMember(target => target.CreateDate, action => action.Ignore());

                config.CreateMap<UserAlbumCreateDTO, UserAlbum>();
            });
        }
    }
}
