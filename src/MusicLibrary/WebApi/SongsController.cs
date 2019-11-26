using BusinessLayer.Facades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiModels;
using WebApiModels.Extensions;

namespace MusicLibrary.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly SongFacade songFacade;

        public SongsController(SongFacade songFacade)
        {
            this.songFacade = songFacade;
        }

        [HttpGet]
        public async Task<IEnumerable<SongViewModel>> GetSongs()
        {
            return (await songFacade.GetSongsAsync()).ToSongViewModel();
        }

        [HttpGet("{id}")]
        public async Task<SongViewModel> GetSong(int id)
        {
            return (await songFacade.GetSongAsync(id)).ToSongViewModel();
        }
    }
}
