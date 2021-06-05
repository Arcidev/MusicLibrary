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
    public abstract class SongManagementMasterPageViewModel : AdministrationMasterPageViewModel
    {
        protected readonly AlbumFacade albumFacade;
        protected readonly SongFacade songFacade;
        protected readonly IUploadedFileStorage uploadedFileStorage;

        public List<AlbumBandInfoDTO> AddedAlbums { get; set; } = new ();

        public List<AlbumBandInfoDTO> AlbumInfoes { get; set; }

        public SongBaseDTO Song { get; set; }

        public UploadedFilesCollection Files { get; set; } = new ();

        public string SongNameError { get; set; }

        public string SongFileName { get; set; }

        public int? SelectedAlbumId { get; set; }

        protected SongManagementMasterPageViewModel(AlbumFacade albumFacade, SongFacade songFacade, IUploadedFileStorage uploadedFileStorage)
        {
            this.albumFacade = albumFacade;
            this.songFacade = songFacade;
            this.uploadedFileStorage = uploadedFileStorage;
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                var albumInfoes = await albumFacade.GetAlbumBandInfoesAsync();
                ActiveAdminPage = "Songs";
                AlbumInfoes = albumInfoes as List<AlbumBandInfoDTO> ?? albumInfoes.ToList();
                OnAlbumInfoesLoaded();
            }
            
            await base.PreRender();
        }

        public void AddAlbum()
        {
            if (!SelectedAlbumId.HasValue)
                return;

            if (AddedAlbums.Any(x => x.AlbumId == SelectedAlbumId.Value))
                return;

            var album = AlbumInfoes.FirstOrDefault(x => x.AlbumId == SelectedAlbumId.Value);
            if (album == null)
                return;

            AddedAlbums.Add(album);
            AlbumInfoes.Remove(album);
        }

        public void RemoveAddedAlbum(int id)
        {
            var album = AddedAlbums.FirstOrDefault(x => x.AlbumId == id);
            if (album == null)
                return;

            AddedAlbums.Remove(album);
            AlbumInfoes.Add(album);
        }

        public void UploadedSong()
        {
            var file = Files.Files.LastOrDefault();
            if (file == null)
                return;

            SongFileName = $"/files/{file.FileId}/{Path.GetExtension(file.FileName)[1..]}";
        }

        public virtual void ResetSong()
        {
            SongFileName = null;
            Files.Clear();
        }

        public abstract Task SaveChanges();

        protected virtual void OnAlbumInfoesLoaded() { }

        protected bool ValidateSong()
        {
            SongNameError = string.IsNullOrEmpty(Song.Name) ? Texts.NameRequired : null;
            return SongNameError == null;
        }

        protected string ParseYTUrl()
        {
            var videoParam = "watch?v=";
            var index = Song.YoutubeUrlParam?.IndexOf(videoParam);
            if (index >= 0)
                return Song.YoutubeUrlParam[(index.Value + videoParam.Length)..];

            return Song.YoutubeUrlParam;
        }
    }
}
