using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Linq;
using Shared.Enums;

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
                    Quality = (Quality)ReviewQuality.Value,
                    Text = ReviewText
                });

                Reviews = BandFacade.GetReviews(bandId);
                InitReviewValues();
            }, failureCallback: (ex) => AddReviewErrorMessage = ex.Message);
        }

        protected override void LoadReviews()
        {
            Reviews = BandFacade.GetReviews(int.Parse(Context.Parameters["BandId"].ToString()));
        }
    }
}
