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

            await albumFacade.LoadUserAlbumsCollectionAsync(UserId, UserCollection, Filter);
            await base.PreRender();
        }

        public void Edit(int id)
        {
            UserCollection.RowEditOptions.EditRowId = id;
        }

        public async Task Update(AlbumInfoDTO userAlbum)
        {
            var success = await ExecuteSafelyAsync(async () =>
            {
                if (userAlbum.HasInCollection)
                {
                    await albumFacade.AddAlbumToUserCollectionAsync(new UserAlbumCreateDTO()
                    {
                        AlbumId = userAlbum.AlbumId,
                        UserId = UserId
                    });

                    return;
                }

                await albumFacade.RemoveAlbumFromUserCollectionAsync(UserId, userAlbum.AlbumId);
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
