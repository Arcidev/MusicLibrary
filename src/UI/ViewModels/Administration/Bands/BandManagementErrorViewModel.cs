
namespace MusicLibrary.ViewModels.Administration
{
    public class BandManagementErrorViewModel
    {
        public string NameError { get; set; }

        public string DescriptionError { get; set; }

        public bool ContainsError => NameError != null || DescriptionError != null;
    }
}
