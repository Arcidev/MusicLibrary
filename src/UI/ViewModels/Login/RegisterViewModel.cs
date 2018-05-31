using BL.DTO;
using MusicLibrary.Resources;
using Shared.Enums;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Login
{
    public class RegisterViewModel : BaseLoginViewModel
    {
        public RegisterErrorViewModel RegisterErrorViewModel { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordAgain { get; set; }

        public async Task Register()
        {
            RegisterErrorViewModel = new RegisterErrorViewModel();

            if (string.IsNullOrWhiteSpace(FirstName))
                RegisterErrorViewModel.FirstNameError = Texts.FirstNameRequired;

            if (string.IsNullOrWhiteSpace(LastName))
                RegisterErrorViewModel.LastNameError = Texts.LastNameRequired;

            if (string.IsNullOrWhiteSpace(Email))
                RegisterErrorViewModel.EmailError = Texts.EmailRequired;
            else
            {
                try
                {
                    new MailAddress(Email);
                }
                catch(Exception)
                {
                    RegisterErrorViewModel.EmailError = Texts.InvalidEmail;
                }
            }

            if (!IsStrongPassword())
                RegisterErrorViewModel.PasswordError = Texts.WeakPassword;

            if (!string.IsNullOrEmpty(Password) || !string.IsNullOrEmpty(PasswordAgain))
            {
                if (Password != PasswordAgain)
                    RegisterErrorViewModel.PasswordAgainError = Texts.PasswordMismatch;
            }

            if (RegisterErrorViewModel.ContainsError)
                return;

            var success = await ExecuteSafelyAsync(async () =>
            {
                var user = await UserFacade.AddUserAsync(new UserCreateDTO()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Password = Password,
                    UserRole = UserRole.User
                });

                SignUserIn(user);
            }, failureCallback: (ex) => ErrorMessage = ex.Message);

            if (success)
                Context.RedirectToRoute("Index");
        }

        private bool IsStrongPassword()
        {
            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
                return false;

            return Regex.Match(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$").Success;
        }
    }
}
