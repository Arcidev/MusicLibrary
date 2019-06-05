using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Collections.Generic;
using BL.Extensions;
using DotVVM.Framework.Runtime.Filters;

namespace MusicLibrary.ViewModels
{
    [Authorize]
    public class UserCollectionViewModel : ContentMasterPageViewModel
    {
        [Bind(Direction.None)]
        public AlbumFacade AlbumFacade { get; set; }

        public IEnumerable<IEnumerable<AlbumDTO>> Albums { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActivePage = "UserCollection";
                Albums = AlbumFacade.GetUserAlbums(UserId).Chunk(5);
            }

            return base.PreRender();
        }
    }
}
