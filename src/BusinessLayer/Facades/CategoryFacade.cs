using AutoMapper;
using BusinessLayer.DTO;
using BusinessLayer.Queries;
using BusinessLayer.Repositories;
using BusinessLayer.Resources;
using DataLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Facades
{
    public class CategoryFacade : BaseFacade
    {
        private readonly Func<CategoryRepository> categoryRepositoryFunc;
        private readonly Func<CategoriesQuery> categoriesQueryFunc;
        private readonly Func<CategoryAlbumsQuery> categoryAlbumsQueryFunc;
        
        public CategoryFacade(Func<CategoryRepository> categoryRepositoryFunc,
            Func<CategoriesQuery> categoriesQueryFunc,
            Func<CategoryAlbumsQuery> categoryAlbumsQueryFunc,
            IMapper mapper,
            Func<IUnitOfWorkProvider> uowProvider) : base(mapper, uowProvider)
        {
            this.categoryRepositoryFunc = categoryRepositoryFunc;
            this.categoriesQueryFunc = categoriesQueryFunc;
            this.categoryAlbumsQueryFunc = categoryAlbumsQueryFunc;
        }

        public CategoryDTO AddCategory(CategoryDTO category)
        {
            using var uow = uowProviderFunc().Create();
            var entity = mapper.Map<Category>(category);
            var repo = categoryRepositoryFunc();
            repo.Insert(entity);

            uow.Commit();
            return mapper.Map<CategoryDTO>(entity);
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            using var uow = uowProviderFunc().Create();
            var query = categoriesQueryFunc();
            return query.Execute();
        }

        public CategoryDTO GetCategory(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = categoryRepositoryFunc();
            var entity = repo.GetById(id);
            IsNotNull(entity, ErrorMessages.CategoryNotExist);

            return mapper.Map<CategoryDTO>(entity);
        }

        public CategoryDTO EditCategory(CategoryDTO category)
        {
            using var uow = uowProviderFunc().Create();
            var repo = categoryRepositoryFunc();
            var entity = repo.GetById(category.Id);
            mapper.Map(category, entity);

            uow.Commit();
            return mapper.Map<CategoryDTO>(entity);
        }

        public void DeleteCategory(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = categoryRepositoryFunc();
            repo.Delete(id);

            uow.Commit();
        }

        public IEnumerable<AlbumDTO> GetAlbumsByCategory(int categoryId)
        {
            using var uow = uowProviderFunc().Create();
            var query = categoryAlbumsQueryFunc();
            query.CategoryId = categoryId;

            return query.Execute();
        }
    }
}
