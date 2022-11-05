using Jdenticon;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace MusicLibrary.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdenticonController : ControllerBase
    {
        [HttpGet("{value}")]
        public FileStreamResult GetIdenticon(string value)
        {
            var ms = new MemoryStream();
            Identicon.FromValue(value, 180, "SHA512").SaveAsSvg(ms);
            ms.Position = 0;

            return File(ms, "image/svg+xml");
        }
    }
}
