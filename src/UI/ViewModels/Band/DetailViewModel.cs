using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Linq;

namespace MusicLibrary.ViewModels.Band
{
    public class DetailViewModel : ContentMasterPageViewModel
    {
        [Bind(Direction.None)]
        public BandFacade BandFacade { get; set; }

        public BandDTO Band { get; set; }

        public bool HasAlbums { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                int bandId = int.Parse(Context.Parameters["BandId"].ToString());
                Band = BandFacade.GetBand(bandId);
                HasAlbums = Band.Albums.Any();
            }

            return base.PreRender();
        }
    }
}
