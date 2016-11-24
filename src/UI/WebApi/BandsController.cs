using BL.Facades;
using System.Collections.Generic;
using System.Web.Http;
using WebApi.Models;
using WebApi.Models.Extensions;

namespace MusicLibrary.WebApi
{
    public class BandsController : ApiController
    {
        public BandFacade BandFacade { get; set; }

        [HttpGet]
        [ActionName("albums")]
        public IEnumerable<AlbumViewModel> GetBandAlbums(int id)
        {
            return BandFacade.GetBandAlbums(id, null, null, true).ToAlbumViewModel();
        }

        [HttpGet]
        [ActionName("list")]
        public IEnumerable<BandViewModel> GetBands()
        {
            return BandFacade.GetBands().ToBandViewModel();
        }

        [HttpGet]
        [ActionName("get")]
        public BandViewModel GetBand(int id)
        {
            return BandFacade.GetBand(id, false).ToBandViewModel();
        }
    }
}
