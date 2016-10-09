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
            var query = Context.Albums.Where(x => x.Approved).OrderByDescending(x => x.Reviews.Average(y => (int)y.Quality));
            if (Take.HasValue)
                return query.Take(Take.Value).ProjectTo<AlbumDTO>();

            return query.ProjectTo<AlbumDTO>();
        }
    }
}
