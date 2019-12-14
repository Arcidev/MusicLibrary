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
    public class SongCreateViewModel : SongManagementMasterPageViewModel
    {
        public SongCreateViewModel(AlbumFacade albumFacade, SongFacade songFacade, IUploadedFileStorage uploadedFileStorage) : base(albumFacade, songFacade, uploadedFileStorage) { }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Song = new SongBaseDTO();
            }

            await base.PreRender();
        }

        public override async Task SaveChanges()
        {
            if (!ValidateSong())
                return;

            var role = (UserRole)Enum.Parse(typeof(UserRole), UserRole);
            var success = await ExecuteSafelyAsync(async () =>
            {
                await songFacade.AddSongAsync(new SongCreateDTO()
                {
                    Approved = role != Shared.Enums.UserRole.User ? Song.Approved : false,
                    Name = Song.Name,
                    AddedAlbums = AddedAlbums.Select(x => x.AlbumId),
                    YoutubeUrlParam = ParseYTUrl()
                }, Files.Files.LastOrDefault(), uploadedFileStorage);
            });

            if (success)
                Context.RedirectToRoute("SongsAdmin");
        }
    }
}
