using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Collections.Generic;
using System;
using BL.Extensions;
using System.Linq;

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

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public IEnumerable<AlbumDTO> RecentlyAddedAlbums { get; set; }

        public IEnumerable<AlbumDTO> FeaturedAlbums { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActivePage = "Index";
                SliderImages = SliderImageFacade.GetImages().ToList();
                new Random().Shuffle(SliderImages);
                Categories = CategoryFacade.GetCategories();
                RecentlyAddedAlbums = AlbumFacade.GetRecentAlbums(5);
                FeaturedAlbums = AlbumFacade.GetFeaturedAlbums(5);
            }

            return base.PreRender();
        }
    }
}
