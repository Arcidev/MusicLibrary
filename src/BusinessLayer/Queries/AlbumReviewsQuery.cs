using BusinessLayer.DTO;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class AlbumReviewsQuery : AppQuery<ReviewDTO>
    {
        public int AlbumId { get; set; }

        public AlbumReviewsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<ReviewDTO> GetQueryable()
        {
            return Context.AlbumReviews.Where(x => x.AlbumId == AlbumId).ProjectToType<ReviewDTO>();
        }
    }
}
