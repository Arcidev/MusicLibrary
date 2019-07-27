using BusinessLayer.Facades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public IEnumerable<SongViewModel> GetSongs()
        {
            return songFacade.GetSongs().ToSongViewModel();
        }

        [HttpGet("{id}")]
        public SongViewModel GetSong(int id)
        {
            return songFacade.GetSong(id).ToSongViewModel();
        }
    }
}
