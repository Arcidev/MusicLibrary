using BusinessLayer.DTO;
using BusinessLayer.Facades;
using MusicLibrary.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
    public class AlbumsViewModel : ContentMasterPageViewModel
    {
        private readonly AlbumFacade albumFacade;

        public IEnumerable<IEnumerable<AlbumDTO>> Albums { get; set; }

        public AlbumsViewModel(AlbumFacade albumFacade, CategoryFacade categoryFacade) : base(categoryFacade)
        {
            this.albumFacade = albumFacade;
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                if (Context.Parameters.ContainsKey("Filter"))
                    SearchString = Context.Parameters["Filter"].ToString();

                int? categoryId = null;
                if (Context.Parameters.ContainsKey("CategoryId"))
                    categoryId = int.Parse(Context.Parameters["CategoryId"].ToString());

                ActivePage = "Albums";
                Albums = (await albumFacade.GetAlbumsAsync(categoryId, SearchString)).Chunk(5);
            }

            await base.PreRender();
        }
    }
}
