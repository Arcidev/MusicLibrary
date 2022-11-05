using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    public class SongsViewModel : AdministrationMasterPageViewModel
    {
        private readonly SongFacade songFacade;

        public List<int> SelectedSongIds { get; set; } = new ();

        public GridViewDataSet<SongInfoDTO> Songs { get; set; }

        public string Filter { get; set; }

        public SongsViewModel(SongFacade songFacade)
        {
            this.songFacade = songFacade;

            Songs = new ()
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

        public override async Task Init()
        {
            await Context.Authorize(new[] { nameof(Shared.Enums.UserRole.SuperUser), nameof(Shared.Enums.UserRole.Admin) });
            await base.Init();
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "Songs";
            }

            await songFacade.LoadSongInfoesAsync(Songs, Filter);
            await base.PreRender();
        }

        public async Task ApproveSelected()
        {
            await songFacade.ApproveSongsAsync(SelectedSongIds, true);
            SelectedSongIds.Clear();
        }

        public async Task DisapproveSelected()
        {
            await songFacade.ApproveSongsAsync(SelectedSongIds, false);
            SelectedSongIds.Clear();
        }
    }
}
