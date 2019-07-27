using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize(Roles = nameof(Shared.Enums.UserRole.SuperUser) + ", " + nameof(Shared.Enums.UserRole.Admin))]
    public class AlbumsViewModel : AdministrationMasterPageViewModel
    {
        private readonly AlbumFacade albumFacade;

        public IList<int> SelectedAlbumIds { get; set; } = new List<int>();

        public GridViewDataSet<AlbumInfoDTO> Albums { get; set; }

        public string Filter { get; set; }

        public AlbumsViewModel(AlbumFacade albumFacade)
        {
            this.albumFacade = albumFacade;

            Albums = new GridViewDataSet<AlbumInfoDTO>()
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

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "Albums";
            }

            albumFacade.LoadAlbums(Albums, Filter);

            await base.PreRender();
        }

        public void ApproveSelected()
        {
            albumFacade.ApproveAlbums(SelectedAlbumIds, true);
            SelectedAlbumIds.Clear();
        }

        public void DisapproveSelected()
        {
            albumFacade.ApproveAlbums(SelectedAlbumIds, false);
            SelectedAlbumIds.Clear();
        }
    }
}
