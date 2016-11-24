using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Collections.Generic;
using System.Linq;

namespace MusicLibrary.ViewModels.Band
{
    public class DetailViewModel : MasterPageViewModel
    {
        [Bind(Direction.None)]
        public CategoryFacade CategoryFacade { get; set; }

        [Bind(Direction.None)]
        public BandFacade BandFacade { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public BandDTO Band { get; set; }

        public bool HasAlbums { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                int bandId = int.Parse(Context.Parameters["BandId"].ToString());
                Categories = CategoryFacade.GetCategories();
                Band = BandFacade.GetBand(bandId);
                HasAlbums = Band.Albums.Any();
            }

            return base.PreRender();
        }
    }
}
