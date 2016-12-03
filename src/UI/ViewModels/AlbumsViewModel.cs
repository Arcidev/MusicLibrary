using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Collections.Generic;
using BL.Extensions;

namespace MusicLibrary.ViewModels
{
    public class AlbumsViewModel : ContentMasterPageViewModel
    {
        [Bind(Direction.None)]
        public AlbumFacade AlbumFacade { get; set; }

        public IEnumerable<IEnumerable<AlbumDTO>> Albums { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                int? categoryId = null;
                if (Context.Parameters.ContainsKey("Filter"))
                    SearchString = Context.Parameters["Filter"].ToString();

                if (Context.Parameters.ContainsKey("CategoryId"))
                    categoryId = int.Parse(Context.Parameters["CategoryId"].ToString());

                ActivePage = "Albums";
                Albums = AlbumFacade.GetAlbums(categoryId, SearchString).Chunk(5);
            }

            return base.PreRender();
        }
    }
}
