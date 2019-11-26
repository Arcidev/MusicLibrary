using AutoMapper;
using BusinessLayer.DTO;
using BusinessLayer.Queries;
using BusinessLayer.Repositories;
using BusinessLayer.Resources;
using DataLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO category)
        {
            using var uow = uowProviderFunc().Create();
            var entity = mapper.Map<Category>(category);
            var repo = categoryRepositoryFunc();
            repo.Insert(entity);

            await uow.CommitAsync();
            return mapper.Map<CategoryDTO>(entity);
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            using var uow = uowProviderFunc().Create();
            var query = categoriesQueryFunc();

            return await query.ExecuteAsync();
        }

        public async Task<CategoryDTO> GetCategoryAsync(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = categoryRepositoryFunc();
            var entity = await repo.GetByIdAsync(id);
            IsNotNull(entity, ErrorMessages.CategoryNotExist);

            return mapper.Map<CategoryDTO>(entity);
        }

        public async Task<CategoryDTO> EditCategoryAsync(CategoryDTO category)
        {
            using var uow = uowProviderFunc().Create();
            var repo = categoryRepositoryFunc();
            var entity = await repo.GetByIdAsync(category.Id);
            mapper.Map(category, entity);

            await uow.CommitAsync();
            return mapper.Map<CategoryDTO>(entity);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = categoryRepositoryFunc();
            repo.Delete(id);

            await uow.CommitAsync();
        }

        public async Task<IEnumerable<AlbumDTO>> GetAlbumsByCategoryAsync(int categoryId)
        {
            using var uow = uowProviderFunc().Create();
            var query = categoryAlbumsQueryFunc();
            query.CategoryId = categoryId;

            return await query.ExecuteAsync();
        }
    }
}
