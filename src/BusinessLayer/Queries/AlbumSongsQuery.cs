using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class AlbumSongsQuery<T> : AppQuery<T>
    {
        public int AlbumId { get; set; }

        public bool? Approved { get; set; }

        public AlbumSongsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<T> GetQueryable()
        {
            var query = Context.AlbumSongs.Where(x => x.AlbumId == AlbumId).Select(x => x.Song);
            if (Approved.HasValue)
                query = query.Where(x => x.Approved == Approved.Value);

            return query.ProjectToType<T>();
        }
    }
}
