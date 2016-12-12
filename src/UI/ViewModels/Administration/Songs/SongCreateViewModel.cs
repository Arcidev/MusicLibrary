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
    public class SongCreateViewModel : SongManagementMasterPageViewModel
    {
        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Song = new SongBaseDTO();
            }

            await base.PreRender();
        }

        public override void SaveChanges()
        {
            if (!ValidateSong())
                return;

            var role = (UserRole)Enum.Parse(typeof(UserRole), UserRole);
            var success = ExecuteSafely(() =>
            {
                SongFacade.AddSong(new SongCreateDTO()
                {
                    Approved = role != Shared.Enums.UserRole.User ? Song.Approved : false,
                    Name = Song.Name,
                    AddedAlbums = AddedAlbums.Select(x => x.AlbumId),
                    YoutubeUrlParam = ParseYTUrl()
                }, Files.Files.LastOrDefault(), FileStorage);
            });

            if (success)
                Context.RedirectToRoute("SongsAdmin");
        }
    }
}
