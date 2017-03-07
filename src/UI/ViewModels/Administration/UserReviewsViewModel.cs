using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using MusicLibrary.Resources;
using Shared.Enums;
using System;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize]
    public class UserReviewsViewModel : AdministrationMasterPageViewModel
    {
        [Bind(Direction.None)]
        public AlbumFacade AlbumFacade { get; set; }

        [Bind(Direction.None)]
        public BandFacade BandFacade { get; set; }

        public GridViewDataSet<UserAlbumReviewDTO> AlbumReviews { get; set; }

        public GridViewDataSet<UserBandReviewDTO> BandReviews { get; set; }

        public int? BandReviewEditId { get; set; }

        public int? AlbumReviewEditId { get; set; }

        public string EditReviewQuality { get; set; }

        public string EditReviewText { get; set; }

        public string ErrorMessage { get; set; }

        public UserReviewsViewModel()
        {
            AlbumReviews = new GridViewDataSet<UserAlbumReviewDTO>()
            {
                PagingOptions = new PagingOptions()
                {
                    PageSize = 10
                },
                SortingOptions = new SortingOptions()
                {
                    SortDescending = true,
                    SortExpression = nameof(UserAlbumReviewDTO.EditDate)
                }
            };
            BandReviews = new GridViewDataSet<UserBandReviewDTO>()
            {
                PagingOptions = new PagingOptions()
                {
                    PageSize = 10
                },
                SortingOptions = new SortingOptions()
                {
                    SortDescending = true,
                    SortExpression = nameof(UserBandReviewDTO.EditDate)
                }
            };
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "UserReviews";
            }

            var userId = int.Parse(UserId);
            AlbumFacade.LoadUserReviews(userId, AlbumReviews);
            BandFacade.LoadUserReviews(userId, BandReviews);

            await base.PreRender();
        }

        public void Edit(UserBandReviewDTO review)
        {
            InitEditValues(review);
            BandReviewEditId = review.Id;
            AlbumReviewEditId = null;
        }

        public void Edit(UserAlbumReviewDTO review)
        {
            InitEditValues(review);
            BandReviewEditId = null;
            AlbumReviewEditId = review.Id;
        }

        public void EditBandReview()
        {
            EditReview(BandReviewEditId ?? 0, BandFacade.EditUserReview);
        }

        public void EditAlbumReview()
        {
            EditReview(AlbumReviewEditId ?? 0, AlbumFacade.EditUserReview);
        }

        public void CancelEdit()
        {
            BandReviewEditId = null;
            AlbumReviewEditId = null;
        }

        private void EditReview(int id, Action<int, ReviewEditDTO> action)
        {
            if (string.IsNullOrEmpty(EditReviewText))
            {
                ErrorMessage = Texts.ReviewTextRequired;
                return;
            }

            ExecuteSafely(() =>
            {
                action(id, new ReviewEditDTO()
                {
                    CreatedById = int.Parse(UserId),
                    Quality = (Quality)int.Parse(EditReviewQuality),
                    Text = EditReviewText
                });

                CancelEdit();
            }, failureCallback: (ex) => ErrorMessage = ex.Message);
        }

        private void InitEditValues(ReviewDTO review)
        {
            ErrorMessage = null;
            EditReviewQuality = review.QualityInt.ToString();
            EditReviewText = review.Text;
        }
    }
}
