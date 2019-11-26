using BusinessLayer.Facades;
using MusicLibrary.Resources;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Login
{
    public class LoginViewModel : BaseLoginViewModel
    {
        [Required(ErrorMessageResourceName = nameof(Texts.EmailRequired), ErrorMessageResourceType = typeof(Texts))]
        [EmailAddress(ErrorMessageResourceName = nameof(Texts.InvalidEmail), ErrorMessageResourceType = typeof(Texts))]
        public string Email { get; set; }

        public string Password { get; set; }

        public LoginViewModel(UserFacade userFacade) : base(userFacade) { }

        public async Task SignIn()
        {
            var success = await ExecuteSafelyAsync(async () =>
            {
                var user = await userFacade.VerifyAndGetUserAsync(Email, Password);
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
