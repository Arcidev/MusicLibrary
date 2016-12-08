using BL.DTO;
using DotVVM.Framework.Controls;
using MusicLibrary.Resources;
using Shared.Enums;
using System;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
	public abstract class DetailMasterPageViewModel : ContentMasterPageViewModel
    {
        public GridViewDataSet<ReviewDTO> Reviews { get; set; }

        public string ReviewQuality { get; set; }

        public string ReviewText { get; set; }

        public bool AddReviewVisible { get; set; }

        public string ReviewErrorMessage { get; set; }

        public int? ReviewEditId { get; set; }

        protected DetailMasterPageViewModel()
        {
            Reviews = new GridViewDataSet<ReviewDTO>()
            {
                PageSize = 10,
                SortDescending = true,
                SortExpression = nameof(ReviewDTO.EditDate)
            };
        }

        public override Task PreRender()
        {
            LoadReviews();

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
