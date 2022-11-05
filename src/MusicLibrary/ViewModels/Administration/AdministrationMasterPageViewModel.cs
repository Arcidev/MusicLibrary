using DotVVM.Framework.Hosting;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    public class AdministrationMasterPageViewModel : MasterPageViewModel
    {
        public override async Task Init()
        {
            await Context.Authorize();
            await base.Init();
        }

        public string UserRole => Context.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        public string ActiveAdminPage { get; set; }
    }
}
