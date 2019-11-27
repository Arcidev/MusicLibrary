using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.Storage;
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
        private readonly UserFacade userFacade;

        private IUploadedFileStorage FileStorage => (IUploadedFileStorage)Context.Configuration.ServiceProvider.GetService(typeof(IUploadedFileStorage));

        public UserProfileErrorViewModel UserProfileErrorViewModel { get; set; }

        public UserDTO User { get; set; }

        public string Password { get; set; }

        public string PasswordAgain { get; set; }

        public UploadedFilesCollection Files { get; set; } = new UploadedFilesCollection();

        public string ImageFileName { get; set; }

        public UserProfileViewModel(UserFacade userFacade)
        {
            this.userFacade = userFacade;
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "UserProfile";
                await LoadUser();
            }

            await base.PreRender();
        }

        public async Task SaveChanges()
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

            await ExecuteSafelyAsync(async () =>
            {
                User.Password = Password;
                await userFacade.EditUserAsync(User, Files.Files.LastOrDefault(), FileStorage);
                await LoadUser();
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
            ImageFileName = User.ImageStorageFile != null ? $"/SavedFiles/{User.ImageStorageFile.FileName}" : $"/api/identicon/{User.FullName}";
            Files.Clear();
        }

        private bool IsStrongPassword()
        {
            if (Password.Length < 6)
                return false;

            return Regex.Match(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$").Success;
        }

        private async Task LoadUser()
        {
            User = await userFacade.GetUserAsync(UserId);
            ImageFileName = User.ImageStorageFile != null ? $"/SavedFiles/{User.ImageStorageFile.FileName}" : $"/api/identicon/{User.FullName}";
        }
    }
}