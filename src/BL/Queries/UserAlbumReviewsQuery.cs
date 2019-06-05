using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class UserAlbumReviewsQuery : AppQuery<UserAlbumReviewDTO>
    {
        public int UserId { get; set; }

        public UserAlbumReviewsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<UserAlbumReviewDTO> GetQueryable()
        {
            return Context.AlbumReviews.Where(x => x.CreatedById == UserId).ProjectTo<UserAlbumReviewDTO>(mapperConfig);
        }
    }
}
