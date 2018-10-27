using BL.Facades;
using System.Collections.Generic;
using System.Web.Http;
using WebApi.Models;
using WebApi.Models.Extensions;

namespace MusicLibrary.WebApi
{
    public class SongsController : ApiController
    {
        public SongFacade SongFacade { get; set; }

        [HttpGet, ActionName("list")]
        public IEnumerable<SongViewModel> GetSongs()
        {
            return SongFacade.GetSongs().ToSongViewModel();
        }

        [HttpGet, ActionName("get")]
        public SongViewModel GetSong(int id)
        {
            return SongFacade.GetSong(id).ToSongViewModel();
        }
    }
}
