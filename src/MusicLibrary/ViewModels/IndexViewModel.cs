using BusinessLayer.DTO;
using BusinessLayer.Extensions;
using BusinessLayer.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
    public class IndexViewModel : MasterPageViewModel
    {
        private readonly SliderImageFacade sliderImageFacade;
        private readonly CategoryFacade categoryFacade;
        private readonly AlbumFacade albumFacade;

        public IList<SliderImageDTO> SliderImages { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public IEnumerable<AlbumDTO> RecentlyAddedAlbums { get; set; }

        public IEnumerable<AlbumDTO> FeaturedAlbums { get; set; }

        public IndexViewModel(SliderImageFacade sliderImageFacade, CategoryFacade categoryFacade, AlbumFacade albumFacade)
        {
            this.sliderImageFacade = sliderImageFacade;
            this.categoryFacade = categoryFacade;
            this.albumFacade = albumFacade;
        }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActivePage = "Index";
                SliderImages = sliderImageFacade.GetImages().ToList();
                new Random().Shuffle(SliderImages);
                Categories = categoryFacade.GetCategories();
                RecentlyAddedAlbums = albumFacade.GetRecentAlbums(5);
                FeaturedAlbums = albumFacade.GetFeaturedAlbums(5);
            }

            return base.PreRender();
        }
    }
}
