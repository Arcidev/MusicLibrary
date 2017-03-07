using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize]
    public class UserCollectionViewModel : AdministrationMasterPageViewModel
    {
        [Bind(Direction.None)]
        public AlbumFacade AlbumFacade { get; set; }

        public GridViewDataSet<AlbumInfoDTO> UserCollection { get; set; }

        public string Filter { get; set; }

        public UserCollectionViewModel()
        {
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

            var userId = int.Parse(UserId);
            AlbumFacade.LoadUserAlbumsCollection(userId, UserCollection, Filter);

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
                    await AlbumFacade.AddAlbumToUserCollection(new UserAlbumCreateDTO()
                    {
                        AlbumId = userAlbum.AlbumId,
                        UserId = int.Parse(UserId)
                    });

                    return;
                }

                await AlbumFacade.RemoveAlbumFromUserCollection(int.Parse(UserId), userAlbum.AlbumId);
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
