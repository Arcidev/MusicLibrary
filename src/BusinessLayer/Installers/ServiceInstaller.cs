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
                services.AddTransient(type);

            return services.AddSingleton<Func<AlbumRepository>>(provider => () => provider.GetRequiredService<AlbumRepository>())
                .AddSingleton<Func<AlbumReviewRepository>>(provider => () => provider.GetRequiredService<AlbumReviewRepository>())
                .AddSingleton<Func<AlbumSongRepository>>(provider => () => provider.GetRequiredService<AlbumSongRepository>())
                .AddSingleton<Func<BandRepository>>(provider => () => provider.GetRequiredService<BandRepository>())
                .AddSingleton<Func<BandReviewRepository>>(provider => () => provider.GetRequiredService<BandReviewRepository>())
                .AddSingleton<Func<CategoryRepository>>(provider => () => provider.GetRequiredService<CategoryRepository>())
                .AddSingleton<Func<SliderImageRepository>>(provider => () => provider.GetRequiredService<SliderImageRepository>())
                .AddSingleton<Func<SongRepository>>(provider => () => provider.GetRequiredService<SongRepository>())
                .AddSingleton<Func<StorageFileRepository>>(provider => () => provider.GetRequiredService<StorageFileRepository>())
                .AddSingleton<Func<UserAlbumRepository>>(provider => () => provider.GetRequiredService<UserAlbumRepository>())
                .AddSingleton<Func<UserRepository>>(provider => () => provider.GetRequiredService<UserRepository>())

                .AddSingleton<Func<AlbumReviewsQuery>>(provider => () => provider.GetRequiredService<AlbumReviewsQuery>())
                .AddSingleton<Func<AlbumSongsQuery<SongDTO>>>(provider => () => provider.GetRequiredService<AlbumSongsQuery<SongDTO>>())
                .AddSingleton<Func<AlbumSongsQuery<SongInfoDTO>>>(provider => () => provider.GetRequiredService<AlbumSongsQuery<SongInfoDTO>>())
                .AddSingleton<Func<AlbumsQuery<AlbumDTO>>>(provider => () => provider.GetRequiredService<AlbumsQuery<AlbumDTO>>())
                .AddSingleton<Func<AlbumsQuery<AlbumInfoDTO>>>(provider => () => provider.GetRequiredService<AlbumsQuery<AlbumInfoDTO>>())
                .AddSingleton<Func<AlbumsQuery<AlbumBandInfoDTO>>>(provider => () => provider.GetRequiredService<AlbumsQuery<AlbumBandInfoDTO>>())
                .AddSingleton<Func<BandAlbumsQuery>>(provider => () => provider.GetRequiredService<BandAlbumsQuery>())
                .AddSingleton<Func<BandInfoesQuery>>(provider => () => provider.GetRequiredService<BandInfoesQuery>())
                .AddSingleton<Func<BandMembersQuery>>(provider => () => provider.GetRequiredService<BandMembersQuery>())
                .AddSingleton<Func<BandReviewsQuery>>(provider => () => provider.GetRequiredService<BandReviewsQuery>())
                .AddSingleton<Func<BandsQuery<BandDTO>>>(provider => () => provider.GetRequiredService<BandsQuery<BandDTO>>())
                .AddSingleton<Func<BandsQuery<BandInfoDTO>>>(provider => () => provider.GetRequiredService<BandsQuery<BandInfoDTO>>())
                .AddSingleton<Func<CategoriesQuery>>(provider => () => provider.GetRequiredService<CategoriesQuery>())
                .AddSingleton<Func<CategoryAlbumsQuery>>(provider => () => provider.GetRequiredService<CategoryAlbumsQuery>())
                .AddSingleton<Func<FeaturedAlbumsQuery>>(provider => () => provider.GetRequiredService<FeaturedAlbumsQuery>())
                .AddSingleton<Func<IsInUserAlbumCollectionQuery>>(provider => () => provider.GetRequiredService<IsInUserAlbumCollectionQuery>())
                .AddSingleton<Func<RecentAlbumsQuery>>(provider => () => provider.GetRequiredService<RecentAlbumsQuery>())
                .AddSingleton<Func<SliderImagesQuery>>(provider => () => provider.GetRequiredService<SliderImagesQuery>())
                .AddSingleton<Func<SongsQuery<SongDTO>>>(provider => () => provider.GetRequiredService<SongsQuery<SongDTO>>())
                .AddSingleton<Func<SongsQuery<SongInfoDTO>>>(provider => () => provider.GetRequiredService<SongsQuery<SongInfoDTO>>())
                .AddSingleton<Func<UserAlbumReviewsQuery>>(provider => () => provider.GetRequiredService<UserAlbumReviewsQuery>())
                .AddSingleton<Func<UserAlbumsQuery>>(provider => () => provider.GetRequiredService<UserAlbumsQuery>())
                .AddSingleton<Func<UserBandReviewsQuery>>(provider => () => provider.GetRequiredService<UserBandReviewsQuery>())
                .AddSingleton<Func<UsersQuery>>(provider => () => provider.GetRequiredService<UsersQuery>())

                .AddSingleton<IUnitOfWorkProvider, AppUnitOfWorkProvider>()
                .AddSingleton<IUnitOfWorkRegistry, AsyncLocalUnitOfWorkRegistry>()
                .AddSingleton<IDateTimeProvider, UtcDateTimeProvider>()
                .AddSingleton<Func<IUnitOfWorkProvider>>(provider => () => provider.GetService<IUnitOfWorkProvider>())
                .AddTransient(typeof(IRepository<,>), typeof(EntityFrameworkRepository<,>));
        }
    }
}
