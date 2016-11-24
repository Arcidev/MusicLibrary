using BL.Facades;
using System.Collections.Generic;
using System.Web.Http;
using WebApi.Models;
using WebApi.Models.Extensions;

namespace MusicLibrary.WebApi
{
    public class AlbumsController : ApiController
    {
        public AlbumFacade AlbumFacade { get; set; }

        [HttpGet]
        [ActionName("songs")]
        public IEnumerable<SongViewModel> GetAlbumSongs(int id)
        {
            return AlbumFacade.GetAlbumSongs(id).ToSongViewModel();
        }

        [HttpGet]
        [ActionName("list")]
        public IEnumerable<AlbumViewModel> GetAlbums()
        {
            return AlbumFacade.GetAlbums().ToAlbumViewModel();
        }

        [HttpGet]
        [ActionName("get")]
        public AlbumViewModel GetAlbum(int id)
        {
            return AlbumFacade.GetAlbum(id, false, false).ToAlbumViewModel();
        }
    }
}
