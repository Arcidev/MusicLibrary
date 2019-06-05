using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class CategoryAlbumsQuery : AppQuery<AlbumDTO>
    {
        public CategoryAlbumsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        public int CategoryId { get; set; }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            return Context.Albums.Where(x => x.CategoryId == CategoryId).ProjectTo<AlbumDTO>(mapperConfig);
        }
    }
}
