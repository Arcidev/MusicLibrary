using BusinessLayer.Facades;
using DotVVM.Core.Storage;
using DotVVM.Framework.Hosting;
using Shared.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    public class BandCreateViewModel : BandManagementMasterPageViewModel
    {
        public BandCreateViewModel(BandFacade bandFacade, IUploadedFileStorage uploadedFileStorage) : base(bandFacade, uploadedFileStorage) { }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Band = new ();
            }

            await base.PreRender();
        }

        public override async Task Init()
        {
            await Context.Authorize();
            await base.Init();
        }

        public override async Task SaveChanges()
        {
            if (!ValidateBand())
                return;

            var role = Enum.Parse<UserRole>(UserRole);
            var success = await ExecuteSafelyAsync(async () =>
            {
                await bandFacade.AddBandAsync(new ()
                {
                    Approved = role != Shared.Enums.UserRole.User && Band.Approved,
                    Description = Band.Description,
                    Name = Band.Name
                }, Files.Files.LastOrDefault(), uploadedFileStorage);
            });

            if (success)
                Context.RedirectToRoute("BandsAdmin");
        }
    }
}
