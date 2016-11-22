using BL.DTO;
using BL.Facades;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi.Models;

namespace MusicLibrary.WebApi
{
    public class AlbumsController : ApiController
    {
        public AlbumFacade AlbumFacade { get; set; }

        public SongFacade SongFacade { get; set; }

        [HttpGet]
        [ActionName("albumSongs")]
        public IEnumerable<SongViewModel> GetAlbumSongs(int id)
        {
            return AlbumFacade.GetAlbumSongs(id).Select(x => new SongViewModel()
            {
                AudioStorageFileId = x.AudioStorageFileId,
                CreateDate = x.CreateDate,
                Id = x.Id,
                Name = x.Name,
                YoutubeUrlParam = x.YoutubeUrlParam
            });
        }

        [HttpGet]
        [ActionName("list")]
        public IEnumerable<AlbumViewModel> GetAlbums()
        {
            return AlbumFacade.GetAlbums().Select(ToAlbumViewmModel);
        }

        private AlbumViewModel ToAlbumViewmModel(AlbumDTO dto)
        {
            return new AlbumViewModel()
            {
                BandId = dto.BandId,
                CategoryId = dto.CategoryId,
                CreateDate = dto.CreateDate,
                Id = dto.Id,
                ImageStorageFileId = dto.ImageStorageFileId,
                Name = dto.Name
            };
        }
    }
}
