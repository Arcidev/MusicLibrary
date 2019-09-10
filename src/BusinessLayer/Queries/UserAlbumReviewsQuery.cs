using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class UserAlbumReviewsQuery : AppQuery<UserAlbumReviewDTO>
    {
        public int UserId { get; set; }

        public UserAlbumReviewsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<UserAlbumReviewDTO> GetQueryable()
        {
            return Context.AlbumReviews.Where(x => x.CreatedById == UserId).OrderByDescending(x => x.EditDate).ProjectTo<UserAlbumReviewDTO>(mapperConfig);
        }
    }
}
