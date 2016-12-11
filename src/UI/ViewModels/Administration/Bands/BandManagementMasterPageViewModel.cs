using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Storage;
using DotVVM.Framework.ViewModel;
using MusicLibrary.Resources;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    public abstract class BandManagementMasterPageViewModel : AdministrationMasterPageViewModel
    {
        protected IUploadedFileStorage FileStorage { get { return Context.Configuration.ServiceLocator.GetService<IUploadedFileStorage>(); } }

        [Bind(Direction.None)]
        public BandFacade BandFacade { get; set; }

        public BandBaseDTO Band { get; set; }

        public UploadedFilesCollection Files { get; set; } = new UploadedFilesCollection();

        public BandManagementErrorViewModel BandManagementErrorViewModel { get; set; }

        public string ImageFileName { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "Bands";
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

        protected bool ValidateBand()
        {
            BandManagementErrorViewModel = new BandManagementErrorViewModel();

            if (string.IsNullOrEmpty(Band.Name))
                BandManagementErrorViewModel.NameError = Texts.NameRequired;

            if (string.IsNullOrEmpty(Band.Description))
                BandManagementErrorViewModel.DescriptionError = Texts.DescriptionRequired;

            return !BandManagementErrorViewModel.ContainsError;
        }
    }
}
