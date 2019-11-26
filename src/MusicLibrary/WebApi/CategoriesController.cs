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
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryFacade categoryFacade;

        public CategoriesController(CategoryFacade categoryFacade)
        {
            this.categoryFacade = categoryFacade;
        }

        [HttpGet("{id}/albums")]
        public async Task<IEnumerable<AlbumViewModel>> GetCategoryAlbums(int id)
        {
            return (await categoryFacade.GetAlbumsByCategoryAsync(id)).ToAlbumViewModel();
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
            return (await categoryFacade.GetCategoriesAsync()).ToCategoryViewModel();
        }

        [HttpGet("{id}")]
        public async Task<CategoryViewModel> GetCategory(int id)
        {
            return (await categoryFacade.GetCategoryAsync(id)).ToCategoryViewModel();
        }
    }
}
