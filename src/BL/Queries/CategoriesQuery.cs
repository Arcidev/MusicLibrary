using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class CategoriesQuery : AppQuery<CategoryDTO>
    {
        public CategoriesQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<CategoryDTO> GetQueryable()
        {
            return Context.Categories.ProjectTo<CategoryDTO>(mapperConfig);
        }
    }
}
