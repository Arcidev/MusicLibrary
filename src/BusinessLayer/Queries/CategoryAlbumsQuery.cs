using BusinessLayer.DTO;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class CategoryAlbumsQuery : AppQuery<AlbumDTO>
    {
        public CategoryAlbumsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        public int CategoryId { get; set; }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            return Context.Albums.Where(x => x.CategoryId == CategoryId).ProjectToType<AlbumDTO>();
        }
    }
}
