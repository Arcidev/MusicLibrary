using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class FeaturedAlbumsQuery : AppQuery<AlbumDTO>
    {
        public FeaturedAlbumsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            return Context.Albums.Where(x => x.Approved).OrderByDescending(x => x.Reviews.Average(y => (int)y.Quality)).ProjectTo<AlbumDTO>();
        }
    }
}
