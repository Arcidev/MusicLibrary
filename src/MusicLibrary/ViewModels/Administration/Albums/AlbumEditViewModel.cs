using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Core.Storage;
using DotVVM.Framework.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    public class AlbumEditViewModel : AlbumManagementMasterPageViewModel
    {
        public string OriginalImageFileName { get; set; }

        public IEnumerable<SongInfoDTO> AlbumSongs { get; set; }

        public AlbumEditViewModel(CategoryFacade categoryFacade, AlbumFacade albumFacade, BandFacade bandFacade, SongFacade songFacade, IUploadedFileStorage uploadedFileStorage) : base(categoryFacade, albumFacade, bandFacade, songFacade, uploadedFileStorage) { }

        public override async Task Init()
        {
            await Context.Authorize(new[] { nameof(Shared.Enums.UserRole.SuperUser), nameof(Shared.Enums.UserRole.Admin) });
            await base.Init();
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                var album = await albumFacade.GetAlbumAsync(int.Parse(Context.Parameters["AlbumId"].ToString()), false, false);
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
                AlbumSongs = await albumFacade.GetAlbumSongInfoesAsync(album.Id);
                ResetImage();
            }

            await base.PreRender();
        }

        public override void ResetImage()
        {
            ImageFileName = OriginalImageFileName != null ? $"/SavedFiles/{OriginalImageFileName}" : "";
            Files.Clear();
        }

        public override async Task SaveChanges()
        {
            if (!ValidateAlbum())
                return;

            var success = await ExecuteSafelyAsync(async () =>
            {
                await albumFacade.EditAlbumAsync(new ()
                {
                    Id = int.Parse(Context.Parameters["AlbumId"].ToString()),
                    Approved = Album.Approved,
                    BandId = SelectedBandId.Value,
                    CategoryId = SelectedCategoryId.Value,
                    Name = Album.Name,
                    RemovedSongs = AlbumSongs.Where(x => x.Removed).Select(x => x.Id),
                    AddedSongs = AddedSongs.Select(x => x.Id)
                }, Files.Files.LastOrDefault(), uploadedFileStorage);
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
