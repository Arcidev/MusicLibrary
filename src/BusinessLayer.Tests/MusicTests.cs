using BusinessLayer.DTO;
using BusinessLayer.Facades;
using Microsoft.Extensions.DependencyInjection;
using Shared.Enums;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BL.Tests
{
    public class MusicTests : TestBase, IAsyncLifetime
    {
        private static UserFacade UserFacade => services.GetRequiredService<UserFacade>();

        private static AlbumFacade AlbumFacade => services.GetRequiredService<AlbumFacade>();

        private static BandFacade BandFacade => services.GetRequiredService<BandFacade>();

        private static SongFacade SongFacade => services.GetRequiredService<SongFacade>();

        private static CategoryFacade CategoryFacade => services.GetRequiredService<CategoryFacade>();

        [Fact]
        public async Task TestFeaturedAlbums()
        {
            var albums = await AlbumFacade.GetFeaturedAlbumsAsync(2);
            Assert.Equal(2, albums.Count);
            Assert.Equal("Test album 2", albums.First().Name);
        }

        [Fact]
        public async Task TestGetBands()
        {
            var bands = await BandFacade.GetBandsAsync();
            Assert.Contains(bands, x => x.Name == "Test Band");
        }

        [Fact]
        public async Task TestGetCategories()
        {
            var categories = await CategoryFacade.GetCategoriesAsync();
            Assert.Contains(categories, x => x.Name == "Test Category");
        }

        [Fact]
        public async Task TestUserCollections()
        {
            var albumFacade = AlbumFacade;
            var album = (await albumFacade.GetFeaturedAlbumsAsync(1)).First();
            var user = await UserFacade.GetUserByEmailAsync("albumtest@mail.sk");

            await albumFacade.AddAlbumToUserCollectionAsync(new () { AlbumId = album.Id, UserId = user.Id });
            var collection = await albumFacade.GetUserAlbumsAsync(user.Id);
            Assert.Single(collection);
            Assert.Equal(album.Id, collection.First().Id);
        }

        [Fact]
        public async Task TestGetAlbums()
        {
            var albums = await AlbumFacade.GetAlbumsAsync();
            Assert.Contains(albums, x => x.Name == "Test album 1");
        }

        [Fact]
        public async Task TestGetBandAlbums()
        {
            var bandFacade = BandFacade;
            var band = (await bandFacade.GetBandsAsync()).Last();
            var albums = await BandFacade.GetBandAlbumsAsync(band.Id);
            Assert.True(albums.All(x => x.BandId == band.Id));
        }

        [Fact]
        public async Task TestSongs()
        {
            var songFacade = SongFacade;
            var song = await songFacade.AddSongAsync(new ()
            {
                Approved = true,
                Name = "test song"
            });

            var song2 = await songFacade.AddSongAsync(new ()
            {
                Approved = true,
                Name = "test song2"
            });

            var albumFacade = AlbumFacade;
            var album = (await albumFacade.GetAlbumsAsync()).First();
            await albumFacade.AddSongToAlbumAsync(album.Id, song.Id);
            await albumFacade.AddSongToAlbumAsync(album.Id, song2.Id);
            album = await albumFacade.GetAlbumAsync(album.Id, includeSongs: true);

            Assert.Equal(2, album.Songs.Count());
            await songFacade.DeleteSongAsync(song.Id);
            await songFacade.DeleteSongAsync(song2.Id);

            album = await albumFacade.GetAlbumAsync(album.Id, includeSongs: true);
            Assert.Empty(album.Songs);
        }

        [Fact]
        public async Task TestCategories()
        {
            var categoryFacade = CategoryFacade;
            var category = await categoryFacade.AddCategoryAsync(new ()
            {
                Name = "Another test category"
            });

            var categories = await categoryFacade.GetCategoriesAsync();
            Assert.Contains(categories, x => x.Name == "Another test category");

            category.Name = "Another test category 2";
            category = await categoryFacade.EditCategoryAsync(category);
            categories = await categoryFacade.GetCategoriesAsync();
            Assert.DoesNotContain(categories, x => x.Name == "Another test category");
            Assert.Contains(categories, x => x.Name == "Another test category 2");

            await categoryFacade.DeleteCategoryAsync(category.Id);
            categories = await categoryFacade.GetCategoriesAsync();
            Assert.DoesNotContain(categories, x => x.Name == "Another test category 2");
        }

        public async Task InitializeAsync()
        {
            var band = await BandFacade.AddBandAsync(new ()
            {
                Name = "Test Band",
                Description = "Test Description",
                Approved = true
            });

            var category = (await CategoryFacade.GetCategoriesAsync()).FirstOrDefault(x => x.Name == "Test Category");
            category ??= await CategoryFacade.AddCategoryAsync(new ()
            {
                Name = "Test Category"
            });

            var albumFacade = AlbumFacade;
            var album1 = await albumFacade.AddAlbumAsync(new ()
            {
                Approved = true,
                BandId = band.Id,
                CategoryId = category.Id,
                Name = "Test album 1"
            });
            var album2 = await albumFacade.AddAlbumAsync(new ()
            {
                Approved = true,
                BandId = band.Id,
                CategoryId = category.Id,
                Name = "Test album 2"
            });

            var user = await UserFacade.GetUserByEmailAsync("albumtest@mail.sk");
            user ??= await UserFacade.AddUserAsync(new ()
            {
                Email = "albumtest@mail.sk",
                FirstName = "test",
                LastName = "test",
                Password = "abcd"
            });

            var positiveReview = new AlbumReviewCreateDTO()
            {
                Text = "Positive review",
                Quality = Quality.Awesome,
                AlbumId = album1.Id,
                CreatedById = user.Id
            };

            await albumFacade.AddReviewAsync(positiveReview);

            positiveReview.AlbumId = album2.Id;
            await albumFacade.AddReviewAsync(positiveReview);
            await albumFacade.AddReviewAsync(new ()
            {
                AlbumId = album1.Id,
                Quality = Quality.Trash,
                Text = "Absolute trash",
                CreatedById = user.Id
            });
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
