using BusinessLayer.DTO;
using Shared.Enums;
using BusinessLayer.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Album
{
    public class DetailViewModel : DetailMasterPageViewModel
    {
        private readonly AlbumFacade albumFacade;
        private readonly BandFacade bandFacade;

        public AlbumDTO Album { get; set; }

        public IEnumerable<AlbumDTO> OtherBandAlbums { get; set; }

        public string YoutubeUrlParam { get; set; }

        public string AudioFile { get; set; }

        public bool HasOtherBandAlbums { get; set; }

        public bool HasInCollection { get; set; }

        public DetailViewModel(AlbumFacade albumFacade, BandFacade bandFacade, CategoryFacade categoryFacade) : base(categoryFacade)
        {
            this.albumFacade = albumFacade;
            this.bandFacade = bandFacade;
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                int albumId = int.Parse(Context.Parameters["AlbumId"].ToString());
                Album = await albumFacade.GetAlbumAsync(albumId);
                OtherBandAlbums = await bandFacade.GetBandAlbumsAsync(Album.BandId, Album.Id, 6, true);
                HasOtherBandAlbums = OtherBandAlbums.Any();

                var userId = UserId;
                if (userId > 0)
                    HasInCollection = await albumFacade.HasUserInCollectionAsync(userId, albumId);
            }

            await base.PreRender();
        }

        public void SetAudioFile(SongDTO song)
        {
            YoutubeUrlParam = null;
            AudioFile = song.AudioStorageFile?.FileName;
        }

        public void SetYoutubeVideo(SongDTO song)
        {
            AudioFile = null;
            YoutubeUrlParam = song.YoutubeUrlParam != null ? $"https://www.youtube.com/embed/{song.YoutubeUrlParam}" : null;
        }

        public async Task AddToCollection(int id)
        {
            await ExecuteSafelyAsync(async () =>
            {
                await albumFacade.AddAlbumToUserCollectionAsync(new ()
                {
                    AlbumId = id,
                    UserId = UserId
                });

                HasInCollection = true;
            });
        }

        public async Task RemoveFromCollection(int id)
        {
            await ExecuteSafelyAsync(async () =>
            {
                await albumFacade.RemoveAlbumFromUserCollectionAsync(UserId, id);

                HasInCollection = false;
            });
        }

        public override async Task AddReview()
        {
            if (!ValidateReview())
                return;

            await ExecuteSafelyAsync(async () =>
            {
                var albumId = int.Parse(Context.Parameters["AlbumId"].ToString());
                await albumFacade.AddReviewAsync(new ()
                {
                    AlbumId = albumId,
                    CreatedById = UserId,
                    Quality = (Quality)int.Parse(ReviewQuality),
                    Text = ReviewText
                });

                InitReviewValues();
            }, failureCallback: (ex) => ReviewErrorMessage = ex.Message);
        }

        protected override async Task LoadReviews() => await albumFacade.LoadReviewsAsync(int.Parse(Context.Parameters["AlbumId"].ToString()), Reviews);

        protected override Func<int, ReviewEditDTO, Task> GetEditReviewAction() => albumFacade.EditUserReviewAsync;
    }
}
