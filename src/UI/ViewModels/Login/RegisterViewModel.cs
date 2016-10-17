using BL.DTO;
using BL.Facades;
using DAL.Enums;
using DotVVM.Framework.ViewModel;
using MusicLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace MusicLibrary.ViewModels.Login
{
	public class RegisterViewModel : BaseLoginViewModel
    {
        [Bind(Direction.None)]
        public UserFacade UserFacade { get; set; }

        [Bind(Direction.ServerToClient)]
        public RegisterErrorViewModel RegisterErrorViewModel { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordAgain { get; set; }

        public void Register()
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

            var user = UserFacade.AddUser(new UserDTO()
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password,
                UserRole = UserRole.User
            });

            SignUserIn(user);
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
