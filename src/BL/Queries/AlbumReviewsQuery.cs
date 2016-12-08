using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class AlbumReviewsQuery : AppQuery<AlbumReviewDTO>
    {
        public int AlbumId { get; set; }

        public AlbumReviewsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<AlbumReviewDTO> GetQueryable()
        {
            return Context.AlbumReviews.Where(x => x.AlbumId == AlbumId).OrderByDescending(x => x.EditDate).ProjectTo<AlbumReviewDTO>();
        }
    }
}
