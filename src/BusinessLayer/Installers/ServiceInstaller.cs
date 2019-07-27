using BusinessLayer.DTO;
using BusinessLayer.Queries;
using BusinessLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace BusinessLayer.Installers
{
    /// <summary>
    /// Service install helper
    /// </summary>
    public static class ServiceInstaller
    {
        /// <summary>
        /// Extends <see cref="IServiceCollection"/> to allow chained service installation
        /// </summary>
        /// <param name="services">DI container</param>
        /// <returns>Passed DI container to allow chaining</returns>
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var baseRepository = typeof(BaseRepository<,>);
            var baseQuery = typeof(AppQuery<>);
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t =>  !t.IsAbstract && t.BaseType != null && t.BaseType.IsGenericType && (t.BaseType.GetGenericTypeDefinition() == baseRepository || t.BaseType.GetGenericTypeDefinition() == baseQuery)))
            {
                services.AddTransient(type);
            }

            return services.AddTransient<Func<AlbumRepository>>(provider => () => provider.GetRequiredService<AlbumRepository>())
                .AddTransient<Func<AlbumReviewRepository>>(provider => () => provider.GetRequiredService<AlbumReviewRepository>())
                .AddTransient<Func<AlbumSongRepository>>(provider => () => provider.GetRequiredService<AlbumSongRepository>())
                .AddTransient<Func<BandRepository>>(provider => () => provider.GetRequiredService<BandRepository>())
                .AddTransient<Func<BandReviewRepository>>(provider => () => provider.GetRequiredService<BandReviewRepository>())
                .AddTransient<Func<CategoryRepository>>(provider => () => provider.GetRequiredService<CategoryRepository>())
                .AddTransient<Func<SliderImageRepository>>(provider => () => provider.GetRequiredService<SliderImageRepository>())
                .AddTransient<Func<SongRepository>>(provider => () => provider.GetRequiredService<SongRepository>())
                .AddTransient<Func<StorageFileRepository>>(provider => () => provider.GetRequiredService<StorageFileRepository>())
                .AddTransient<Func<UserAlbumRepository>>(provider => () => provider.GetRequiredService<UserAlbumRepository>())
                .AddTransient<Func<UserRepository>>(provider => () => provider.GetRequiredService<UserRepository>())

                .AddTransient<Func<AlbumReviewsQuery>>(provider => () => provider.GetRequiredService<AlbumReviewsQuery>())
                .AddTransient<Func<AlbumSongsQuery<SongDTO>>>(provider => () => provider.GetRequiredService<AlbumSongsQuery<SongDTO>>())
                .AddTransient<Func<AlbumSongsQuery<SongInfoDTO>>>(provider => () => provider.GetRequiredService<AlbumSongsQuery<SongInfoDTO>>())
                .AddTransient<Func<AlbumsQuery<AlbumDTO>>>(provider => () => provider.GetRequiredService<AlbumsQuery<AlbumDTO>>())
                .AddTransient<Func<AlbumsQuery<AlbumInfoDTO>>>(provider => () => provider.GetRequiredService<AlbumsQuery<AlbumInfoDTO>>())
                .AddTransient<Func<AlbumsQuery<AlbumBandInfoDTO>>>(provider => () => provider.GetRequiredService<AlbumsQuery<AlbumBandInfoDTO>>())
                .AddTransient<Func<BandAlbumsQuery>>(provider => () => provider.GetRequiredService<BandAlbumsQuery>())
                .AddTransient<Func<BandInfoesQuery>>(provider => () => provider.GetRequiredService<BandInfoesQuery>())
                .AddTransient<Func<BandMembersQuery>>(provider => () => provider.GetRequiredService<BandMembersQuery>())
                .AddTransient<Func<BandReviewsQuery>>(provider => () => provider.GetRequiredService<BandReviewsQuery>())
                .AddTransient<Func<BandsQuery<BandDTO>>>(provider => () => provider.GetRequiredService<BandsQuery<BandDTO>>())
                .AddTransient<Func<BandsQuery<BandInfoDTO>>>(provider => () => provider.GetRequiredService<BandsQuery<BandInfoDTO>>())
                .AddTransient<Func<CategoriesQuery>>(provider => () => provider.GetRequiredService<CategoriesQuery>())
                .AddTransient<Func<CategoryAlbumsQuery>>(provider => () => provider.GetRequiredService<CategoryAlbumsQuery>())
                .AddTransient<Func<FeaturedAlbumsQuery>>(provider => () => provider.GetRequiredService<FeaturedAlbumsQuery>())
                .AddTransient<Func<IsInUserAlbumCollectionQuery>>(provider => () => provider.GetRequiredService<IsInUserAlbumCollectionQuery>())
                .AddTransient<Func<RecentAlbumsQuery>>(provider => () => provider.GetRequiredService<RecentAlbumsQuery>())
                .AddTransient<Func<SliderImagesQuery>>(provider => () => provider.GetRequiredService<SliderImagesQuery>())
                .AddTransient<Func<SongsQuery<SongDTO>>>(provider => () => provider.GetRequiredService<SongsQuery<SongDTO>>())
                .AddTransient<Func<SongsQuery<SongInfoDTO>>>(provider => () => provider.GetRequiredService<SongsQuery<SongInfoDTO>>())
                .AddTransient<Func<UserAlbumReviewsQuery>>(provider => () => provider.GetRequiredService<UserAlbumReviewsQuery>())
                .AddTransient<Func<UserAlbumsQuery>>(provider => () => provider.GetRequiredService<UserAlbumsQuery>())
                .AddTransient<Func<UserBandReviewsQuery>>(provider => () => provider.GetRequiredService<UserBandReviewsQuery>())
                .AddTransient<Func<UsersQuery>>(provider => () => provider.GetRequiredService<UsersQuery>())

                .AddSingleton<IUnitOfWorkProvider, AppUnitOfWorkProvider>()
                .AddSingleton<IUnitOfWorkRegistry, AsyncLocalUnitOfWorkRegistry>()
                .AddSingleton<IDateTimeProvider, UtcDateTimeProvider>()
                .AddTransient<Func<IUnitOfWorkProvider>>(provider => () => provider.GetService<IUnitOfWorkProvider>())
                .AddTransient(typeof(IRepository<,>), typeof(EntityFrameworkRepository<,>));
        }
    }
}
