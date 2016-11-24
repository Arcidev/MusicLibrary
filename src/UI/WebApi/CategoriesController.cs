using BL.Facades;
using System.Collections.Generic;
using System.Web.Http;
using WebApi.Models;
using WebApi.Models.Extensions;

namespace MusicLibrary.WebApi
{
    public class CategoriesController : ApiController
    {
        public CategoryFacade CategoryFacade { get; set; }

        [HttpGet]
        [ActionName("albums")]
        public IEnumerable<AlbumViewModel> GetCategoryAlbums(int id)
        {
            return CategoryFacade.GetAlbumsByCategory(id).ToAlbumViewModel();
        }

        [HttpGet]
        [ActionName("list")]
        public IEnumerable<CategoryViewModel> GetCategories()
        {
            return CategoryFacade.GetCategories().ToCategoryViewModel();
        }

        [HttpGet]
        [ActionName("get")]
        public CategoryViewModel GetCategory(int id)
        {
            return CategoryFacade.GetCategory(id).ToCategoryViewModel();
        }
    }
}
