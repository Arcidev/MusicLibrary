using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Runtime.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize(Roles = new[] { nameof(Shared.Enums.UserRole.SuperUser), nameof(Shared.Enums.UserRole.Admin) })]
    public class AlbumEditViewModel : AlbumManagementMasterPageViewModel
    {
        public string OriginalImageFileName { get; set; }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                var album = AlbumFacade.GetAlbum(int.Parse(Context.Parameters["AlbumId"].ToString()), false, true);
                Album = album;
                OriginalImageFileName = album.ImageStorageFile?.FileName;
                SelectedBandId = album.BandId;
                SelectedCategoryId = album.CategoryId;
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
            if (!ValidateAlbum())
                return;

            ExecuteSafely(() =>
            {
                AlbumFacade.EditAlbum(new AlbumEditDTO()
                {
                    Id = int.Parse(Context.Parameters["AlbumId"].ToString()),
                    Approved = Album.Approved,
                    BandId = SelectedBandId.Value,
                    CategoryId = SelectedCategoryId.Value,
                    Name = Album.Name,
                    RemovedSongs = null,
                    AddedSongs = null
                }, Files.Files.LastOrDefault(), FileStorage);
            });
        }
    }
}
