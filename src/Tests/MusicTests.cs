using BL.DTO;
using BL.Facades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Tests
{
    [TestClass]
    public class MusicTests : TestBase
    {
        public UserFacade UserFacade { get { return Container.Resolve<UserFacade>(); } }

        public AlbumFacade AlbumFacade { get { return Container.Resolve<AlbumFacade>(); } }

        public BandFacade BandFacade { get { return Container.Resolve<BandFacade>(); } }

        public SongFacade SongFacade { get { return Container.Resolve<SongFacade>(); } }

        public CategoryFacade CategoryFacade { get { return Container.Resolve<CategoryFacade>(); } }

        [TestInitialize]
        public void Init()
        {
            var band = BandFacade.AddBand(new BandCreateDTO()
            {
                Name = "Test Band",
                Description = "Test Description",
                Approved = true
            });

            var category = CategoryFacade.GetCategories().FirstOrDefault();
            if (category == null)
            {
                category = CategoryFacade.AddCategory(new CategoryDTO()
                {
                    Name = "Test Category"
                });
            }

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

            var user = UserFacade.GetUserByEmailAsync("albumtest@mail.sk").Result;
            if (user == null)
            {
                user = UserFacade.AddUserAsync(new UserCreateDTO()
                {
                    Email = "albumtest@mail.sk",
                    FirstName = "test",
                    LastName = "test",
                    Password = "abcd"
                }).Result;
            }

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

        [TestMethod]
        public void TestFeaturedAlbums()
        {
            var albums = AlbumFacade.GetFeaturedAlbums(2);
            Assert.AreEqual(2, albums.Count());
            Assert.AreEqual("Test album 2", albums.First().Name);
        }

        [TestMethod]
        public void TestGetBands()
        {
            var bands = BandFacade.GetBands();
            Assert.IsTrue(bands.Any(x => x.Name == "Test Band"));
        }

        [TestMethod]
        public void TestGetCategories()
        {
            var categories = CategoryFacade.GetCategories();
            Assert.IsTrue(categories.Any(x => x.Name == "Test Category"));
        }

        [TestMethod]
        public async Task TestGetReviews()
        {
            var albumFacade = AlbumFacade;
            var album = albumFacade.GetFeaturedAlbums(1).First();
            var user = await UserFacade.GetUserByEmailAsync("albumtest@mail.sk");
            Assert.IsNotNull(user);

            var reviews = albumFacade.GetReviews(album.Id);

            Assert.IsTrue(reviews.Any());
            Assert.IsTrue(reviews.All(x => x.AlbumId == album.Id));
        }

        [TestMethod]
        public async Task TestUserCollections()
        {
            var albumFacade = AlbumFacade;
            var album = albumFacade.GetFeaturedAlbums(1).First();
            var user = await UserFacade.GetUserByEmailAsync("albumtest@mail.sk");

            albumFacade.AddAlbumToUserCollection(new UserAlbumCreateDTO() { AlbumId = album.Id, UserId = user.Id });
            var collection = albumFacade.GetUserAlbums(user.Id);
            Assert.AreEqual(1, collection.Count());
            Assert.AreEqual(album.Id, collection.First().Id);
        }

        [TestMethod]
        public void TestGetAlbums()
        {
            var albums = AlbumFacade.GetAlbums();
            Assert.IsTrue(albums.Any(x => x.Name == "Test album 1"));
        }

        [TestMethod]
        public void TestGetBandAlbums()
        {
            var bandFacade = BandFacade;
            var band = bandFacade.GetBands().Last();
            var albums = BandFacade.GetBandAlbums(band.Id);
            Assert.IsTrue(albums.All(x => x.BandId == band.Id));
        }

        [TestMethod]
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

            Assert.AreEqual(2, album.Songs.Count());
            songFacade.DeleteSong(song.Id);
            songFacade.DeleteSong(song2.Id);

            album = albumFacade.GetAlbum(album.Id, includeSongs: true);
            Assert.AreEqual(0, album.Songs.Count());
        }

        [TestMethod]
        public void TestCategories()
        {
            var categoryFacade = CategoryFacade;
            var category = categoryFacade.AddCategory(new CategoryDTO()
            {
                Name = "Another test category"
            });

            var categories = categoryFacade.GetCategories();
            Assert.IsTrue(categories.Any(x => x.Name == "Another test category"));

            category.Name = "Another test category 2";
            category = categoryFacade.EditCategory(category);
            categories = categoryFacade.GetCategories();
            Assert.IsFalse(categories.Any(x => x.Name == "Another test category"));
            Assert.IsTrue(categories.Any(x => x.Name == "Another test category 2"));

            categoryFacade.DeleteCategory(category.Id);
            categories = categoryFacade.GetCategories();
            Assert.IsFalse(categories.Any(x => x.Name == "Another test category 2"));
        }
    }
}
