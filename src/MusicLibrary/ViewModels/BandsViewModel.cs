using BusinessLayer.DTO;
using BusinessLayer.Facades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
    public class BandsViewModel : ContentMasterPageViewModel
    {
        private readonly BandFacade bandFacade;

        public IEnumerable<BandDTO> Bands { get; set; }

        public BandsViewModel(BandFacade bandFacade, CategoryFacade categoryFacade) : base(categoryFacade)
        {
            this.bandFacade = bandFacade;
        }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActivePage = "Bands";
                Bands = bandFacade.GetBands();
            }

            return base.PreRender();
        }
    }
}
