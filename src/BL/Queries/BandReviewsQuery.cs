using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class BandReviewsQuery : AppQuery<BandReviewDTO>
    {
        public int? UserId { get; set; }

        public int? BandId { get; set; }

        public BandReviewsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<BandReviewDTO> GetQueryable()
        {
            var query = Context.BandReviews.AsQueryable();
            if (UserId.HasValue)
                query = query.Where(x => x.CreatedById == UserId.Value);

            if (BandId.HasValue)
                query = query.Where(x => x.BandId == BandId.Value);

            return query.OrderByDescending(x => x.EditDate).ProjectTo<BandReviewDTO>();
        }
    }
}
