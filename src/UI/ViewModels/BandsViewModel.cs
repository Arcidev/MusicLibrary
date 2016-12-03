using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Collections.Generic;

namespace MusicLibrary.ViewModels
{
    public class BandsViewModel : ContentMasterPageViewModel
    {
        [Bind(Direction.None)]
        public BandFacade BandFacade { get; set; }

        public IEnumerable<BandDTO> Bands { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActivePage = "Bands";
                Bands = BandFacade.GetBands();
            }

            return base.PreRender();
        }
    }
}
