using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Collections.Generic;

namespace MusicLibrary.ViewModels
{
    public class BandsViewModel : MasterPageViewModel
    {
        [Bind(Direction.None)]
        public CategoryFacade CategoryFacade { get; set; }

        [Bind(Direction.None)]
        public BandFacade BandFacade { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public IEnumerable<BandDTO> Bands { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActivePage = "Bands";
                Categories = CategoryFacade.GetCategories();
                Bands = BandFacade.GetBands();
            }

            return base.PreRender();
        }
    }
}
