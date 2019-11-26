using BusinessLayer.DTO;
using BusinessLayer.Facades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
    public abstract class ContentMasterPageViewModel : MasterPageViewModel
    {
        protected readonly CategoryFacade categoryFacade;

        public IEnumerable<CategoryDTO> Categories { get; set; }

        protected ContentMasterPageViewModel(CategoryFacade categoryFacade)
        {
            this.categoryFacade = categoryFacade;
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Categories = await categoryFacade.GetCategoriesAsync();
            }

            await base.PreRender();
        }
    }
}
