using DotVVM.Framework.ViewModel;

namespace MusicLibrary.ViewModels
{
	public class MasterPageViewModel : DotvvmViewModelBase
	{
        public bool IsUserLoggedIn { get { return Context.OwinContext.Authentication.User.Identity.IsAuthenticated; } }

        public void SignOut()
        {
            Context.OwinContext.Authentication.SignOut();
            Context.RedirectToRoute("Index");
        }
    }
}
