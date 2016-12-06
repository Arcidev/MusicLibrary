using BL.DTO;
using MusicLibrary.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
	public abstract class DetailMasterPageViewModel : ContentMasterPageViewModel
    {
        public IEnumerable<ReviewDTO> Reviews { get; set; }

        public int? ReviewQuality { get; set; }

        public string ReviewText { get; set; }

        public bool AddReviewVisible { get; set; }

        public string AddReviewErrorMessage { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                LoadReviews();
            }

            return base.PreRender();
        }

        public abstract void AddReview();

        protected abstract void LoadReviews();

        protected bool ValidateReview()
        {
            if (string.IsNullOrEmpty(ReviewText))
            {
                AddReviewErrorMessage = Texts.ReviewTextRequired;
                return false;
            }

            if (!ReviewQuality.HasValue)
            {
                AddReviewErrorMessage = Texts.ReviewQualityRequired;
                return false;
            }

            int userId;
            if (!int.TryParse(UserId, out userId))
            {
                AddReviewErrorMessage = Texts.UserNotLoggedIn;
                return false;
            }

            return true;
        }

        protected void InitReviewValues()
        {
            AddReviewVisible = false;
            AddReviewErrorMessage = null;
            ReviewText = null;
            ReviewQuality = null;
        }
    }
}
