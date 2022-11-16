using BusinessLayer.DTO;
using DataLayer.Entities;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Installers
{
    /// <summary>
    /// AutoMapper install helper
    /// </summary>
    public static class MapperInstaller
    {
        /// <summary>
        /// Extends <see cref="IServiceCollection"/> to allow chained automapper installation
        /// </summary>
        /// <param name="services">DI container</param>
        /// <returns>Passed DI container to allow chaining</returns>
        public static void ConfigureMapper()
        {
            TypeAdapterConfig<Band, BandDTO>.NewConfig()
                .Ignore(dest => dest.Albums);

            TypeAdapterConfig<Album, AlbumBandInfoDTO>.NewConfig()
                .Map(dest => dest.AlbumId, src => src.Id)
                .Map(dest => dest.AlbumName, src => src.Name)
                .Map(dest => dest.BandName, src => src.Band.Name);

            TypeAdapterConfig<Album, AlbumInfoDTO>.NewConfig()
                .Map(dest => dest.AlbumId, src => src.Id)
                .Map(dest => dest.AlbumName, src => src.Name)
                .Map(dest => dest.BandName, src => src.Band.Name)
                .Map(dest => dest.Category, src => src.Category);

            TypeAdapterConfig<Album, AlbumDTO>.NewConfig()
                .Ignore(dest => dest.Band);

            TypeAdapterConfig<AlbumReview, ReviewDTO>.NewConfig()
                .Map(dest => dest.CreatedByFirstName, src => src.CreatedBy.FirstName)
                .Map(dest => dest.CreatedByLastName, src => src.CreatedBy.LastName)
                .Map(dest => dest.CreatedByImageStorageFileName, src => src.CreatedBy.ImageStorageFile.FileName);

            TypeAdapterConfig<BandReview, ReviewDTO>.NewConfig()
                .Map(dest => dest.CreatedByFirstName, src => src.CreatedBy.FirstName)
                .Map(dest => dest.CreatedByLastName, src => src.CreatedBy.LastName)
                .Map(dest => dest.CreatedByImageStorageFileName, src => src.CreatedBy.ImageStorageFile.FileName);
        }
    }
}
