using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class BandReviewsQuery : AppQuery<ReviewDTO>
    {
        public int BandId { get; set; }

        public BandReviewsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<ReviewDTO> GetQueryable()
        {
            return Context.BandReviews.Where(x => x.BandId == BandId).ProjectTo<ReviewDTO>();
        }
    }
}
