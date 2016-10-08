using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class RecentAlbumsQuery : AppQuery<AlbumDTO>
    {
        public RecentAlbumsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            var query = Context.Albums.OrderByDescending(x => x.CreateDate);
            if (Take.HasValue)
                return query.Take(Take.Value).ProjectTo<AlbumDTO>();

            return query.ProjectTo<AlbumDTO>();
        }
    }
}
