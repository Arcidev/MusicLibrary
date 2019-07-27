using BusinessLayer.DTO;
using BusinessLayer.Facades;
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

        protected DetailMasterPageViewModel(CategoryFacade categoryFacade) : base(categoryFacade)
        {
            Reviews = new GridViewDataSet<ReviewDTO>()
            {
                PagingOptions = new PagingOptions()
                {
                    PageSize = 10
                },
                SortingOptions = new SortingOptions()
                {
                    SortDescending = true,
                    SortExpression = nameof(ReviewDTO.EditDate)
                }
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
                    CreatedById = UserId,
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

            if (UserId == 0)
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
