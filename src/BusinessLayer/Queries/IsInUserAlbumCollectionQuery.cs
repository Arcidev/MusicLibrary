using Riganti.Utils.Infrastructure.Core;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class IsInUserAlbumCollectionQuery : AppQuery<int>
    {
        public int UserId { get; set; }

        public IEnumerable<int> AlbumIds { get; set; }

        public IsInUserAlbumCollectionQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<int> GetQueryable()
        {
            var query = Context.UserAlbums.Where(x => x.UserId == UserId);
            if (AlbumIds != null && AlbumIds.Any())
                query = query.Where(x => AlbumIds.Contains(x.AlbumId));

            return query.Select(x => x.AlbumId);
        }
    }
}
