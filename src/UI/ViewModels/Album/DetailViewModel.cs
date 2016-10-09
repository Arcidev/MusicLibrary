using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Collections.Generic;

namespace MusicLibrary.ViewModels.Album
{
    public class DetailViewModel : MasterPageViewModel
    {
        [Bind(Direction.None)]
        public CategoryFacade CategoryFacade { get; set; }

        [Bind(Direction.None)]
        public AlbumFacade AlbumFacade { get; set; }

        [Bind(Direction.None)]
        public BandFacade BandFacade { get; set; }

        public IList<CategoryDTO> Categories { get; set; }

        public AlbumDTO Album { get; set; }

        public IList<AlbumDTO> OtherBandAlbums { get; set; }

        public string YoutubeUrlParam { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                int albumId = int.Parse(Context.Parameters["AlbumId"].ToString());
                Categories = CategoryFacade.GetCategories();
                Album = AlbumFacade.GetAlbum(albumId);
                OtherBandAlbums = BandFacade.GetBandAlbums(Album.BandId, Album.Id, 6, true);
            }

            return base.PreRender();
        }
    }
}
