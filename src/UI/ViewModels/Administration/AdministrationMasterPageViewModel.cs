using DotVVM.Framework.Runtime.Filters;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize]
    public class AdministrationMasterPageViewModel : MasterPageViewModel
    {
        public string ActiveAdminPage { get; set; }
    }
}
