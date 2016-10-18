using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Login
{
	public class LoginViewModel : BaseLoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public async Task SignIn()
        {
            var success = await ExecuteSafelyAsync(async () =>
            {
                var user = await UserFacade.VerifyAndGetUserAsync(Email, Password);
                SignUserIn(user);
            }, failureCallback: (ex) => ErrorMessage = ex.Message);

            if (success)
                Context.RedirectToRoute("Index");
        }
	}
}
