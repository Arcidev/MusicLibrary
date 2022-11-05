using BusinessLayer.Facades;
using DotVVM.Core.Storage;
using DotVVM.Framework.Hosting;
using Shared.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    public class SongCreateViewModel : SongManagementMasterPageViewModel
    {
        public SongCreateViewModel(AlbumFacade albumFacade, SongFacade songFacade, IUploadedFileStorage uploadedFileStorage) : base(albumFacade, songFacade, uploadedFileStorage) { }

        public override async Task Init()
        {
            await Context.Authorize();
            await base.Init();
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Song = new ();
            }

            await base.PreRender();
        }

        public override async Task SaveChanges()
        {
            if (!ValidateSong())
                return;

            var role = Enum.Parse<UserRole>(UserRole);
            var success = await ExecuteSafelyAsync(async () =>
            {
                await songFacade.AddSongAsync(new ()
                {
                    Approved = role != Shared.Enums.UserRole.User && Song.Approved,
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
