using BusinessLayer.Facades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public IEnumerable<AlbumViewModel> GetCategoryAlbums(int id)
        {
            return categoryFacade.GetAlbumsByCategory(id).ToAlbumViewModel();
        }

        [HttpGet]
        public IEnumerable<CategoryViewModel> GetCategories()
        {
            return categoryFacade.GetCategories().ToCategoryViewModel();
        }

        [HttpGet("{id}")]
        public CategoryViewModel GetCategory(int id)
        {
            return categoryFacade.GetCategory(id).ToCategoryViewModel();
        }
    }
}
