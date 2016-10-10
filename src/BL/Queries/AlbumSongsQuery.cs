using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class AlbumSongsQuery : AppQuery<SongDTO>
    {
        public int AlbumId { get; set; }

        public bool? Approved { get; set; }

        public AlbumSongsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<SongDTO> GetQueryable()
        {
            var query = Context.AlbumSongs.Where(x => x.AlbumId == AlbumId).Select(x => x.Song);
            if (Approved.HasValue)
                query = query.Where(x => x.Approved == Approved.Value);

            return query.ProjectTo<SongDTO>();
        }
    }
}
