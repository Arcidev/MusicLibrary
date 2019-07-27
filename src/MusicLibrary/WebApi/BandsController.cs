using BusinessLayer.Facades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApiModels;
using WebApiModels.Extensions;

namespace MusicLibrary.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class BandsController : ControllerBase
    {
        private readonly BandFacade bandFacade;

        public BandsController(BandFacade bandFacade)
        {
            this.bandFacade = bandFacade;
        }

        [HttpGet("{id}/albums")]
        public IEnumerable<AlbumViewModel> GetBandAlbums(int id)
        {
            return bandFacade.GetBandAlbums(id, null, null, true).ToAlbumViewModel();
        }

        [HttpGet]
        public IEnumerable<BandViewModel> GetBands()
        {
            return bandFacade.GetBands().ToBandViewModel();
        }

        [HttpGet("{id}")]
        public BandViewModel GetBand(int id)
        {
            return bandFacade.GetBand(id, false, false).ToBandViewModel();
        }
    }
}
