using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using MusicLibrary.Resources;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize]
    public class UserProfileViewModel : AdministrationMasterPageViewModel
    {
        [Bind(Direction.None)]
        public UserFacade UserFacade { get; set; }

        public UserProfileErrorViewModel UserProfileErrorViewModel { get; set; }

        public UserDTO User { get; set; }

        public string Password { get; set; }

        public string PasswordAgain { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "UserProfile";
                User = UserFacade.GetUser(int.Parse(UserId));
            }

            return base.PreRender();
        }

        public void SaveChanges()
        {
            UserProfileErrorViewModel = new UserProfileErrorViewModel();

            if (string.IsNullOrWhiteSpace(User.FirstName))
                UserProfileErrorViewModel.FirstNameError = Texts.FirstNameRequired;

            if (string.IsNullOrWhiteSpace(User.LastName))
                UserProfileErrorViewModel.LastNameError = Texts.LastNameRequired;

            if (!string.IsNullOrWhiteSpace(Password) && !IsStrongPassword())
                UserProfileErrorViewModel.PasswordError = Texts.WeakPassword;

            if (!string.IsNullOrEmpty(Password) || !string.IsNullOrEmpty(PasswordAgain))
            {
                if (Password != PasswordAgain)
                    UserProfileErrorViewModel.PasswordAgainError = Texts.PasswordMismatch;
            }

            if (UserProfileErrorViewModel.ContainsError)
                return;

            ExecuteSafely(() =>
            {
                User.Password = Password;
                UserFacade.EditUser(User);
            });
        }

        private bool IsStrongPassword()
        {
            if (Password.Length < 6)
                return false;

            return Regex.Match(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$").Success;
        }
    }
}
