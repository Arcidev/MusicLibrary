
namespace MusicLibrary.ViewModels.Administration
{
    public class BandManagementErrorViewModel
    {
        public string NameError { get; set; }

        public string DescriptionError { get; set; }

        public bool ContainsError
        {
            get
            {
                return NameError != null || DescriptionError != null;
            }
        }
    }
}
