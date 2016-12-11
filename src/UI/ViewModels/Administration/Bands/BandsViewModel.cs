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
    public class BandsViewModel : AdministrationMasterPageViewModel
    {
        [Bind(Direction.None)]
        public BandFacade BandFacade { get; set; }

        public IList<int> SelectedBandIds { get; set; } = new List<int>();

        public GridViewDataSet<BandInfoDTO> Bands { get; set; }

        public string Filter { get; set; }

        public BandsViewModel()
        {
            Bands = new GridViewDataSet<BandInfoDTO>() { PageSize = 20, SortExpression = nameof(BandInfoDTO.Name) };
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "Bands";
            }

            BandFacade.LoadBands(Bands, Filter);

            await base.PreRender();
        }

        public void ApproveSelected()
        {
            BandFacade.ApproveBands(SelectedBandIds, true);
            SelectedBandIds.Clear();
        }

        public void DisapproveSelected()
        {
            BandFacade.ApproveBands(SelectedBandIds, false);
            SelectedBandIds.Clear();
        }
    }
}
