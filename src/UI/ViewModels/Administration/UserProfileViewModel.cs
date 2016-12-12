using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.Storage;
using DotVVM.Framework.ViewModel;
using MusicLibrary.Resources;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize]
    public class UserProfileViewModel : AdministrationMasterPageViewModel
    {
        private IUploadedFileStorage FileStorage { get { return Context.Configuration.ServiceLocator.GetService<IUploadedFileStorage>(); } }

        [Bind(Direction.None)]
        public UserFacade UserFacade { get; set; }

        public UserProfileErrorViewModel UserProfileErrorViewModel { get; set; }

        public UserDTO User { get; set; }

        public string Password { get; set; }

        public string PasswordAgain { get; set; }

        public UploadedFilesCollection Files { get; set; } = new UploadedFilesCollection();

        public string ImageFileName { get; set; }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "UserProfile";
                LoadUser();
            }

            await base.PreRender();
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
                UserFacade.EditUser(User, Files.Files.LastOrDefault(), FileStorage);
                LoadUser();
                Files.Clear();
            });
        }

        public void UploadedImage()
        {
            var file = Files.Files.LastOrDefault();
            if (file == null)
                return;

            ImageFileName = $"/files/{file.FileId}/{Path.GetExtension(file.FileName).Substring(1)}";
        }

        public void ResetImage()
        {
            ImageFileName = User.ImageStorageFile != null ? $"/SavedFiles/{User.ImageStorageFile.FileName}" : "";
            Files.Clear();
        }

        private bool IsStrongPassword()
        {
            if (Password.Length < 6)
                return false;

            return Regex.Match(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$").Success;
        }

        private void LoadUser()
        {
            User = UserFacade.GetUser(int.Parse(UserId));
            ImageFileName = User.ImageStorageFile != null ? $"/SavedFiles/{User.ImageStorageFile.FileName}" : "";
        }
    }
}
