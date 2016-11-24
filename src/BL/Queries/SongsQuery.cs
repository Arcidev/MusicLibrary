using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class SongsQuery : AppQuery<SongDTO>
    {
        public SongsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<SongDTO> GetQueryable()
        {
            return Context.Songs.Where(x => x.Approved).ProjectTo<SongDTO>();
        }
    }
}
