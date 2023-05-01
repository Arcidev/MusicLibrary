using BusinessLayer.DTO;
using BusinessLayer.Facades;
using System.Collections.Generic;
using System.Linq;
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
                if (Context.Parameters.TryGetValue("Filter", out var value))
                    SearchString = value.ToString();

                int? categoryId = null;
                if (Context.Parameters.TryGetValue("CategoryId", out value))
                    categoryId = int.Parse(value.ToString());

                ActivePage = "Albums";
                Albums = (await albumFacade.GetAlbumsAsync(categoryId, SearchString)).Chunk(5);
            }

            await base.PreRender();
        }
    }
}
