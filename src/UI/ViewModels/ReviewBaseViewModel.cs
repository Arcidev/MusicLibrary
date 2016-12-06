
namespace MusicLibrary.ViewModels
{
    public class ReviewBaseViewModel : ContentMasterPageViewModel
    {
        public int? ReviewQuality { get; set; }

        public string ReviewText { get; set; }

        public bool AddReviewVisible { get; set; }

        public string AddReviewErrorMessage { get; set; }

        protected void InitReviewValues()
        {
            AddReviewVisible = false;
            AddReviewErrorMessage = null;
            ReviewText = null;
            ReviewQuality = null;
        }
    }
}