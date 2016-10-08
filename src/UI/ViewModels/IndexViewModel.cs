using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Collections.Generic;

namespace MusicLibrary.ViewModels
{
    public class IndexViewModel : MasterPageViewModel
    {
        [Bind(Direction.None)]
        public SliderImageFacade SliderImageFacade { get; set; }

        public IList<SliderImageDTO> SliderImages { get; set; }

        public override Task PreRender()
        {
            SliderImages = SliderImageFacade.GetImages();
            return base.PreRender();
        }
    }
}
