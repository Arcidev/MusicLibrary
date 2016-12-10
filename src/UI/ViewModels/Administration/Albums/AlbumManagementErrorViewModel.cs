
namespace MusicLibrary.ViewModels.Administration
{
	public class AlbumManagementErrorViewModel
    {
        public string NameError { get; set; }

        public string BandError { get; set; }

        public string CategoryError { get; set; }

        public bool ContainsError
        {
            get
            {
                return NameError != null ||
                    BandError != null ||
                    CategoryError != null;
            }
        }
    }
}
