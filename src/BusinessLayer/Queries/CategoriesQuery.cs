using BusinessLayer.DTO;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class CategoriesQuery : AppQuery<CategoryDTO>
    {
        public CategoriesQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<CategoryDTO> GetQueryable()
        {
            return Context.Categories.ProjectToType<CategoryDTO>();
        }
    }
}
