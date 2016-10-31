using BL.DTO;
using BL.Facades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Enums;
using System.Linq;

namespace BL.Tests
{
    [TestClass]
    public class MusicTests : TestBase
    {
        public UserFacade UserFacade { get { return Container.Resolve<UserFacade>(); } }

        public AlbumFacade AlbumFacade { get { return Container.Resolve<AlbumFacade>(); } }

        public BandFacade BandFacade { get { return Container.Resolve<BandFacade>(); } }

        public CategoryFacade CategoryFacade { get { return Container.Resolve<CategoryFacade>(); } }

        [TestInitialize]
        public void Init()
        {
            var albumFacade = AlbumFacade;

            var band = BandFacade.AddBand(new BandCreateDTO()
            {
                Name = "Test Band"
            });

            var category = CategoryFacade.AddCategory(new CategoryDTO()
            {
                Name = "Test Category"
            });

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

            var user = UserFacade.AddUserAsync(new UserDTO()
            {
                Email = "albumtest@mail.sk",
                FirstName = "test",
                LastName = "test",
                Password = "abcd"
            }).Result;

            var positiveReview = new AlbumReviewDTO()
            {
                Text = "Positive review",
                Quality = Quality.Awesome,
                AlbumId = album1.Id,
                CreatedById = user.Id
            };

            albumFacade.AddReview(positiveReview);

            positiveReview.AlbumId = album2.Id;
            albumFacade.AddReview(positiveReview);
            albumFacade.AddReview(new AlbumReviewDTO()
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
            Assert.AreEqual(2, albums.Count);
            Assert.AreEqual("Test album 2", albums.First().Name);
            Assert.AreEqual("Test album 1", albums.Last().Name);
        }
    }
}
