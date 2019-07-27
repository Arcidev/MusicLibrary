using BusinessLayer.Facades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public IEnumerable<SongViewModel> GetAlbumSongs(int id)
        {
            return albumFacade.GetAlbumSongs(id).ToSongViewModel();
        }

        [HttpGet]
        public IEnumerable<AlbumViewModel> GetAlbums()
        {
            return albumFacade.GetAlbums().ToAlbumViewModel();
        }

        [HttpGet("{id}")]
        public AlbumViewModel GetAlbum(int id)
        {
            return albumFacade.GetAlbum(id, false, false).ToAlbumViewModel();
        }
    }
}
