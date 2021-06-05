using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Core.Storage;
using DotVVM.Framework.Controls;
using MusicLibrary.Resources;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    public abstract class AlbumManagementMasterPageViewModel : AdministrationMasterPageViewModel
    {
        protected readonly CategoryFacade categoryFacade;
        protected readonly AlbumFacade albumFacade;
        protected readonly BandFacade bandFacade;
        protected readonly SongFacade songFacade;
        protected readonly IUploadedFileStorage uploadedFileStorage;

        public AlbumBaseDTO Album { get; set; }

        public UploadedFilesCollection Files { get; set; } = new UploadedFilesCollection();

        public AlbumManagementErrorViewModel AlbumManagementErrorViewModel { get; set; }

        public List<SongInfoDTO> AddedSongs { get; set; } = new ();

        public IEnumerable<BandInfoDTO> BandInfoes { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public List<SongInfoDTO> SongInfoes { get; set; }

        public string ImageFileName { get; set; }

        public int? SelectedCategoryId { get; set; }

        public int? SelectedBandId  { get; set; }

        public int? SelectedSongId { get; set; }

        protected AlbumManagementMasterPageViewModel(CategoryFacade categoryFacade, AlbumFacade albumFacade, BandFacade bandFacade, SongFacade songFacade, IUploadedFileStorage uploadedFileStorage)
        {
            this.categoryFacade = categoryFacade;
            this.albumFacade = albumFacade;
            this.bandFacade = bandFacade;
            this.songFacade = songFacade;
            this.uploadedFileStorage = uploadedFileStorage;
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "Albums";

                await Task.WhenAll(Task.Run(async () => BandInfoes = await bandFacade.GetBandInfoesAsync()),
                    Task.Run(async () => Categories = await categoryFacade.GetCategoriesAsync()),
                    Task.Run(async () => { var songInfoes = await songFacade.GetSongInfoesAsync(); SongInfoes = songInfoes as List<SongInfoDTO> ?? songInfoes.ToList(); }));

                OnSongsLoaded();
            }
            
            await base.PreRender();
        }

        public void AddSong()
        {
            if (!SelectedSongId.HasValue)
                return;

            if (AddedSongs.Any(x => x.Id == SelectedSongId.Value))
                return;

            var song = SongInfoes.FirstOrDefault(x => x.Id == SelectedSongId.Value);
            if (song == null)
                return;

            AddedSongs.Add(song);
            SongInfoes.Remove(song);
        }

        public void RemoveAddedSong(int id)
        {
            var song = AddedSongs.FirstOrDefault(x => x.Id == id);
            if (song == null)
                return;

            AddedSongs.Remove(song);
            SongInfoes.Add(song);
        }

        public void UploadedImage()
        {
            var file = Files.Files.LastOrDefault();
            if (file == null)
                return;

            ImageFileName = $"/files/{file.FileId}/{Path.GetExtension(file.FileName)[1..]}";
        }

        public virtual void ResetImage()
        {
            ImageFileName = null;
            Files.Clear();
        }

        public abstract Task SaveChanges();

        protected virtual void OnSongsLoaded() { }

        protected bool ValidateAlbum()
        {
            AlbumManagementErrorViewModel = new ();

            if (string.IsNullOrEmpty(Album.Name))
                AlbumManagementErrorViewModel.NameError = Texts.NameRequired;

            if (!SelectedBandId.HasValue)
                AlbumManagementErrorViewModel.BandError = Texts.BandRequired;

            if (!SelectedCategoryId.HasValue)
                AlbumManagementErrorViewModel.CategoryError = Texts.CategoryRequired;

            return !AlbumManagementErrorViewModel.ContainsError;
        }
    }
}
