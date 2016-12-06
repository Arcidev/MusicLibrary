using BL.DTO;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
	public abstract class ContentMasterPageViewModel : MasterPageViewModel
    {
        [Bind(Direction.None)]
        public CategoryFacade CategoryFacade { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Categories = CategoryFacade.GetCategories();
            }

            return base.PreRender();
        }
    }
}
