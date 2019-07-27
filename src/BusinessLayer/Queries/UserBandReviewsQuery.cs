using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class UserBandReviewsQuery : AppQuery<UserBandReviewDTO>
    {
        public int UserId { get; set; }

        public UserBandReviewsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<UserBandReviewDTO> GetQueryable()
        {
            return Context.BandReviews.Where(x => x.CreatedById == UserId).OrderByDescending(x => x.EditDate).ProjectTo<UserBandReviewDTO>(mapperConfig);
        }
    }
}
