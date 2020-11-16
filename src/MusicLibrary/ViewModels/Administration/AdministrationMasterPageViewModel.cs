using DotVVM.Framework.Runtime.Filters;
using System.Security.Claims;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize]
    public class AdministrationMasterPageViewModel : MasterPageViewModel
    {
        public string UserRole => Context.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        public string ActiveAdminPage { get; set; }
    }
}
