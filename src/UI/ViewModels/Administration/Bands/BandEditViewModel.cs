using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Runtime.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize(Roles = new[] { nameof(Shared.Enums.UserRole.SuperUser), nameof(Shared.Enums.UserRole.Admin) })]
    public class BandEditViewModel : BandManagementMasterPageViewModel
    {
        public string OriginalImageFileName { get; set; }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                var band = BandFacade.GetBand(int.Parse(Context.Parameters["BandId"].ToString()), true, true);
                Band = new BandBaseDTO()
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

        public override void SaveChanges()
        {
            if (!ValidateBand())
                return;

            var success = ExecuteSafely(() =>
            {
                BandFacade.EditBand(new BandEditDTO()
                {
                    Id = int.Parse(Context.Parameters["BandId"].ToString()),
                    Approved = Band.Approved,
                    Name = Band.Name,
                    Description = Band.Description,
                }, Files.Files.LastOrDefault(), FileStorage);
            });

            if (success)
                Context.RedirectToRoute("BandsAdmin");
        }
    }
}
