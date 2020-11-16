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
            Reviews = new ()
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

        public override async Task PreRender()
        {
            await LoadReviews();
            await base.PreRender();
        }

        public void Edit(ReviewDTO review)
        {
            AddReviewVisible = false;
            ReviewErrorMessage = null;
            ReviewEditId = review.Id;
            ReviewText = review.Text;
            ReviewQuality = review.QualityInt.ToString();
        }

        public async Task EditReview()
        {
            if (!ValidateReview())
                return;

            await ExecuteSafelyAsync(async () =>
            {
                await GetEditReviewAction()(ReviewEditId ?? 0, new ()
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

        public abstract Task AddReview();

        protected abstract Task LoadReviews();

        protected abstract Func<int, ReviewEditDTO, Task> GetEditReviewAction();

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
