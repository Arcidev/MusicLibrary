using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Collections.Generic;
using BL.Extensions;
using System.Web;

namespace MusicLibrary.ViewModels
{
    public class UserCollectionViewModel : ContentMasterPageViewModel
    {
        [Bind(Direction.None)]
        public AlbumFacade AlbumFacade { get; set; }

        public IEnumerable<IEnumerable<AlbumDTO>> Albums { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                int userId;
                if (!int.TryParse(UserId, out userId))
                    throw new HttpException(401, "Access Denied");

                ActivePage = "UserCollection";
                Albums = AlbumFacade.GetAlbums().Chunk(5);
            }

            return base.PreRender();
        }
    }
}
