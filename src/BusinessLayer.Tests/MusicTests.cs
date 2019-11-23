using BusinessLayer.DTO;
using BusinessLayer.Facades;
using Microsoft.Extensions.DependencyInjection;
using Shared.Enums;
using System.Linq;
using Xunit;

namespace BL.Tests
{
    public class MusicTests : TestBase
    {
        private static UserFacade UserFacade => services.GetRequiredService<UserFacade>();

        private static AlbumFacade AlbumFacade => services.GetRequiredService<AlbumFacade>();

        private static BandFacade BandFacade => services.GetRequiredService<BandFacade>();

        private static SongFacade SongFacade => services.GetRequiredService<SongFacade>();

        private static CategoryFacade CategoryFacade => services.GetRequiredService<CategoryFacade>();

        static MusicTests()
        {
            var band = BandFacade.AddBand(new BandBaseDTO()
            {
                Name = "Test Band",
                Description = "Test Description",
                Approved = true
            });

            var category = CategoryFacade.GetCategories().FirstOrDefault(x => x.Name == "Test Category");
            category ??= CategoryFacade.AddCategory(new CategoryDTO()
            {
                Name = "Test Category"
            });
         
            var albumFacade = AlbumFacade;
            var album1 = albumFacade.AddAlbum(new AlbumCreateDTO()
            {
                Approved = true,
                BandId = band.Id,
                CategoryId = category.Id,
                Name = "Test album 1"
            });
            var album2 = albumFacade.AddAlbum(new AlbumCreateDTO()
            {
                Approved = true,
                BandId = band.Id,
                CategoryId = category.Id,
                Name = "Test album 2"
            });

            var user = UserFacade.GetUserByEmail("albumtest@mail.sk");
            user ??= UserFacade.AddUser(new UserCreateDTO()
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

            albumFacade.AddReview(positiveReview);

            positiveReview.AlbumId = album2.Id;
            albumFacade.AddReview(positiveReview);
            albumFacade.AddReview(new AlbumReviewCreateDTO()
            {
                AlbumId = album1.Id,
                Quality = Quality.Trash,
                Text = "Absolute trash",
                CreatedById = user.Id
            });
        }

        [Fact]
        public void TestFeaturedAlbums()
        {
            var albums = AlbumFacade.GetFeaturedAlbums(2);
            Assert.Equal(2, albums.Count());
            Assert.Equal("Test album 2", albums.First().Name);
        }

        [Fact]
        public void TestGetBands()
        {
            var bands = BandFacade.GetBands();
            Assert.Contains(bands, x => x.Name == "Test Band");
        }

        [Fact]
        public void TestGetCategories()
        {
            var categories = CategoryFacade.GetCategories();
            Assert.Contains(categories, x => x.Name == "Test Category");
        }

        [Fact]
        public void TestUserCollections()
        {
            var albumFacade = AlbumFacade;
            var album = albumFacade.GetFeaturedAlbums(1).First();
            var user = UserFacade.GetUserByEmail("albumtest@mail.sk");

            albumFacade.AddAlbumToUserCollection(new UserAlbumCreateDTO() { AlbumId = album.Id, UserId = user.Id });
            var collection = albumFacade.GetUserAlbums(user.Id);
            Assert.Single(collection);
            Assert.Equal(album.Id, collection.First().Id);
        }

        [Fact]
        public void TestGetAlbums()
        {
            var albums = AlbumFacade.GetAlbums();
            Assert.Contains(albums, x => x.Name == "Test album 1");
        }

        [Fact]
        public void TestGetBandAlbums()
        {
            var bandFacade = BandFacade;
            var band = bandFacade.GetBands().Last();
            var albums = BandFacade.GetBandAlbums(band.Id);
            Assert.True(albums.All(x => x.BandId == band.Id));
        }

        [Fact]
        public void TestSongs()
        {
            var songFacade = SongFacade;
            var song = songFacade.AddSong(new SongCreateDTO()
            {
                Approved = true,
                Name = "test song"
            });

            var song2 = songFacade.AddSong(new SongCreateDTO()
            {
                Approved = true,
                Name = "test song2"
            });

            var albumFacade = AlbumFacade;
            var album = albumFacade.GetAlbums().First();
            albumFacade.AddSongToAlbum(album.Id, song.Id);
            albumFacade.AddSongToAlbum(album.Id, song2.Id);
            album = albumFacade.GetAlbum(album.Id, includeSongs: true);

            Assert.Equal(2, album.Songs.Count());
            songFacade.DeleteSong(song.Id);
            songFacade.DeleteSong(song2.Id);

            album = albumFacade.GetAlbum(album.Id, includeSongs: true);
            Assert.Empty(album.Songs);
        }

        [Fact]
        public void TestCategories()
        {
            var categoryFacade = CategoryFacade;
            var category = categoryFacade.AddCategory(new CategoryDTO()
            {
                Name = "Another test category"
            });

            var categories = categoryFacade.GetCategories();
            Assert.Contains(categories, x => x.Name == "Another test category");

            category.Name = "Another test category 2";
            category = categoryFacade.EditCategory(category);
            categories = categoryFacade.GetCategories();
            Assert.DoesNotContain(categories, x => x.Name == "Another test category");
            Assert.Contains(categories, x => x.Name == "Another test category 2");

            categoryFacade.DeleteCategory(category.Id);
            categories = categoryFacade.GetCategories();
            Assert.DoesNotContain(categories, x => x.Name == "Another test category 2");
        }
    }
}
