using Devcorner.NIdenticon;
using Devcorner.NIdenticon.BrushGenerators;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MusicLibrary.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdenticonController : ControllerBase
    {
        [HttpGet("{name}")]
        public FileStreamResult DownloadFile(string name)
        {
            var identicon = new IdenticonGenerator("SHA512", new Size(180, 180), Color.Transparent, new Size(8, 8))
            {
                DefaultBlockGenerators = IdenticonGenerator.ExtendedBlockGeneratorsConfig,
                DefaultBrushGenerator = new StaticColorBrushGenerator(Color.Black)
            };

            using var bitmap = identicon.Create(name);
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Position = 0;

            return File(ms, "image/png");
        }
    }
}
