using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize]
    public class UserCollectionViewModel : AdministrationMasterPageViewModel
    {
        private readonly AlbumFacade albumFacade;

        public GridViewDataSet<AlbumInfoDTO> UserCollection { get; set; }

        public string Filter { get; set; }

        public UserCollectionViewModel(AlbumFacade albumFacade)
        {
            this.albumFacade = albumFacade;

            UserCollection = new GridViewDataSet<AlbumInfoDTO>()
            {
                PagingOptions = new PagingOptions()
                {
                    PageSize = 20
                },
                SortingOptions = new SortingOptions()
                {
                    SortExpression = nameof(AlbumInfoDTO.AlbumName)
                },
                RowEditOptions = new RowEditOptions()
                {
                    PrimaryKeyPropertyName = nameof(AlbumInfoDTO.AlbumId)
                }
            };
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "UserCollection";
            }

            albumFacade.LoadUserAlbumsCollection(UserId, UserCollection, Filter);

            await base.PreRender();
        }

        public void Edit(int id)
        {
            UserCollection.RowEditOptions.EditRowId = id;
        }

        public void Update(AlbumInfoDTO userAlbum)
        {
            var success = ExecuteSafely(() =>
            {
                if (userAlbum.HasInCollection)
                {
                    albumFacade.AddAlbumToUserCollection(new UserAlbumCreateDTO()
                    {
                        AlbumId = userAlbum.AlbumId,
                        UserId = UserId
                    });

                    return;
                }

                albumFacade.RemoveAlbumFromUserCollection(UserId, userAlbum.AlbumId);
            });

            if (success)
                CancelEdit();
        }

        public void CancelEdit()
        {
            UserCollection.RowEditOptions.EditRowId = null;
        }
    }
}
