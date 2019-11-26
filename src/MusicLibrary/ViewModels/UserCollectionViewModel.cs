using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Runtime.Filters;
using MusicLibrary.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
    [Authorize]
    public class UserCollectionViewModel : ContentMasterPageViewModel
    {
        private readonly AlbumFacade albumFacade;

        public IEnumerable<IEnumerable<AlbumDTO>> Albums { get; set; }

        public UserCollectionViewModel(AlbumFacade albumFacade, CategoryFacade categoryFacade) : base(categoryFacade)
        {
            this.albumFacade = albumFacade;
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActivePage = "UserCollection";
                Albums = (await albumFacade.GetUserAlbumsAsync(UserId)).Chunk(5);
            }

            await base.PreRender();
        }
    }
}
