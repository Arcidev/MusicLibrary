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

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                int bandId = int.Parse(Context.Parameters["BandId"].ToString());
                Band = await bandFacade.GetBandAsync(bandId);
                HasAlbums = Band.Albums.Any();
                HasMembers = Band.Members.Any();
            }

            await base.PreRender();
        }

        public override async Task AddReview()
        {
            if (!ValidateReview())
                return;

            await ExecuteSafelyAsync(async () =>
            {
                await bandFacade.AddReviewAsync(new ()
                {
                    BandId = int.Parse(Context.Parameters["BandId"].ToString()),
                    CreatedById = UserId,
                    Quality = (Quality)int.Parse(ReviewQuality),
                    Text = ReviewText
                });

                InitReviewValues();
            }, failureCallback: (ex) => ReviewErrorMessage = ex.Message);
        }

        protected override async Task LoadReviews() => await bandFacade.LoadReviewsAsync(int.Parse(Context.Parameters["BandId"].ToString()), Reviews);

        protected override Func<int, ReviewEditDTO, Task> GetEditReviewAction() => bandFacade.EditUserReviewAsync;
    }
}
