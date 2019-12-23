using BusinessLayer.DTO;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class UserAlbumReviewsQuery : AppQuery<UserAlbumReviewDTO>
    {
        public int UserId { get; set; }

        public UserAlbumReviewsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<UserAlbumReviewDTO> GetQueryable()
        {
            return Context.AlbumReviews.Where(x => x.CreatedById == UserId).ProjectToType<UserAlbumReviewDTO>();
        }
    }
}
