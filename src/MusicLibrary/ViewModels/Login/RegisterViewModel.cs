using BusinessLayer.DTO;
using BusinessLayer.Facades;
using MusicLibrary.Resources;
using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Login
{
    public class RegisterViewModel : BaseLoginViewModel
    {
        [Required(ErrorMessageResourceName = nameof(Texts.FirstNameRequired), ErrorMessageResourceType = typeof(Texts))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = nameof(Texts.LastNameRequired), ErrorMessageResourceType = typeof(Texts))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = nameof(Texts.EmailRequired), ErrorMessageResourceType = typeof(Texts))]
        [EmailAddress(ErrorMessageResourceName = nameof(Texts.InvalidEmail), ErrorMessageResourceType = typeof(Texts))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = nameof(Texts.PasswordRequired), ErrorMessageResourceType = typeof(Texts))]
        [MinLength(6, ErrorMessageResourceName = nameof(Texts.WeakPassword), ErrorMessageResourceType = typeof(Texts))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessageResourceName = nameof(Texts.WeakPassword), ErrorMessageResourceType = typeof(Texts))]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessageResourceName = nameof(Texts.PasswordMismatch), ErrorMessageResourceType = typeof(Texts))]
        public string PasswordAgain { get; set; }

        public RegisterViewModel(UserFacade userFacade) : base(userFacade) { }

        public async Task Register()
        {
            var success = await ExecuteSafelyAsync(async () =>
            {
                var user = await userFacade.AddUserAsync(new ()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Password = Password,
                    UserRole = UserRole.User
                });

                await SignUserIn(user);
            }, failureCallback: (ex) => ErrorMessage = ex.Message);

            if (success)
                Context.RedirectToRoute("Index");
        }
    }
}
