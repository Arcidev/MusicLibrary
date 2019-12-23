using BusinessLayer.DTO;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class UserBandReviewsQuery : AppQuery<UserBandReviewDTO>
    {
        public int UserId { get; set; }

        public UserBandReviewsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<UserBandReviewDTO> GetQueryable()
        {
            return Context.BandReviews.Where(x => x.CreatedById == UserId).ProjectToType<UserBandReviewDTO>();
        }
    }
}
