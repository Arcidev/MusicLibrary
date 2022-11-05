using BusinessLayer.Facades;
using DotVVM.Core.Storage;
using DotVVM.Framework.Hosting;
using Shared.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    public class AlbumCreateViewModel : AlbumManagementMasterPageViewModel
    {
        public AlbumCreateViewModel(CategoryFacade categoryFacade, AlbumFacade albumFacade, BandFacade bandFacade, SongFacade songFacade, IUploadedFileStorage uploadedFileStorage) : base(categoryFacade, albumFacade, bandFacade, songFacade, uploadedFileStorage) { }

        public override async Task Init()
        {
            await Context.Authorize();
            await base.Init();
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Album = new ();
            }

            await base.PreRender();
        }

        public override async Task SaveChanges()
        {
            if (!ValidateAlbum())
                return;

            var role = Enum.Parse<UserRole>(UserRole);
            var success = await ExecuteSafelyAsync(async() =>
            {
                await albumFacade.AddAlbumAsync(new ()
                {
                    Approved = role != Shared.Enums.UserRole.User && Album.Approved,
                    BandId = SelectedBandId.Value,
                    CategoryId = SelectedCategoryId.Value,
                    Name = Album.Name,
                    AddedSongs = AddedSongs.Select(x => x.Id)
                }, Files.Files.LastOrDefault(), uploadedFileStorage);
            });

            if (success)
                Context.RedirectToRoute("AlbumsAdmin");
        }
    }
}
