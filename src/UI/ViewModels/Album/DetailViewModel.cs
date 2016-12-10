using System.Threading.Tasks;
using BL.Facades;
using DotVVM.Framework.ViewModel;
using BL.DTO;
using System.Collections.Generic;
using System.Linq;
using Shared.Enums;
using System;

namespace MusicLibrary.ViewModels.Album
{
    public class DetailViewModel : DetailMasterPageViewModel
    {
        [Bind(Direction.None)]
        public AlbumFacade AlbumFacade { get; set; }

        [Bind(Direction.None)]
        public BandFacade BandFacade { get; set; }

        public AlbumDTO Album { get; set; }

        public IEnumerable<AlbumDTO> OtherBandAlbums { get; set; }

        public string YoutubeUrlParam { get; set; }

        public string AudioFile { get; set; }

        public bool HasOtherBandAlbums { get; set; }

        public bool HasInCollection { get; set; }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                int albumId = int.Parse(Context.Parameters["AlbumId"].ToString());
                Album = AlbumFacade.GetAlbum(albumId);
                OtherBandAlbums = BandFacade.GetBandAlbums(Album.BandId, Album.Id, 6, true);
                HasOtherBandAlbums = OtherBandAlbums.Any();

                int userId;
                if (int.TryParse(UserId, out userId))
                    HasInCollection = await AlbumFacade.HasUserInCollection(userId, albumId);
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
                await AlbumFacade.AddAlbumToUserCollection(new UserAlbumCreateDTO()
                {
                    AlbumId = id,
                    UserId = int.Parse(UserId)
                });

                HasInCollection = true;
            });
        }

        public async Task RemoveFromCollection(int id)
        {
            await ExecuteSafelyAsync(async () =>
            {
                await AlbumFacade.RemoveAlbumFromUserCollection(int.Parse(UserId), id);

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
                AlbumFacade.AddReview(new AlbumReviewCreateDTO()
                {
                    AlbumId = albumId,
                    CreatedById = int.Parse(UserId),
                    Quality = (Quality)int.Parse(ReviewQuality),
                    Text = ReviewText
                });

                InitReviewValues();
            }, failureCallback: (ex) => ReviewErrorMessage = ex.Message);
        }

        protected override void LoadReviews()
        {
            AlbumFacade.LoadReviews(int.Parse(Context.Parameters["AlbumId"].ToString()), Reviews);
        }

        protected override Action<int, ReviewEditDTO> GetEditReviewAction()
        {
            return AlbumFacade.EditUserReview;
        }
    }
}
