using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Runtime.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize(Roles = nameof(Shared.Enums.UserRole.SuperUser) + ", " + nameof(Shared.Enums.UserRole.Admin))]
    public class AlbumEditViewModel : AlbumManagementMasterPageViewModel
    {
        public string OriginalImageFileName { get; set; }

        public IList<SongInfoDTO> AlbumSongs { get; set; }

        public AlbumEditViewModel(CategoryFacade categoryFacade, AlbumFacade albumFacade, BandFacade bandFacade, SongFacade songFacade) : base(categoryFacade, albumFacade, bandFacade, songFacade) { }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                var album = albumFacade.GetAlbum(int.Parse(Context.Parameters["AlbumId"].ToString()), false, false);
                Album = new AlbumBaseDTO()
                {
                    Approved = album.Approved,
                    BandId = album.BandId,
                    CategoryId = album.CategoryId,
                    Name = album.Name
                };
                OriginalImageFileName = album.ImageStorageFile?.FileName;
                SelectedBandId = album.BandId;
                SelectedCategoryId = album.CategoryId;
                AlbumSongs = albumFacade.GetAlbumSongInfoes(album.Id).ToList();
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
                albumFacade.EditAlbum(new AlbumEditDTO()
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
