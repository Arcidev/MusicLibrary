using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Runtime.Filters;
using Shared.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize]
    public class AlbumCreateViewModel : AlbumManagementMasterPageViewModel
    {
        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Album = new AlbumBaseDTO();
            }

            await base.PreRender();
        }

        public override void SaveChanges()
        {
            if (!ValidateAlbum())
                return;

            var role = (UserRole)Enum.Parse(typeof(UserRole), UserRole);
            var success = ExecuteSafely(() =>
            {
                AlbumFacade.AddAlbum(new AlbumCreateDTO()
                {
                    Approved = role != Shared.Enums.UserRole.User ? Album.Approved : false,
                    BandId = SelectedBandId.Value,
                    CategoryId = SelectedCategoryId.Value,
                    Name = Album.Name,
                    AddedSongs = AddedSongs.Select(x => x.Id)
                }, Files.Files.LastOrDefault(), FileStorage);
            });

            if (success)
                Context.RedirectToRoute("AlbumsAdmin");
        }
    }
}