using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Runtime.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize(Roles = new[] { nameof(Shared.Enums.UserRole.SuperUser), nameof(Shared.Enums.UserRole.Admin) })]
    public class AlbumEditViewModel : AlbumManagementMasterPageViewModel
    {
        public string OriginalImageFileName { get; set; }

        public IList<SongInfoDTO> AlbumSongs { get; set; }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                var albumId = int.Parse(Context.Parameters["AlbumId"].ToString());
                var album = AlbumFacade.GetAlbum(albumId, false, false);
                Album = album;
                OriginalImageFileName = album.ImageStorageFile?.FileName;
                SelectedBandId = album.BandId;
                SelectedCategoryId = album.CategoryId;
                AlbumSongs = AlbumFacade.GetAlbumSongInfoes(albumId).ToList();
                ResetImage();
            }

            await base.PreRender();
        }

        public override void ResetImage()
        {
            ImageFileName = OriginalImageFileName != null ? $"/SavedFiles/{OriginalImageFileName}" : "";
            Files.Clear();
        }

        public override void SaveChanges()
        {
            if (!ValidateAlbum())
                return;

            var success = ExecuteSafely(() =>
            {
                AlbumFacade.EditAlbum(new AlbumEditDTO()
                {
                    Id = int.Parse(Context.Parameters["AlbumId"].ToString()),
                    Approved = Album.Approved,
                    BandId = SelectedBandId.Value,
                    CategoryId = SelectedCategoryId.Value,
                    Name = Album.Name,
                    RemovedSongs = AlbumSongs.Where(x => x.Removed).Select(x => x.Id),
                    AddedSongs = AddedSongs.Select(x => x.Id)
                }, Files.Files.LastOrDefault(), FileStorage);
            });

            if (success)
                Context.RedirectToRoute("AlbumsAdmin");
        }

        public void SetRemovedSong(SongInfoDTO song, bool removed)
        {
            song.Removed = removed;
        }

        protected override void OnSongsLoaded()
        {
            foreach (var albumSong in AlbumSongs)
                SongInfoes.Remove(albumSong);
        }
    }
}
