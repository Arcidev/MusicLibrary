using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class AlbumsQuery : AppQuery<AlbumDTO>
    {
        public AlbumsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        public int? CategoryId { get; set; }

        public string Filter { get; set; }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            var query = Context.Albums.Where(x => x.Approved);
            if (CategoryId.HasValue)
                query = query.Where(x => x.CategoryId == CategoryId.Value);

            if (!string.IsNullOrEmpty(Filter))
                query = query.Where(x => x.Name.Contains(Filter) || x.Band.Name.Contains(Filter));

            return query.ProjectTo<AlbumDTO>();
        }
    }
}
