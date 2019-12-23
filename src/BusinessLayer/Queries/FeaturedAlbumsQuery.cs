using BusinessLayer.DTO;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class FeaturedAlbumsQuery : AppQuery<AlbumDTO>
    {
        public FeaturedAlbumsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            return Context.Albums.Where(x => x.Approved).OrderByDescending(x => x.AverageQuality).ProjectToType<AlbumDTO>();
        }
    }
}
