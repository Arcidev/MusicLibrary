using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Collections.Generic;
using BL.Extensions;

namespace MusicLibrary.ViewModels
{
    public class AlbumsViewModel : MasterPageViewModel
    {
        [Bind(Direction.None)]
        public CategoryFacade CategoryFacade { get; set; }

        [Bind(Direction.None)]
        public AlbumFacade AlbumFacade { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public IEnumerable<IEnumerable<AlbumDTO>> Albums { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActivePage = "Albums";
                Categories = CategoryFacade.GetCategories();
                Albums = AlbumFacade.GetAlbums().Chunk(5);
            }

            return base.PreRender();
        }
    }
}
