using BL.DTO;
using MusicLibrary.Resources;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
	public abstract class DetailMasterPageViewModel : ContentMasterPageViewModel
    {
        public IEnumerable<ReviewDTO> Reviews { get; set; }

        public string ReviewQuality { get; set; }

        public string ReviewText { get; set; }

        public bool AddReviewVisible { get; set; }

        public string ReviewErrorMessage { get; set; }

        public int? ReviewEditId { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                LoadReviews();
            }

            return base.PreRender();
        }

        public void Edit(ReviewDTO review)
        {
            AddReviewVisible = false;
            ReviewErrorMessage = null;
            ReviewEditId = review.Id;
            ReviewText = review.Text;
            ReviewQuality = review.QualityInt.ToString();
        }

        public void EditReview()
        {
            if (!ValidateReview())
                return;

            ExecuteSafely(() =>
            {
                GetEditReviewAction()(ReviewEditId ?? 0, new ReviewEditDTO()
                {
                    CreatedById = int.Parse(UserId),
                    Quality = (Quality)int.Parse(ReviewQuality),
                    Text = ReviewText
                });

                CancelEdit();
            }, failureCallback: (ex) => ReviewErrorMessage = ex.Message);
        }

        public void CancelEdit()
        {
            ReviewEditId = null;
        }

        public void ShowAddReview()
        {
            InitReviewValues();
            AddReviewVisible = true;
            ReviewEditId = null;
        }

        public abstract void AddReview();

        protected abstract void LoadReviews();

        protected abstract Action<int, ReviewEditDTO> GetEditReviewAction();

        protected bool ValidateReview()
        {
            if (string.IsNullOrEmpty(ReviewText))
            {
                ReviewErrorMessage = Texts.ReviewTextRequired;
                return false;
            }

            if (string.IsNullOrEmpty(ReviewQuality))
            {
                ReviewErrorMessage = Texts.ReviewQualityRequired;
                return false;
            }

            int userId;
            if (!int.TryParse(UserId, out userId))
            {
                ReviewErrorMessage = Texts.UserNotLoggedIn;
                return false;
            }

            return true;
        }

        protected void InitReviewValues()
        {
            AddReviewVisible = false;
            ReviewErrorMessage = null;
            ReviewText = null;
            ReviewQuality = null;
        }
    }
}
