using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class AlbumsQuery<T> : AppQuery<T>
    {
        public AlbumsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        public int? CategoryId { get; set; }

        public int? SongId { get; set; }

        public string Filter { get; set; }

        public bool? Approved { get; set; }

        public bool IncludeBandFilter { get; set; }

        protected override IQueryable<T> GetQueryable()
        {
            var query = Context.Albums.AsQueryable();
            if (Approved.HasValue)
                query = query.Where(x => x.Approved == Approved.Value);

            if (CategoryId.HasValue)
                query = query.Where(x => x.CategoryId == CategoryId.Value);

            if (!string.IsNullOrEmpty(Filter))
                query = query.Where(x => x.Name.Contains(Filter) || (IncludeBandFilter && x.Band.Name.Contains(Filter)));

            if (SongId.HasValue)
                query = query.Where(x => x.AlbumSongs.Any(y => y.SongId == SongId.Value));

            return query.ProjectToType<T>();
        }
    }
}
