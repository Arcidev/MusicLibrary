using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class BandReviewsQuery : AppQuery<ReviewDTO>
    {
        public int BandId { get; set; }

        public BandReviewsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<ReviewDTO> GetQueryable()
        {
            return Context.BandReviews.Where(x => x.BandId == BandId).ProjectTo<ReviewDTO>(mapperConfig);
        }
    }
}
