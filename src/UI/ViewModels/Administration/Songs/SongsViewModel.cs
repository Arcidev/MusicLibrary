using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize(Roles = new[] { nameof(Shared.Enums.UserRole.SuperUser), nameof(Shared.Enums.UserRole.Admin) })]
    public class SongsViewModel : AdministrationMasterPageViewModel
    {
        [Bind(Direction.None)]
        public SongFacade SongFacade { get; set; }

        public IList<int> SelectedSongIds { get; set; } = new List<int>();

        public GridViewDataSet<SongInfoDTO> Songs { get; set; }

        public string Filter { get; set; }

        public SongsViewModel()
        {
            Songs = new GridViewDataSet<SongInfoDTO>()
            {
                PagingOptions = new PagingOptions()
                {
                    PageSize = 20
                },
                SortingOptions = new SortingOptions()
                {
                    SortExpression = nameof(SongInfoDTO.Name)
                }
            };
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "Songs";
            }

            SongFacade.LoadSongInfoes(Songs, Filter);

            await base.PreRender();
        }

        public void ApproveSelected()
        {
            SongFacade.ApproveSongs(SelectedSongIds, true);
            SelectedSongIds.Clear();
        }

        public void DisapproveSelected()
        {
            SongFacade.ApproveSongs(SelectedSongIds, false);
            SelectedSongIds.Clear();
        }
    }
}
