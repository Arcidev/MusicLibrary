using DotVVM.Framework.Configuration;
using DotVVM.Framework.ResourceManagement;
using MusicLibrary.AppStart;
using MusicLibrary.Presenters;

namespace MusicLibrary
{
    public class DotvvmStartup : IDotvvmStartup
    {
        // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            ConfigureRoutes(config, applicationPath);
            ConfigureControls(config, applicationPath);
            ConfigureResources(config, applicationPath);
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
            config.RouteTable.Add("TempFilePresenter", "files/{FileId}/{FileExtension}", null, null, WindsorBootstrap.Resolve<TempFilePresenter>);
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            config.Markup.AddMarkupControl("cc", "LoadingAnimation", "Controls/LoadingAnimation.dotcontrol");
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            config.Resources.Register("templateStyle-css", new StylesheetResource()
            {
                Url = "~/Content/Styles/templateStyle.min.css"
            });
            config.Resources.Register("style-css", new StylesheetResource()
            {
                Url = "~/Content/Styles/style.min.css"
            });
            config.Resources.Register("modernizr", new ScriptResource()
            {
                Url = "~/Content/Scripts/modernizr.custom.53451.js"
            });
            config.Resources.Register("jquery-gallery", new ScriptResource()
            {
                Url = "~/Content/Scripts/jquery.gallery.js",
                Dependencies = new[] { "jquery", "modernizr" }
            });
            config.Resources.Register("jquery-slider", new ScriptResource()
            {
                Url = "~/Content/Scripts/jquery-slider.js",
                Dependencies = new[] { "jquery" }
            });
            config.Resources.Register("index", new ScriptResource()
            {
                Url = "~/Content/Scripts/index.js",
                Dependencies = new[] { "jquery-gallery" }
            });
            config.Resources.Register("stars", new ScriptResource()
            {
                Url = "~/Content/Scripts/stars.js",
                Dependencies = new[] { "jquery" }
            });
        }
    }
}
