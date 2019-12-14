using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.Storage;
using Shared.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize]
    public class BandCreateViewModel : BandManagementMasterPageViewModel
    {
        public BandCreateViewModel(BandFacade bandFacade, IUploadedFileStorage uploadedFileStorage) : base(bandFacade, uploadedFileStorage) { }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Band = new BandBaseDTO();
            }

            await base.PreRender();
        }

        public override async Task SaveChanges()
        {
            if (!ValidateBand())
                return;

            var role = (UserRole)Enum.Parse(typeof(UserRole), UserRole);
            var success = await ExecuteSafelyAsync(async () =>
            {
                await bandFacade.AddBandAsync(new BandBaseDTO()
                {
                    Approved = role != Shared.Enums.UserRole.User ? Band.Approved : false,
                    Description = Band.Description,
                    Name = Band.Name
                }, Files.Files.LastOrDefault(), uploadedFileStorage);
            });

            if (success)
                Context.RedirectToRoute("BandsAdmin");
        }
    }
}
