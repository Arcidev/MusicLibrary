using BusinessLayer.Installers;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ResourceManagement;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MusicLibrary.Presenters;
using System.Linq;
using System.Reflection;

namespace MusicLibrary
{
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
    {
        // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            ConfigureRoutes(config);
            ConfigureControls(config);
            ConfigureResources(config);
        }

        private static void ConfigureRoutes(DotvvmConfiguration config)
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
            config.RouteTable.Add("TempFilePresenter", "files/{FileId}/{FileExtension}", serviceProvider => serviceProvider.GetRequiredService<TempFilePresenter>());
        }

        private static void ConfigureControls(DotvvmConfiguration config)
        {
            config.Markup.AddMarkupControl("cc", "LoadingAnimation", "Controls/LoadingAnimation.dotcontrol");
        }

        private static void ConfigureResources(DotvvmConfiguration config)
        {
            config.Resources.Register("jquery", new ScriptResource
            {
                Location = new UrlResourceLocation("https://code.jquery.com/jquery-3.6.1.min.js"),
                IntegrityHash = "sha384-i61gTtaoovXtAbKjo903+O55Jkn2+RtzHtvNez+yI49HAASvznhe9sZyjaSHTau9"
            });
            config.Resources.Register("templateStyle-css", new StylesheetResource()
            {
                Location = new UrlResourceLocation("~/Content/Styles/templateStyle.min.css")
            });
            config.Resources.Register("style-css", new StylesheetResource()
            {
                Location = new UrlResourceLocation("~/Content/Styles/style.min.css")
            });
            config.Resources.Register("modernizr", new ScriptResource()
            {
                Location = new UrlResourceLocation("~/Content/Scripts/modernizr.custom.53451.js")
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
            config.Resources.Register("stars", new ScriptResource()
            {
                Location = new UrlResourceLocation("~/Content/Scripts/stars.js"),
                Dependencies = new[] { "jquery" }
            });
        }

        public void ConfigureServices(IDotvvmServiceCollection options)
        {
            MapperInstaller.ConfigureMapper();
            options.Services.ConfigureServices()
                .ConfigureFacades();

            var vmType = typeof(IDotvvmViewModel);
            var vmPresenter = typeof(IDotvvmPresenter);
            var controllerType = typeof(ControllerBase);
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract && (vmType.IsAssignableFrom(t) || vmPresenter.IsAssignableFrom(t) || controllerType.IsAssignableFrom(t))))
            {
                options.Services.AddTransient(type);
            }

            options.AddDefaultTempStorages("temp");
        }
    }
}
