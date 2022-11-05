using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    public class AlbumsViewModel : AdministrationMasterPageViewModel
    {
        private readonly AlbumFacade albumFacade;

        public List<int> SelectedAlbumIds { get; set; } = new ();

        public GridViewDataSet<AlbumInfoDTO> Albums { get; set; }

        public string Filter { get; set; }

        public AlbumsViewModel(AlbumFacade albumFacade)
        {
            this.albumFacade = albumFacade;

            Albums = new ()
            {
                PagingOptions = new PagingOptions()
                {
                    PageSize = 20
                },
                SortingOptions = new SortingOptions()
                {
                    SortExpression = nameof(AlbumInfoDTO.AlbumName)
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
                ActiveAdminPage = "Albums";
            }

            await albumFacade.LoadAlbumsAsync(Albums, Filter);
            await base.PreRender();
        }

        public async Task ApproveSelected()
        {
            await albumFacade.ApproveAlbumsAsync(SelectedAlbumIds, true);
            SelectedAlbumIds.Clear();
        }

        public async Task DisapproveSelected()
        {
            await albumFacade.ApproveAlbumsAsync(SelectedAlbumIds, false);
            SelectedAlbumIds.Clear();
        }
    }
}
