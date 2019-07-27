using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Runtime.Filters;
using Shared.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize]
    public class BandCreateViewModel : BandManagementMasterPageViewModel
    {
        public BandCreateViewModel(BandFacade bandFacade) : base(bandFacade) { }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Band = new BandBaseDTO();
            }

            await base.PreRender();
        }

        public override void SaveChanges()
        {
            if (!ValidateBand())
                return;

            var role = (UserRole)Enum.Parse(typeof(UserRole), UserRole);
            var success = ExecuteSafely(() =>
            {
                bandFacade.AddBand(new BandBaseDTO()
                {
                    Approved = role != Shared.Enums.UserRole.User ? Band.Approved : false,
                    Description = Band.Description,
                    Name = Band.Name
                }, Files.Files.LastOrDefault(), FileStorage);
            });

            if (success)
                Context.RedirectToRoute("BandsAdmin");
        }
    }
}
