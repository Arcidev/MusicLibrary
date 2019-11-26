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
    public class BandsController : ControllerBase
    {
        private readonly BandFacade bandFacade;

        public BandsController(BandFacade bandFacade)
        {
            this.bandFacade = bandFacade;
        }

        [HttpGet("{id}/albums")]
        public async Task<IEnumerable<AlbumViewModel>> GetBandAlbums(int id)
        {
            return (await bandFacade.GetBandAlbumsAsync(id, null, null, true)).ToAlbumViewModel();
        }

        [HttpGet]
        public async Task<IEnumerable<BandViewModel>> GetBands()
        {
            return (await bandFacade.GetBandsAsync()).ToBandViewModel();
        }

        [HttpGet("{id}")]
        public async Task<BandViewModel> GetBand(int id)
        {
            return (await bandFacade.GetBandAsync(id, false, false)).ToBandViewModel();
        }
    }
}
