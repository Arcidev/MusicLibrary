using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;
using System.Security.Claims;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize]
    public class AdministrationMasterPageViewModel : MasterPageViewModel
    {
        public string UserRole => Context.GetAuthentication().User.FindFirst(ClaimTypes.Role).Value;

        public string ActiveAdminPage { get; set; }
    }
}
