using BusinessLayer.Facades;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Login
{
    public class LoginViewModel : BaseLoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public LoginViewModel(UserFacade userFacade) : base(userFacade) { }

        public async Task SignIn()
        {
            var success = await ExecuteSafelyAsync(async () =>
            {
                var user = userFacade.VerifyAndGetUser(Email, Password);
                await SignUserIn(user);
            }, failureCallback: (ex) => ErrorMessage = ex.Message);

            if (success)
            {
                if (Context.Query.TryGetValue("ReturnUrl", out var returnUrl))
                {
                    Context.RedirectToUrl(returnUrl);
                    return;
                }
                Context.RedirectToRoute("Index");
            }
        }
    }
}
