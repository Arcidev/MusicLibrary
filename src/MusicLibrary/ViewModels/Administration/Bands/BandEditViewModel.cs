using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize(Roles = nameof(Shared.Enums.UserRole.SuperUser) + ", " + nameof(Shared.Enums.UserRole.Admin))]
    public class BandEditViewModel : BandManagementMasterPageViewModel
    {
        public string OriginalImageFileName { get; set; }

        public BandEditViewModel(BandFacade bandFacade, IUploadedFileStorage uploadedFileStorage) : base(bandFacade, uploadedFileStorage) { }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                var band = await bandFacade.GetBandAsync(int.Parse(Context.Parameters["BandId"].ToString()), true, true);
                Band = new ()
                {
                    Approved = band.Approved,
                    Description = band.Description,
                    Name = band.Name
                };
                OriginalImageFileName = band.ImageStorageFile?.FileName;
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
            if (!ValidateBand())
                return;

            var success = await ExecuteSafelyAsync(async () =>
            {
                await bandFacade.EditBandAsync(new ()
                {
                    Id = int.Parse(Context.Parameters["BandId"].ToString()),
                    Approved = Band.Approved,
                    Name = Band.Name,
                    Description = Band.Description,
                }, Files.Files.LastOrDefault(), uploadedFileStorage);
            });

            if (success)
                Context.RedirectToRoute("BandsAdmin");
        }
    }
}
