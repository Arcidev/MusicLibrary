using DotVVM.Framework.Configuration;
using DotVVM.Framework.ResourceManagement;

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

            // Uncomment the following line to auto-register all dothtml files in the Views folder
            // config.RouteTable.AutoDiscoverRoutes(new DefaultRouteStrategy(config));    
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            // register code-only controls and markup controls
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            config.Resources.Register("templateStyle-css", new StylesheetResource()
            {
                Url = "~/Content/Styles/templateStyle.css"
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
            config.Resources.Register("index", new ScriptResource()
            {
                Url = "~/Content/Scripts/index.js",
                Dependencies = new[] { "jquery-gallery" }
            });
        }
    }
}
