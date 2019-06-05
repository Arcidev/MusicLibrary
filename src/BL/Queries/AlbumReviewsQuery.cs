using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class AlbumReviewsQuery : AppQuery<ReviewDTO>
    {
        public int AlbumId { get; set; }

        public AlbumReviewsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<ReviewDTO> GetQueryable()
        {
            return Context.AlbumReviews.Where(x => x.AlbumId == AlbumId).ProjectTo<ReviewDTO>(mapperConfig);
        }
    }
}
