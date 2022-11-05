using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    public class BandsViewModel : AdministrationMasterPageViewModel
    {
        private readonly BandFacade bandFacade;

        public List<int> SelectedBandIds { get; set; } = new ();

        public GridViewDataSet<BandInfoDTO> Bands { get; set; }

        public string Filter { get; set; }

        public BandsViewModel(BandFacade bandFacade)
        {
            this.bandFacade = bandFacade;

            Bands = new ()
            {
                PagingOptions = new PagingOptions()
                {
                    PageSize = 20
                },
                SortingOptions = new SortingOptions()
                {
                    SortExpression = nameof(BandInfoDTO.Name)
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
                ActiveAdminPage = "Bands";
            }

            await bandFacade.LoadBandsAsync(Bands, Filter);
            await base.PreRender();
        }

        public async Task ApproveSelected()
        {
            await bandFacade.ApproveBandsAsync(SelectedBandIds, true);
            SelectedBandIds.Clear();
        }

        public async Task DisapproveSelected()
        {
            await bandFacade.ApproveBandsAsync(SelectedBandIds, false);
            SelectedBandIds.Clear();
        }
    }
}
