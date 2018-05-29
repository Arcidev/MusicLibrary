using DotVVM.Framework.Configuration;
using DotVVM.Framework.ResourceManagement;
using DotVVM.Framework.Storage;
using DotVVM.Framework.ViewModel.Serialization;
using Microsoft.Extensions.DependencyInjection;
using MusicLibrary.AppStart;
using MusicLibrary.Presenters;
using System;
using System.IO;
using System.Web.Hosting;

namespace MusicLibrary
{
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
    {
        // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            ConfigureRoutes(config, applicationPath);
            ConfigureControls(config, applicationPath);
            ConfigureResources(config, applicationPath);
        }

        public void ConfigureServices(IDotvvmServiceCollection options)
        {
            options.Services.AddSingleton<IViewModelLoader>(serviceProvider => new WindsorViewModelLoader(WindsorBootstrap.container));
            options.Services.AddSingleton<IUploadedFileStorage>(serviceProvider => new FileSystemUploadedFileStorage(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Temp"), TimeSpan.FromMinutes(30)));
        }

        private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
        {
            config.RouteTable.Add("Index", "", "Views/index.dothtml");
            config.RouteTable.Add("AlbumDetail", "album/{AlbumId}/detail", "Views/Album/detail.dothtml");
            config.RouteTable.Add("Login", "login", "Views/Login/login.dothtml");
            config.RouteTable.Add("Register", "register", "Views/Login/register.dothtml");
            config.RouteTable.Add("Bands", "bands", "Views/bands.dothtml");
            config.RouteTable.Add("Albums", "albums", "Views/albums.dothtml");
            config.RouteTable.Add("BandDetail", "band/{BandId}/detail", "Views/Band/detail.dothtml");
            config.RouteTable.Add("AlbumsFiltered", "albums/{Filter}", "Views/albums.dothtml");
            config.RouteTable.Add("AlbumsByCategory", "albums/cateogry/{CategoryId}", "Views/albums.dothtml");
            config.RouteTable.Add("UserCollection", "collection", "Views/userCollection.dothtml");
            config.RouteTable.Add("UserProfile", "administration/profile", "Views/Administration/userProfile.dothtml");
            config.RouteTable.Add("UserReviews", "administration/reviews", "Views/Administration/userReviews.dothtml");
            config.RouteTable.Add("UserCollectionAdmin", "administration/collection", "Views/Administration/userCollection.dothtml");
            config.RouteTable.Add("AlbumsAdmin", "administration/albums", "Views/Administration/Albums/albums.dothtml");
            config.RouteTable.Add("AlbumEdit", "administration/album/{albumId}/edit", "Views/Administration/Albums/albumEdit.dothtml");
            config.RouteTable.Add("AlbumCreate", "administration/album/create", "Views/Administration/Albums/albumCreate.dothtml");
            config.RouteTable.Add("BandsAdmin", "administration/bands", "Views/Administration/Bands/bands.dothtml");
            config.RouteTable.Add("BandEdit", "administration/band/{bandId}/edit", "Views/Administration/Bands/bandEdit.dothtml");
            config.RouteTable.Add("BandCreate", "administration/band/create", "Views/Administration/Bands/bandCreate.dothtml");
            config.RouteTable.Add("SongsAdmin", "administration/songs", "Views/Administration/Songs/songs.dothtml");
            config.RouteTable.Add("SongEdit", "administration/song/{songId}/edit", "Views/Administration/Songs/songEdit.dothtml");
            config.RouteTable.Add("SongCreate", "administration/song/create", "Views/Administration/Songs/songCreate.dothtml");
            config.RouteTable.Add("Users", "administration/users", "Views/Administration/users.dothtml");
            config.RouteTable.Add("TempFilePresenter", "files/{FileId}/{FileExtension}", serviceProvider => WindsorBootstrap.Resolve<TempFilePresenter>());
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            config.Markup.AddMarkupControl("cc", "LoadingAnimation", "Controls/LoadingAnimation.dotcontrol");
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            config.Resources.Register("templateStyle-css", new StylesheetResource()
            {
                Location = new UrlResourceLocation("~/Content/Styles/templateStyle.min.css")
            });
            config.Resources.Register("style-css", new StylesheetResource()
            {
                Location =  new UrlResourceLocation("~/Content/Styles/style.min.css")
            });
            config.Resources.Register("modernizr", new ScriptResource()
            {
                Location = new UrlResourceLocation("~/Content/Scripts/modernizr.custom.53451.js")
            });
            config.Resources.Register("jquery", new ScriptResource()
            {
                Location = new UrlResourceLocation("https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js")
            });
            config.Resources.Register("jquery-gallery", new ScriptResource()
            {
                Location = new UrlResourceLocation("~/Content/Scripts/jquery.gallery.js"),
                Dependencies = new[] { "jquery", "modernizr" }
            });
            config.Resources.Register("jquery-slider", new ScriptResource()
            {
                Location = new UrlResourceLocation("~/Content/Scripts/jquery-slider.js"),
                Dependencies = new[] { "jquery" }
            });
            config.Resources.Register("index", new ScriptResource()
            {
                Location = new UrlResourceLocation("~/Content/Scripts/index.js"),
                Dependencies = new[] { "jquery-gallery" }
            });
            config.Resources.Register("stars", new ScriptResource()
            {
                Location = new UrlResourceLocation("~/Content/Scripts/stars.js"),
                Dependencies = new[] { "jquery" }
            });
        }
    }
}
