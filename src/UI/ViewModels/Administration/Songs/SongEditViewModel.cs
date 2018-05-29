using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Runtime.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize(Roles = new[] { nameof(Shared.Enums.UserRole.SuperUser), nameof(Shared.Enums.UserRole.Admin) })]
    public class SongEditViewModel : SongManagementMasterPageViewModel
    {
        public string OriginalAudioFileName { get; set; }

        public IList<AlbumBandInfoDTO> SongAlbums { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                var song = SongFacade.GetSong(int.Parse(Context.Parameters["SongId"].ToString()));
                Song = new SongBaseDTO()
                {
                    Approved = song.Approved,
                    Name = song.Name,
                    YoutubeUrlParam = song.YoutubeUrlParam
                };
                OriginalAudioFileName = song.AudioStorageFile?.FileName;
                SongAlbums = AlbumFacade.GetAlbumBandInfoes(song.Id).ToList();
                ResetSong();
            }

            return base.PreRender();
        }

        public override void ResetSong()
        {
            SongFileName = OriginalAudioFileName != null ? $"/SavedFiles/{OriginalAudioFileName}" : "";
            Files.Clear();
        }

        public override void SaveChanges()
        {
            if (!ValidateSong())
                return;

            var success = ExecuteSafely(() =>
            {
                SongFacade.EditSong(new SongEditDTO()
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
