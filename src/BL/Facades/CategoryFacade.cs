using BL.DTO;
using BL.Queries;
using System;
using System.Collections.Generic;

namespace BL.Facades
{
    public class CategoryFacade : BaseFacade
    {
        public Func<CategoriesQuery> CategoriesQueryFunc { get; set; }

        public IList<CategoryDTO> GetCategories()
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = CategoriesQueryFunc();
                return query.Execute();
            }
        }
    }
}
