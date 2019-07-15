using Devcorner.NIdenticon;
using Devcorner.NIdenticon.BrushGenerators;
using DotVVM.Framework.Hosting;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace MusicLibrary.Presenters
{
    public class IdenticonPresenter : IDotvvmPresenter
    {
        public Task ProcessRequest(IDotvvmRequestContext context)
        {
            var identicon = new IdenticonGenerator("SHA512", new Size(180, 180), Color.Transparent, new Size(8, 8))
            {
                DefaultBlockGenerators = IdenticonGenerator.ExtendedBlockGeneratorsConfig,
                DefaultBrushGenerator = new StaticColorBrushGenerator(Color.Black)
            };

            var name = Convert.ToString(context.Parameters["Identicon"]);
            using (var bitmap = identicon.Create(name))
            {
                // save it in the response stream
                context.HttpContext.Response.ContentType = "image/png";
                bitmap.Save(context.HttpContext.Response.Body, ImageFormat.Png);
            }

            return Task.FromResult(0);
        }
    }
}
