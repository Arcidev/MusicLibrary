
namespace MusicLibrary.ViewModels.Administration
{
    public class UserProfileErrorViewModel
    {
        public string FirstNameError { get; set; }

        public string LastNameError { get; set; }

        public string PasswordError { get; set; }

        public string PasswordAgainError { get; set; }

        public bool ContainsError
        {
            get
            {
                return FirstNameError != null ||
                    LastNameError != null ||
                    PasswordError != null ||
                    PasswordAgainError != null;
            }
        }
    }
}
