using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Collections.Generic;

namespace MusicLibrary.ViewModels
{
    public class IndexViewModel : MasterPageViewModel
    {
        [Bind(Direction.None)]
        public SliderImageFacade SliderImageFacade { get; set; }

        [Bind(Direction.None)]
        public CategoryFacade CategoryFacade { get; set; }

        [Bind(Direction.None)]
        public AlbumFacade AlbumFacade { get; set; }

        public IList<SliderImageDTO> SliderImages { get; set; }

        public IList<CategoryDTO> Categories { get; set; }

        public IList<AlbumDTO> RecentlyAddedAlbums { get; set; }

        public IList<AlbumDTO> FeaturedAlbums { get; set; }

        public override Task PreRender()
        {
            SliderImages = SliderImageFacade.GetImages();
            Categories = CategoryFacade.GetCategories();
            RecentlyAddedAlbums = AlbumFacade.GetRecentAlbums(5);
            FeaturedAlbums = AlbumFacade.GetFeaturedAlbums(5);

            return base.PreRender();
        }
    }
}
