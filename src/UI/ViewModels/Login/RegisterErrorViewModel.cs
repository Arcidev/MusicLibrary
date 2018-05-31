
namespace MusicLibrary.ViewModels.Login
{
    public class RegisterErrorViewModel
    {
        public string FirstNameError { get; set; }

        public string LastNameError { get; set; }

        public string EmailError { get; set; }

        public string PasswordError { get; set; }

        public string PasswordAgainError { get; set; }

        public bool ContainsError
        {
            get
            {
                return FirstNameError != null ||
                    LastNameError != null ||
                    EmailError != null ||
                    PasswordError != null ||
                    PasswordAgainError != null;
            }
        }
    }
}
