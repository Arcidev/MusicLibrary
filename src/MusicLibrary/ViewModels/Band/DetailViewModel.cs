using BusinessLayer.DTO;
using BusinessLayer.Facades;
using Shared.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Band
{
    public class DetailViewModel : DetailMasterPageViewModel
    {
        private readonly BandFacade bandFacade;

        public BandDTO Band { get; set; }

        public bool HasAlbums { get; set; }

        public bool HasMembers { get; set; }

        public DetailViewModel(BandFacade bandFacade, CategoryFacade categoryFacade) : base(categoryFacade)
        {
            this.bandFacade = bandFacade;
        }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                int bandId = int.Parse(Context.Parameters["BandId"].ToString());
                Band = bandFacade.GetBand(bandId);
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
                bandFacade.AddReview(new BandReviewCreateDTO()
                {
                    BandId = int.Parse(Context.Parameters["BandId"].ToString()),
                    CreatedById = UserId,
                    Quality = (Quality)int.Parse(ReviewQuality),
                    Text = ReviewText
                });

                InitReviewValues();
            }, failureCallback: (ex) => ReviewErrorMessage = ex.Message);
        }

        protected override void LoadReviews()
        {
            bandFacade.LoadReviews(int.Parse(Context.Parameters["BandId"].ToString()), Reviews);
        }

        protected override Action<int, ReviewEditDTO> GetEditReviewAction()
        {
            return bandFacade.EditUserReview;
        }
    }
}
