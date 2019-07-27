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
                Album = albumFacade.GetAlbum(albumId);
                OtherBandAlbums = bandFacade.GetBandAlbums(Album.BandId, Album.Id, 6, true);
                HasOtherBandAlbums = OtherBandAlbums.Any();

                var userId = UserId;
                if (userId > 0)
                    HasInCollection = albumFacade.HasUserInCollection(userId, albumId);
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

        public void AddToCollection(int id)
        {
            ExecuteSafely(() =>
            {
                albumFacade.AddAlbumToUserCollection(new UserAlbumCreateDTO()
                {
                    AlbumId = id,
                    UserId = UserId
                });

                HasInCollection = true;
            });
        }

        public void RemoveFromCollection(int id)
        {
            ExecuteSafely(() =>
            {
                albumFacade.RemoveAlbumFromUserCollection(UserId, id);

                HasInCollection = false;
            });
        }

        public override void AddReview()
        {
            if (!ValidateReview())
                return;

            ExecuteSafely(() =>
            {
                var albumId = int.Parse(Context.Parameters["AlbumId"].ToString());
                albumFacade.AddReview(new AlbumReviewCreateDTO()
                {
                    AlbumId = albumId,
                    CreatedById = UserId,
                    Quality = (Quality)int.Parse(ReviewQuality),
                    Text = ReviewText
                });

                InitReviewValues();
            }, failureCallback: (ex) => ReviewErrorMessage = ex.Message);
        }

        protected override void LoadReviews()
        {
            albumFacade.LoadReviews(int.Parse(Context.Parameters["AlbumId"].ToString()), Reviews);
        }

        protected override Action<int, ReviewEditDTO> GetEditReviewAction()
        {
            return albumFacade.EditUserReview;
        }
    }
}
