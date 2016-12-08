using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Linq;
using Shared.Enums;
using System;

namespace MusicLibrary.ViewModels.Band
{
    public class DetailViewModel : DetailMasterPageViewModel
    {
        [Bind(Direction.None)]
        public BandFacade BandFacade { get; set; }

        public BandDTO Band { get; set; }

        public bool HasAlbums { get; set; }

        public bool HasMembers { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                int bandId = int.Parse(Context.Parameters["BandId"].ToString());
                Band = BandFacade.GetBand(bandId);
                HasAlbums = Band.Albums.Any();
                HasMembers = Band.Members.Any();
            }

            return base.PreRender();
        }

        public override void AddReview()
        {
            if (!ValidateReview())
                return;

            ExecuteSafely(() =>
            {
                var bandId = int.Parse(Context.Parameters["BandId"].ToString());
                BandFacade.AddReview(new BandReviewCreateDTO()
                {
                    BandId = bandId,
                    CreatedById = int.Parse(UserId),
                    Quality = (Quality)int.Parse(ReviewQuality),
                    Text = ReviewText
                });

                InitReviewValues();
            }, failureCallback: (ex) => ReviewErrorMessage = ex.Message);
        }

        protected override void LoadReviews()
        {
            BandFacade.LoadReviews(int.Parse(Context.Parameters["BandId"].ToString()), Reviews);
        }

        protected override Action<int, ReviewEditDTO> GetEditReviewAction()
        {
            return BandFacade.EditUserReview;
        }
    }
}
