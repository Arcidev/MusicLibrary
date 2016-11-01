using AutoMapper;
using BL.DTO;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using System;
using System.Collections.Generic;

namespace BL.Facades
{
    public class CategoryFacade : BaseFacade
    {
        public Func<CategoriesQuery> CategoriesQueryFunc { get; set; }

        public Func<CategoryRepository> CategoryRepositoryFunc { get; set; }

        public CategoryDTO AddCategory(CategoryDTO category)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<Category>(category);
                var repo = CategoryRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();
                return Mapper.Map<CategoryDTO>(entity);
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
    }
}
