using BusinessLayer.DTO;
using BusinessLayer.Facades;
using MusicLibrary.Extensions;
using System;
using System.Collections.Generic;
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

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActivePage = "Index";
                await Task.WhenAll(Task.Run(async () => { SliderImages = await sliderImageFacade.GetImages(); new Random().Shuffle(SliderImages); }),
                    Task.Run(async () => Categories = await categoryFacade.GetCategoriesAsync()),
                    Task.Run(async () => RecentlyAddedAlbums = await albumFacade.GetRecentAlbumsAsync(5)),
                    Task.Run(async () => FeaturedAlbums = await albumFacade.GetFeaturedAlbumsAsync(5)));
            }

            await base.PreRender();
        }
    }
}
