using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Runtime.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize(Roles = nameof(Shared.Enums.UserRole.SuperUser) + ", " + nameof(Shared.Enums.UserRole.Admin))]
    public class SongEditViewModel : SongManagementMasterPageViewModel
    {
        public string OriginalAudioFileName { get; set; }

        public IList<AlbumBandInfoDTO> SongAlbums { get; set; }

        public SongEditViewModel(AlbumFacade albumFacade, SongFacade songFacade) : base(albumFacade, songFacade) { }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                var song = await songFacade.GetSongAsync(int.Parse(Context.Parameters["SongId"].ToString()));
                Song = new SongBaseDTO()
                {
                    Approved = song.Approved,
                    Name = song.Name,
                    YoutubeUrlParam = song.YoutubeUrlParam
                };
                OriginalAudioFileName = song.AudioStorageFile?.FileName;
                SongAlbums = (await albumFacade.GetAlbumBandInfoesAsync(song.Id)).ToList();
                ResetSong();
            }

            await base.PreRender();
        }

        public override void ResetSong()
        {
            SongFileName = OriginalAudioFileName != null ? $"/SavedFiles/{OriginalAudioFileName}" : "";
            Files.Clear();
        }

        public override async Task SaveChanges()
        {
            if (!ValidateSong())
                return;

            var success = await ExecuteSafelyAsync(async () =>
            {
                await songFacade.EditSongAsync(new SongEditDTO()
                {
                    Id = int.Parse(Context.Parameters["SongId"].ToString()),
                    Approved = Song.Approved,
                    Name = Song.Name,
                    YoutubeUrlParam = ParseYTUrl(),
                    AddedAlbums = AddedAlbums.Select(x => x.AlbumId),
                    RemovedAlbums = SongAlbums.Where(x => x.Removed).Select(x => x.AlbumId)
                }, Files.Files.LastOrDefault(), FileStorage);
            });

            if (success)
                Context.RedirectToRoute("SongsAdmin");
        }

        public void SetRemovedAlbum(AlbumBandInfoDTO album, bool removed)
        {
            album.Removed = removed;
        }

        protected override void OnAlbumInfoesLoaded()
        {
            foreach (var songAlbum in SongAlbums)
                AlbumInfoes.Remove(songAlbum);
        }
    }
}
