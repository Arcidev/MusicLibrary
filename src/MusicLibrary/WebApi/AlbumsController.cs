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
    public class AlbumsController : ControllerBase
    {
        private readonly AlbumFacade albumFacade;

        public AlbumsController(AlbumFacade albumFacade)
        {
            this.albumFacade = albumFacade;
        }

        [HttpGet("{id}/songs")]
        public async Task<IEnumerable<SongViewModel>> GetAlbumSongs(int id)
        {
            return (await albumFacade.GetAlbumSongsAsync(id)).ToSongViewModel();
        }

        [HttpGet]
        public async Task<IEnumerable<AlbumViewModel>> GetAlbums()
        {
            return (await albumFacade.GetAlbumsAsync()).ToAlbumViewModel();
        }

        [HttpGet("{id}")]
        public async Task<AlbumViewModel> GetAlbum(int id)
        {
            return (await albumFacade.GetAlbumAsync(id, false, false)).ToAlbumViewModel();
        }
    }
}
