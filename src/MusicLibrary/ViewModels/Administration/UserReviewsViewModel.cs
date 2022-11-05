using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;
using MusicLibrary.Resources;
using Shared.Enums;
using System;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    public class UserReviewsViewModel : AdministrationMasterPageViewModel
    {
        private readonly AlbumFacade albumFacade;
        private readonly BandFacade bandFacade;

        public GridViewDataSet<UserAlbumReviewDTO> AlbumReviews { get; set; }

        public GridViewDataSet<UserBandReviewDTO> BandReviews { get; set; }

        public int? BandReviewEditId { get; set; }

        public int? AlbumReviewEditId { get; set; }

        public string EditReviewQuality { get; set; }

        public string EditReviewText { get; set; }

        public string ErrorMessage { get; set; }

        public UserReviewsViewModel(AlbumFacade albumFacade, BandFacade bandFacade)
        {
            this.albumFacade = albumFacade;
            this.bandFacade = bandFacade;

            AlbumReviews = new ()
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
            BandReviews = new ()
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

        public override async Task Init()
        {
            await Context.Authorize();
            await base.Init();
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "UserReviews";
            }

            var userId = UserId;

            await Task.WhenAll(albumFacade.LoadUserReviewsAsync(userId, AlbumReviews), bandFacade.LoadUserReviewsAsync(userId, BandReviews));
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

        public async Task EditBandReview()
        {
            await EditReview(BandReviewEditId ?? 0, bandFacade.EditUserReviewAsync);
        }

        public async Task EditAlbumReview()
        {
            await EditReview(AlbumReviewEditId ?? 0, albumFacade.EditUserReviewAsync);
        }

        public void CancelEdit()
        {
            BandReviewEditId = null;
            AlbumReviewEditId = null;
        }

        private async Task EditReview(int id, Func<int, ReviewEditDTO, Task> action)
        {
            if (string.IsNullOrEmpty(EditReviewText))
            {
                ErrorMessage = Texts.ReviewTextRequired;
                return;
            }

            await ExecuteSafelyAsync(async () =>
            {
                await action(id, new ()
                {
                    CreatedById = UserId,
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
