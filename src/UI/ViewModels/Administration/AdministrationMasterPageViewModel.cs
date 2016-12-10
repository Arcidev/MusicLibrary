using DotVVM.Framework.Runtime.Filters;
using System.Security.Claims;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize]
    public class AdministrationMasterPageViewModel : MasterPageViewModel
    {
        public string UserRole { get { return Context.OwinContext.Authentication.User.FindFirst(ClaimTypes.Role).Value; } }

        public string ActiveAdminPage { get; set; }
    }
}
