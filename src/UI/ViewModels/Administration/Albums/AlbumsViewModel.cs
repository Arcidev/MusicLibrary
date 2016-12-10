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
    public class AlbumsViewModel : AdministrationMasterPageViewModel
    {
        [Bind(Direction.None)]
        public AlbumFacade AlbumFacade { get; set; }

        public IList<int> SelectedAlbumIds { get; set; } = new List<int>();

        public GridViewDataSet<AlbumInfoDTO> Albums { get; set; }

        public string Filter { get; set; }

        public AlbumsViewModel()
        {
            Albums = new GridViewDataSet<AlbumInfoDTO>() { PageSize = 20, SortExpression = nameof(AlbumInfoDTO.AlbumName) };
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "Albums";
            }

            AlbumFacade.LoadAlbums(Albums, Filter);

            await base.PreRender();
        }

        public void ApproveSelected()
        {
            AlbumFacade.ApproveAlbums(SelectedAlbumIds, true);
            SelectedAlbumIds.Clear();
        }

        public void DisapproveSelected()
        {
            AlbumFacade.ApproveAlbums(SelectedAlbumIds, false);
            SelectedAlbumIds.Clear();
        }
    }
}
