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
    public abstract class AlbumManagementMasterPageViewModel : AdministrationMasterPageViewModel
    {
        protected IUploadedFileStorage FileStorage { get { return Context.Configuration.ServiceLocator.GetService<IUploadedFileStorage>(); } }

        [Bind(Direction.None)]
        public AlbumFacade AlbumFacade { get; set; }

        [Bind(Direction.None)]
        public BandFacade BandFacade { get; set; }

        [Bind(Direction.None)]
        public CategoryFacade CategoryFacade { get; set; }

        public AlbumBaseDTO Album { get; set; }

        public UploadedFilesCollection Files { get; set; } = new UploadedFilesCollection();

        public AlbumManagementErrorViewModel AlbumManagementErrorViewModel { get; set; }

        public IEnumerable<BandInfoDTO> BandInfoes { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public string ImageFileName { get; set; }

        public int? SelectedCategoryId { get; set; }

        public int? SelectedBandId  { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                BandInfoes = BandFacade.GetBandInfoes();
                Categories = CategoryFacade.GetCategories();
            }
            
            return base.PreRender();
        }

        public void UploadedImage()
        {
            var file = Files.Files.LastOrDefault();
            if (file == null)
                return;

            ImageFileName = $"/files/{file.FileId}/{Path.GetExtension(file.FileName).Substring(1)}";
        }

        public virtual void ResetImage()
        {
            ImageFileName = null;
            Files.Clear();
        }

        public abstract void SaveChanges();

        protected bool ValidateAlbum()
        {
            AlbumManagementErrorViewModel = new AlbumManagementErrorViewModel();

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
