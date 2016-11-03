using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class AlbumReviewsQuery : AppQuery<AlbumReviewDTO>
    {
        public int? UserId { get; set; }

        public int? AlbumId { get; set; }

        public AlbumReviewsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<AlbumReviewDTO> GetQueryable()
        {
            var query = Context.AlbumReviews.AsQueryable();
            if (UserId.HasValue)
                query = query.Where(x => x.CreatedById == UserId.Value);

            if (AlbumId.HasValue)
                query = query.Where(x => x.AlbumId == AlbumId.Value);

            return query.ProjectTo<AlbumReviewDTO>();
        }
    }
}
