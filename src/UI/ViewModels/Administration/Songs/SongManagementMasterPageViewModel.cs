using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Storage;
using DotVVM.Framework.ViewModel;
using MusicLibrary.Resources;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    public abstract class SongManagementMasterPageViewModel : AdministrationMasterPageViewModel
    {
        protected IUploadedFileStorage FileStorage { get { return Context.Configuration.ServiceLocator.GetService<IUploadedFileStorage>(); } }

        [Bind(Direction.None)]
        public SongFacade SongFacade { get; set; }

        [Bind(Direction.None)]
        public AlbumFacade AlbumFacade { get; set; }

        public IList<AlbumBandInfoDTO> AddedAlbums { get; set; } = new List<AlbumBandInfoDTO>();

        public IList<AlbumBandInfoDTO> AlbumInfoes { get; set; }

        public SongBaseDTO Song { get; set; }

        public UploadedFilesCollection Files { get; set; } = new UploadedFilesCollection();

        public string SongNameError { get; set; }

        public string SongFileName { get; set; }

        public int? SelectedAlbumId { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "Songs";
                AlbumInfoes = AlbumFacade.GetAlbumBandInfoes().ToList();
                OnAlbumInfoesLoaded();
            }
            
            return base.PreRender();
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

            SongFileName = $"/files/{file.FileId}/{Path.GetExtension(file.FileName).Substring(1)}";
        }

        public virtual void ResetSong()
        {
            SongFileName = null;
            Files.Clear();
        }

        public abstract void SaveChanges();

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
                return Song.YoutubeUrlParam.Substring(index.Value + videoParam.Length);

            return Song.YoutubeUrlParam;
        }
    }
}
