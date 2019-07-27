using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize(Roles = nameof(Shared.Enums.UserRole.SuperUser) + ", " + nameof(Shared.Enums.UserRole.Admin))]
    public class SongsViewModel : AdministrationMasterPageViewModel
    {
        private readonly SongFacade songFacade;

        public IList<int> SelectedSongIds { get; set; } = new List<int>();

        public GridViewDataSet<SongInfoDTO> Songs { get; set; }

        public string Filter { get; set; }

        public SongsViewModel(SongFacade songFacade)
        {
            this.songFacade = songFacade;

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

            songFacade.LoadSongInfoes(Songs, Filter);

            await base.PreRender();
        }

        public void ApproveSelected()
        {
            songFacade.ApproveSongs(SelectedSongIds, true);
            SelectedSongIds.Clear();
        }

        public void DisapproveSelected()
        {
            songFacade.ApproveSongs(SelectedSongIds, false);
            SelectedSongIds.Clear();
        }
    }
}
