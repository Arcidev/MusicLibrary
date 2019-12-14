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
    public class AlbumCreateViewModel : AlbumManagementMasterPageViewModel
    {
        public AlbumCreateViewModel(CategoryFacade categoryFacade, AlbumFacade albumFacade, BandFacade bandFacade, SongFacade songFacade, IUploadedFileStorage uploadedFileStorage) : base(categoryFacade, albumFacade, bandFacade, songFacade, uploadedFileStorage) { }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Album = new AlbumBaseDTO();
            }

            await base.PreRender();
        }

        public override async Task SaveChanges()
        {
            if (!ValidateAlbum())
                return;

            var role = (UserRole)Enum.Parse(typeof(UserRole), UserRole);
            var success = await ExecuteSafelyAsync(async() =>
            {
                await albumFacade.AddAlbumAsync(new AlbumCreateDTO()
                {
                    Approved = role != Shared.Enums.UserRole.User ? Album.Approved : false,
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
