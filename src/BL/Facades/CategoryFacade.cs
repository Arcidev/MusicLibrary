using AutoMapper;
using BL.DTO;
using BL.Queries;
using BL.Repositories;
using BL.Resources;
using DAL.Entities;
using System;
using System.Collections.Generic;

namespace BL.Facades
{
    public class CategoryFacade : BaseFacade
    {
        public Func<CategoriesQuery> CategoriesQueryFunc { get; set; }

        public Func<CategoryAlbumsQuery> CategoryAlbumsQueryFunc { get; set; }

        public Func<CategoryRepository> CategoryRepositoryFunc { get; set; }

        public Func<IMapper> MapperInstance { get; set; }

        public CategoryDTO AddCategory(CategoryDTO category)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = MapperInstance().Map<Category>(category);
                var repo = CategoryRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();
                return MapperInstance().Map<CategoryDTO>(entity);
            }
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = CategoriesQueryFunc();
                return query.Execute();
            }
        }

        public CategoryDTO GetCategory(int id)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = CategoryRepositoryFunc();
                var entity = repo.GetById(id);
                IsNotNull(entity, ErrorMessages.CategoryNotExist);

                return MapperInstance().Map<CategoryDTO>(entity);
            }
        }

        public CategoryDTO EditCategory(CategoryDTO category)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = CategoryRepositoryFunc();
                var entity = repo.GetById(category.Id);
                MapperInstance().Map(category, entity);

                uow.Commit();
                return MapperInstance().Map<CategoryDTO>(entity); 
            }
        }

        public void DeleteCategory(int id)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = CategoryRepositoryFunc();
                repo.Delete(id);

                uow.Commit();
            }
        }

        public IEnumerable<AlbumDTO> GetAlbumsByCategory(int categoryId)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = CategoryAlbumsQueryFunc();
                query.CategoryId = categoryId;

                return query.Execute();
            }
        }
    }
}
